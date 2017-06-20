using System;
using System.Collections.Generic;
using System.Linq;
using WorkplacePlanner.Data;
using WorkplacePlanner.Data.Entities;
using WorkPlacePlanner.Domain.Dtos.Calendar;
using WorkPlacePlanner.Domain.Dtos.Person;
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
            
            var list = _dataContext.TeamMemberships
                        .Where(m => m.TeamId == teamId
                            && m.StartDate <= endDate  
                            && (m.EndDate == null || m.EndDate >= startDate))
                        .OrderBy(m => m.Person.FirstName)
                        .ThenBy(m => m.Person.LastName)
                        .Select(m => new CalendarRawDto
                        {
                            Person = new PersonDto
                            {
                                Id = m.Person.Id,
                                FirstName = m.Person.FirstName,
                                LastName = m.Person.LastName,
                                Email = m.Person.Email
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
                                Editable = true //TODO
                            }).ToList(),

                            MembershipId = m.Id,
                            HasPermissionToEdit = true //TODO
                        }).ToList();

            FillDefaultCalendarEntries(ref list, teamId, month);

            return list;
        }

        public CalendarMetaDataDto GetCalendarMetaData(int teamId, DateTime date)
        {
            var usageTypes = GetUsageTypes();
            var managers = _dataContext.TeamManagers
                                .Where(m => m.TeamId == teamId
                                        && m.StartDate <= date
                                        && (m.EndDate == null || m.EndDate >= date))
                                 .Select(m => new PersonDto {
                                     Email = m.Person.Email,
                                     FirstName = m.Person.FirstName,
                                     LastName = m.Person.LastName,
                                     Id = m.PersonId
                                 }).ToList();

            var metaDataDto = new CalendarMetaDataDto
            {
                UsageTypes = usageTypes,
                TeamManagers = managers
            };

            return metaDataDto;
        }

        private List<UsageTypeDto> GetUsageTypes()
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
            throw new NotImplementedException();
        }

        #region Private Methods

        private void FillDefaultCalendarEntries(ref List<CalendarRawDto> calendarRows, int teamId, DateTime month)
        {
            List<CalendarEntryDto> monthDetaultEntries = GetDetaultEntriesForMonth(teamId, month);
            foreach (var row in calendarRows)
            {
                monthDetaultEntries.ForEach(e => e.TeamMembershipId = row.MembershipId);
                row.CalendarEntries = MergeCalendarEntries(monthDetaultEntries, row.CalendarEntries);
            }
        }

        private List<CalendarEntryDto> MergeCalendarEntries(List<CalendarEntryDto> defaultEntries, List<CalendarEntryDto> existingEntries)
        {
            var mergedList = existingEntries.Union(defaultEntries).OrderBy(e => e.Date).ToList();
            return mergedList;
        }

        private List<CalendarEntryDto> GetDetaultEntriesForMonth(int teamId, DateTime month)
        {
            int[] workingWeekDays = GetWorkingWeekDays();
            var holidays = GetHolidays(month);
            var usageTypes = GetUsageTypes();

            int usageTypeInOffice = _teamService.GetDefaultUsageType(teamId, month);
            int usageTypeNonWorkingDay = usageTypes.Where(u => u.Abbreviation == "NBD").Select(u => u.Id).FirstOrDefault();
            int usageTypeHoliday = usageTypes.Where(u => u.Abbreviation == "MH").Select(u => u.Id).FirstOrDefault();

            List<CalendarEntryDto> listDefaultEntries = new List<CalendarEntryDto>();

            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            for (DateTime day = startDate; day.Month == startDate.Month; day = day.AddDays(1))
            {
                var holiday = holidays.Where(h => h.Date == day).FirstOrDefault();

                listDefaultEntries.Add(new CalendarEntryDto
                {
                    Id = 0,
                    Date = day,
                    Comment = holiday == null ? string.Empty : holiday.Reason,
                    UsageTypeId = workingWeekDays.Contains((int)day.DayOfWeek) ? (holiday != null ? usageTypeHoliday : usageTypeInOffice): usageTypeNonWorkingDay,
                    Editable = workingWeekDays.Contains((int)day.DayOfWeek)
                });
            }

            return listDefaultEntries;
        }

        private int[] GetWorkingWeekDays()
        {
            string listWeekDays = _settingsService.Get("WorkingWeekDays");
            int[] arryWeekDays = listWeekDays.Split(',').Select(val => int.Parse(val)).ToArray();
            return arryWeekDays;
        }

        private List<Holiday> GetHolidays(DateTime month)
        {
            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);

            var listHolidays = _dataContext.Holidays
                                .Where(h => h.Date >= startDate && h.Date <= endDate)
                                .ToList();

            return listHolidays;
        }

        #endregion
    }
}
