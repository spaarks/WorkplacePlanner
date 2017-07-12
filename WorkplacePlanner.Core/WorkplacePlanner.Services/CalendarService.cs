using System;
using System.Collections.Generic;
using System.Linq;
using WorkplacePlanner.Data;
using WorkplacePlanner.Data.Entities;
using WorkplacePlanner.Utills.CustomExceptions;
using WorkPlacePlanner.Domain.Dtos.Calendar;
using WorkPlacePlanner.Domain.Dtos.User;
using WorkPlacePlanner.Domain.Services;

namespace WorkplacePlanner.Services
{
    public class CalendarService : ICalendarService
    {
        DataContext _dataContext;
        ISettingsService _settingsService;
        ITeamService _teamService;

        public CalendarService(DataContext context, ISettingsService settingsService, ITeamService teamService)
        {
            _dataContext = context;
            _settingsService = settingsService;
            _teamService = teamService;
        }

        public List<CalendarRawDto> GetCalendar(int teamId, DateTime month)
        {
            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);

            string[] nonEditableUsageTypes = _settingsService.Get("UnEditableUsageTypes").Split(',');

            var list = _dataContext.TeamMemberships
                        .Where(m => m.TeamId == teamId
                            && m.StartDate <= endDate  
                            && (m.EndDate == null || m.EndDate >= startDate))
                        .OrderBy(m => m.User.FirstName)
                        .ThenBy(m => m.User.LastName)
                        .Select(m => new CalendarRawDto
                        {
                            User = new UserDto
                            {
                                Id = m.User.Id,
                                FirstName = m.User.FirstName,
                                LastName = m.User.LastName,
                                Email = m.User.Email
                            },
                            CalendarEntries = m.CalendarEntries
                            .Where(e => e.Date >= startDate && e.Date <= endDate)
                            .Select(e => new CalendarEntryDto
                            {
                                Id = e.Id,
                                TeamMembershipId = e.TeamMembershipId,
                                Date = e.Date,
                                Comment = e.Comments,
                                UsageTypeId = e.UsageTypeId,
                                Editable = !nonEditableUsageTypes.Contains(e.UsageType.Abbreviation)
                            }).ToList(),

                            MembershipId = m.Id,
                            HasPermissionToEdit = true //TODO
                        }).ToList();

            FillDefaultCalendarEntries(ref list, teamId, month);

            return list;
        }

        public List<CalendarEntryDto> GetCalendarEntries(int teamMembershipId, DateTime month)
        {
            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);

            string[] nonEditableUsageTypes = _settingsService.Get("UnEditableUsageTypes").Split(',');

            var teamId = _dataContext.TeamMemberships.Where(m => m.Id == teamMembershipId).FirstOrDefault().TeamId;

            var list = _dataContext.TeamMemberships
                            .Where(m => m.Id == teamMembershipId)
                            .SelectMany(m => m.CalendarEntries)
                            .Where(e => e.Date >= startDate && e.Date <= endDate)
                            .Select(e => new CalendarEntryDto
                            {
                                Id = e.Id,
                                TeamMembershipId = e.TeamMembershipId,
                                Date = e.Date,
                                Comment = e.Comments,
                                UsageTypeId = e.UsageTypeId,
                                Editable = !nonEditableUsageTypes.Contains(e.UsageType.Abbreviation)
                            }).ToList();

            FillDefaultCalendarEntries(ref list, teamId, month, teamMembershipId);

            return list;
        }

        public List<Holiday> GetHolidays(DateTime startDate, DateTime endDate)
        {
            var listHolidays = _dataContext.Holidays
                                .Where(h => h.Date >= startDate && h.Date <= endDate)
                                .ToList();

            return listHolidays;
        }

        public List<Holiday> GetHolidaysOfMonth(DateTime month)
        {
            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);

            var listHolidays = GetHolidays(startDate, endDate);

            return listHolidays;
        }
        
        public int[] GetWorkingWeekDays()
        {
            string listWeekDays = _settingsService.Get("WorkingWeekDays");
            int[] arryWeekDays = listWeekDays.Split(',').Select(val => int.Parse(val)).ToArray();
            return arryWeekDays;
        }

        public List<UsageTypeDto> GetUsageTypes()
        {
            var listUsageTypes = _dataContext.UsageTypes.Select(u => new UsageTypeDto
            {
                Id = u.Id,
                Name = u.Name,
                Abbreviation = u.Abbreviation,
                Description = u.Description,
                ColorCode = u.ColorCode,
                Selectable = u.Selectable
            }).ToList();

            return listUsageTypes;
        }

        public void UpdateCalendar(CalendarUpdateDto data)
        {
            if (data.StartDate > data.EndDate)
                throw new InvalidDateRangeException();

            var weekDays = GetWorkingWeekDays();
            var holidays = GetHolidays(data.StartDate, data.EndDate);

            for (DateTime date = data.StartDate; date <= data.EndDate; date = date.AddDays(1))
            {
                if (weekDays.Contains((int)date.DayOfWeek) && !holidays.Any(h => h.Date == date))
                {
                    var calendarEntry = _dataContext.CalendarEntries
                                .Where(e => e.TeamMembershipId == data.TeamMembershipId
                                    && e.Date == date)
                                .FirstOrDefault();

                    if (calendarEntry == null)
                    {
                        calendarEntry = new CalendarEntry
                        {
                            TeamMembershipId = data.TeamMembershipId,
                            UsageTypeId = data.UsageTypeId,
                            Comments = data.Comment,
                            Date = date
                        };

                        _dataContext.CalendarEntries.Add(calendarEntry);
                    }
                    else
                    {
                        calendarEntry.UsageTypeId = data.UsageTypeId;
                        calendarEntry.Comments = data.Comment;

                        _dataContext.CalendarEntries.Update(calendarEntry);
                    }
                }
            }
            _dataContext.SaveChanges();
        }

        #region Private Methods

        private void FillDefaultCalendarEntries(ref List<CalendarRawDto> calendarRows, int teamId, DateTime month)
        {
            List<CalendarEntryDto> monthDetaultEntriesTemplate = GetDetaultEntriesForMonth(teamId, month);
            foreach (var row in calendarRows)
            {
                List<CalendarEntryDto> monthDetaultEntries = monthDetaultEntriesTemplate.Select(e => e.Clone()).ToList();
                monthDetaultEntries.ForEach(e => e.TeamMembershipId = row.MembershipId);
                row.CalendarEntries = MergeCalendarEntries(monthDetaultEntries, row.CalendarEntries);
            }
        }

        private void FillDefaultCalendarEntries(ref List<CalendarEntryDto> calendarEntries, int teamId, DateTime month, int teamMembershipId)
        {
            List<CalendarEntryDto> monthDetaultEntries = GetDetaultEntriesForMonth(teamId, month);
            monthDetaultEntries.ForEach(e => e.TeamMembershipId = teamMembershipId);
            calendarEntries = MergeCalendarEntries(monthDetaultEntries, calendarEntries);
        }

        private List<CalendarEntryDto> MergeCalendarEntries(List<CalendarEntryDto> defaultEntries, List<CalendarEntryDto> existingEntries)
        {
            var mergedList = existingEntries.Union(defaultEntries).OrderBy(e => e.Date).ToList();
            return mergedList;
        }

        private List<CalendarEntryDto> GetDetaultEntriesForMonth(int teamId, DateTime month)
        {
            int[] workingWeekDays = GetWorkingWeekDays();
            var holidays = GetHolidaysOfMonth(month);
            var usageTypes = GetUsageTypes();

            int usageTypeInOffice = _teamService.GetDefaultUsageType(teamId, month);
            int usageTypeNonWorkingDay = usageTypes.Where(u => u.Abbreviation == "NBD").Select(u => u.Id).FirstOrDefault();
            int usageTypeHoliday = usageTypes.Where(u => u.Abbreviation == "MH").Select(u => u.Id).FirstOrDefault();

            List<CalendarEntryDto> listDefaultEntries = new List<CalendarEntryDto>();

            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            for (DateTime day = startDate; day.Month == startDate.Month; day = day.AddDays(1))
            {
                var isHoliday = holidays.Any(h => h.Date == day);
                int usageTypeId = workingWeekDays.Contains((int)day.DayOfWeek) ? (isHoliday ? usageTypeHoliday : usageTypeInOffice) : usageTypeNonWorkingDay;

                listDefaultEntries.Add(new CalendarEntryDto
                {
                    Id = 0,
                    Date = day,
                    Comment = string.Empty,
                    UsageTypeId = usageTypeId,
                    Editable = (usageTypeId == usageTypeInOffice)
                });
            }
            return listDefaultEntries;
        }

        #endregion
    }
}
