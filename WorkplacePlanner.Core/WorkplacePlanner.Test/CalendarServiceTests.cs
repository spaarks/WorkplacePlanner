using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WorkplacePlanner.Data;
using WorkplacePlanner.Data.Entities;
using WorkplacePlanner.Services;
using WorkPlacePlanner.Domain.Dtos.Team;
using WorkPlacePlanner.Domain.Dtos.Calendar;
using Xunit;
using System.Linq;
using WorkplacePlanner.Utills.CustomExceptions;

namespace WorkplacePlanner.Test
{
    public class CalendarServiceTests
    {
        public class GetCalendar
        {
            [Theory]
            [InlineData(1, "2017-1-1", 0, 0)]
            [InlineData(1, "2017-2-1", 1, 28)]
            [InlineData(1, "2017-4-1", 2, 30)]
            [InlineData(1, "2017-5-1", 5, 31)]
            [InlineData(1, "2017-6-1", 5, 30)]
            [InlineData(1, "2017-7-1", 4, 31)]
            [InlineData(1, "2017-8-1", 4, 31)]
            [InlineData(1, "2017-9-1", 4, 30)]
            [InlineData(1, "2017-12-31", 4, 31)]
            [InlineData(1, "2018-2-1", 4, 28)]
            [InlineData(2, "2017-2-1", 0, 0)]
            [InlineData(2, "2017-3-1", 1, 31)]
            [InlineData(2, "2017-4-1", 1, 30)]
            [InlineData(2, "2017-6-1", 3, 30)]
            [InlineData(2, "2017-11-1", 3, 30)]
            [InlineData(2, "2018-1-1", 2, 31)]
            [InlineData(3, "2017-7-1", 1, 31)]
            [InlineData(3, "2020-2-1", 2, 29)]
            public void WhenValidDataExists_ReturnsCorrectRowAndEntryCounts(int teamId, DateTime month, int calendarRowCount, int rowEntryCount)
            {
                var options = Helper.GetContextOptions();

                SetupCalendarTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var calendarRows = service.GetCalendar(teamId, month);

                    Assert.Equal(calendarRowCount, calendarRows.Count);

                    foreach (var row in calendarRows)
                    {
                        Assert.Equal(rowEntryCount, row.CalendarEntries.Count);
                    }
                }
            }

            [Theory]
            [InlineData(1, "2017-6-1", "alex@yopmail.com", 18, 2, 1, 8, 1)]
            [InlineData(1, "2017-7-1", "alex@yopmail.com", 19, 1, 0, 10, 1)]
            [InlineData(1, "2017-8-1", "alex@yopmail.com", 22, 1, 0, 8, 0)]
            [InlineData(1, "2017-12-1", "alex@yopmail.com", 18, 0, 1, 10, 2)]
            [InlineData(1, "2017-5-1", "glen@yopmail.com", 22, 1, 0, 8, 0)]
            [InlineData(1, "2017-6-1", "glen@yopmail.com", 20, 1, 0, 8, 1)]
            [InlineData(1, "2017-7-1", "glen@yopmail.com", 20, 0, 1, 9, 1)]
            public void WhenTeamDefaultUsageTypeNotExists_ReturnsCorrectUsageTypeCounts(int teamId, DateTime month, string personEmail, int ioCount, int wfhCount, int ooCount, int nbdCount, int mhCount)
            {
                var options = Helper.GetContextOptions();

                SetupCalendarTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var calendarRows = service.GetCalendar(teamId, month);
                    var calendarRow = calendarRows.Where(r => r.User.Email == personEmail).FirstOrDefault();

                    Assert.Equal(ioCount, calendarRow.CalendarEntries.Where(e => e.UsageTypeId == 1).Count());
                    Assert.Equal(wfhCount, calendarRow.CalendarEntries.Where(e => e.UsageTypeId == 2).Count());
                    Assert.Equal(ooCount, calendarRow.CalendarEntries.Where(e => e.UsageTypeId == 3).Count());
                    Assert.Equal(nbdCount, calendarRow.CalendarEntries.Where(e => e.UsageTypeId == 4).Count());
                    Assert.Equal(mhCount, calendarRow.CalendarEntries.Where(e => e.UsageTypeId == 5).Count());
                }
            }


            [Theory]
            [InlineData(3, "2017-6-1", "yash@yopmail.com", 0, 21, 0, 8, 1)]
            [InlineData(3, "2017-8-1", "adam@yopmail.com", 23, 0, 0, 8, 0)]
            public void WhenTeamDefaultUsageTypeExists_ReturnsCorrectUsageTypeCounts(int teamId, DateTime month, string personEmail, int ioCount, int wfhCount, int ooCount, int nbdCount, int mhCount)
            {
                var options = Helper.GetContextOptions();

                SetupCalendarTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var calendarRows = service.GetCalendar(teamId, month);
                    var calendarRow = calendarRows.Where(r => r.User.Email == personEmail).FirstOrDefault();

                    Assert.Equal(ioCount, calendarRow.CalendarEntries.Where(e => e.UsageTypeId == 1).Count());
                    Assert.Equal(wfhCount, calendarRow.CalendarEntries.Where(e => e.UsageTypeId == 2).Count());
                    Assert.Equal(ooCount, calendarRow.CalendarEntries.Where(e => e.UsageTypeId == 3).Count());
                    Assert.Equal(nbdCount, calendarRow.CalendarEntries.Where(e => e.UsageTypeId == 4).Count());
                    Assert.Equal(mhCount, calendarRow.CalendarEntries.Where(e => e.UsageTypeId == 5).Count());
                }
            }

            [Theory]
            [InlineData(1, "2017-6-1", "alex@yopmail.com", "2017-6-6", 2, 1)]
            [InlineData(1, "2017-6-1", "alex@yopmail.com", "2017-6-7", 2, 1)]
            [InlineData(1, "2017-6-1", "alex@yopmail.com", "2017-6-10", 4, 1)]
            [InlineData(1, "2017-6-1", "alex@yopmail.com", "2017-6-15", 1, 1)]
            [InlineData(1, "2017-6-1", "alex@yopmail.com", "2017-6-22", 3, 1)]
            [InlineData(1, "2017-6-1", "alex@yopmail.com", "2017-6-24", 4, 1)]
            [InlineData(1, "2017-8-1", "alex@yopmail.com", "2017-8-10", 2, 1)]
            [InlineData(1, "2017-8-1", "alex@yopmail.com", "2017-8-14", 1, 1)]
            [InlineData(1, "2017-7-1", "glen@yopmail.com", "2017-7-8", 3, 2)]
            public void WhenValidDataExists_ReturnsExactCalendersEntries(int teamId, DateTime month, string personEmail, DateTime date, int usageTypeId, int teamMembershipId)
            {
                var options = Helper.GetContextOptions();

                SetupCalendarTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var calendarRows = service.GetCalendar(teamId, month);
                    var calendarRow = calendarRows.Where(r => r.User.Email == personEmail).FirstOrDefault();

                    var calendarEntry = calendarRow.CalendarEntries.Where(e => e.Date == date).FirstOrDefault();

                    Assert.Equal(usageTypeId, calendarEntry.UsageTypeId);
                    Assert.Equal(teamMembershipId, calendarEntry.TeamMembershipId);

                }
            }
        }

        public class GetCalendarEntries
        {
            [Theory]
            
            [InlineData(1, "2017-1-1", 31)]
            [InlineData(1, "2017-2-1", 28)]
            [InlineData(1, "2017-4-1", 30)]
            [InlineData(1, "2017-5-1", 31)]
            [InlineData(1, "2017-6-1", 30)]
            [InlineData(1, "2017-7-1", 31)]
            [InlineData(1, "2017-8-1", 31)]
            [InlineData(1, "2017-9-1", 30)]
            [InlineData(1, "2017-12-31",31)]
            [InlineData(1, "2018-2-1", 28)]
            [InlineData(2, "2017-2-1", 28)]
            [InlineData(2, "2017-3-1", 31)]
            [InlineData(2, "2017-4-1", 30)]
            [InlineData(2, "2017-6-1", 30)]
            [InlineData(2, "2017-11-1", 30)]
            [InlineData(2, "2018-1-1", 31)]
            [InlineData(2, "2020-2-1", 29)]
            public void WhenValidDataExists_ReturnsCorrectEntryCounts(int teamMembershipId, DateTime month, int calendarEntryCount)
            {
                var options = Helper.GetContextOptions();

                SetupCalendarTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var calendarEntries = service.GetCalendarEntries(teamMembershipId, month);

                    Assert.Equal(calendarEntryCount, calendarEntries.Count);
                }
            }

            [Theory]
            [InlineData(1, "2017-6-1", "2017-6-6", 2, "Unit Testing-Working From Home")]
            [InlineData(1, "2017-6-1", "2017-6-15", 1, "Unit Testing-In Office")]
            [InlineData(1, "2017-6-1", "2017-6-8", 5, "")]
            [InlineData(1, "2017-6-1", "2017-6-25", 4, "")]
            [InlineData(2, "2017-7-1", "2017-7-8", 3, "Unit Testing-Out Of Office")]
            public void WhenValidDataExists_ReturnsCorrectUsageTypes(int teamMembershipId, DateTime month, DateTime testDate, int expectedUsageTypeId, string expectedComment)
            {
                var options = Helper.GetContextOptions();

                SetupCalendarTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var calendarEntries = service.GetCalendarEntries(teamMembershipId, month);

                    var calendarEntry = calendarEntries.Where(e => e.Date == testDate).First();

                    Assert.Equal(expectedUsageTypeId, calendarEntry.UsageTypeId);
                    Assert.Equal(expectedComment, calendarEntry.Comment);
                }
            }

            [Fact]
            public void WhenPassingIncorrentMembershipId_ThrowsException()
            {
                var options = Helper.GetContextOptions();

                SetupCalendarTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    Assert.Throws<NullReferenceException>(() => service.GetCalendarEntries(1001, new DateTime(2017, 7, 1)));
                }
            }
        }

        public class UpdateCalendar
        {
            public int InvalidDateRageException { get; private set; }

            [Theory]
            [InlineData(1, "2017-6-7", 3, "Out of office - Unit test")]
            [InlineData(1, "2017-6-12", 2, "Work from home - Unit test")]
            [InlineData(1, "2017-12-14", 1, "Comt to Office - Unit test")]
            [InlineData(2, "2017-6-6", 1, "Out of office - Unit Test")]
            public void SingleDayAtATime_UpdateSuccessfully(int teamMembershipId, DateTime date, int usageTypeId, string comment)
            {
                var options = Helper.GetContextOptions();

                SetupCalendarTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var data = new CalendarUpdateDto {
                        TeamMembershipId = teamMembershipId,
                        StartDate = date,
                        EndDate = date,
                        UsageTypeId = usageTypeId,
                        Comment = comment
                    };

                    service.UpdateCalendar(data);

                    var calendarEntries = service.GetCalendarEntries(teamMembershipId, date);

                    var updatedEntry = calendarEntries.Where(e => e.Date == date).FirstOrDefault();

                    ValidateCalendarEntry(updatedEntry, date, usageTypeId, comment);
                }
            }

            [Theory]
            [InlineData(1, "2017-6-4", 3, "Out of office - Unit test")]
            [InlineData(1, "2017-6-24", 2, "Work from home - Unit test")]
            [InlineData(1, "2017-12-23", 1, "Comt to Office - Unit test")]
            [InlineData(2, "2018-2-24", 1, "Out of office - Unit Test")]
            public void DayOnWeekend_DoNotUpdate(int teamMembershipId, DateTime date, int usageTypeId, string comment)
            {
                var options = Helper.GetContextOptions();

                SetupCalendarTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var data = new CalendarUpdateDto
                    {
                        TeamMembershipId = teamMembershipId,
                        StartDate = date,
                        EndDate = date,
                        UsageTypeId = usageTypeId,
                        Comment = comment
                    };

                    service.UpdateCalendar(data);

                    var calendarEntries = service.GetCalendarEntries(teamMembershipId, date);

                    var updatedEntry = calendarEntries.Where(e => e.Date == date).FirstOrDefault();

                    ValidateCalendarEntry(updatedEntry, date, 4, "");
                }
            }

            [Theory]
            [InlineData(1, "2017-6-8", 3, "Out of office - Unit test")]
            [InlineData(1, "2017-7-6", 2, "Work from home - Unit test")]
            [InlineData(1, "2017-12-25", 1, "Comt to Office - Unit test")]
            [InlineData(2, "2017-12-26", 1, "Out of office - Unit Test")]
            public void Holiday_DoNotUpdate(int teamMembershipId, DateTime date, int usageTypeId, string comment)
            {
                var options = Helper.GetContextOptions();

                SetupCalendarTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var data = new CalendarUpdateDto
                    {
                        TeamMembershipId = teamMembershipId,
                        StartDate = date,
                        EndDate = date,
                        UsageTypeId = usageTypeId,
                        Comment = comment
                    };

                    service.UpdateCalendar(data);

                    var calendarEntries = service.GetCalendarEntries(teamMembershipId, date);

                    var updatedEntry = calendarEntries.Where(e => e.Date == date).FirstOrDefault();

                    ValidateCalendarEntry(updatedEntry, date, 5, "");
                }
            }

            [Theory]
            [InlineData(1, "2017-6-12", "2017-6-16",  3, "Out of office - Unit test")]
            [InlineData(1, "2017-6-19", "2017-6-23", 2, "Work from home - Unit test")]
            [InlineData(1, "2017-10-16", "2017-10-20", 3, "Out of office - Unit test")]
            [InlineData(1, "2017-12-12", "2017-12-14", 3, "In Office - Unit Test")]
            public void MultipleDaysInSameWeek_UpdateSuccessfully(int teamMembershipId, DateTime startDate, DateTime endDate, int usageTypeId, string comment)
            {
                var options = Helper.GetContextOptions();

                SetupCalendarTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var data = new CalendarUpdateDto
                    {
                        TeamMembershipId = teamMembershipId,
                        StartDate = startDate,
                        EndDate = endDate,
                        UsageTypeId = usageTypeId,
                        Comment = comment
                    };

                    service.UpdateCalendar(data);

                    var calendarEntries = service.GetCalendarEntries(teamMembershipId, startDate);

                    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                    {
                        var updatedEntry = calendarEntries.Where(e => e.Date == date).FirstOrDefault();
                        ValidateCalendarEntry(updatedEntry, date, usageTypeId, comment);
                    }
                }
            }

            [Theory]
            [InlineData(1, "2017-6-1", "2017-6-30", 3, "Out of office - Unit test")]
            [InlineData(1, "2017-12-1", "2017-12-31", 2, "Working from home - Unit test")]
            [InlineData(1, "2017-8-1", "2017-8-31", 1, "In office - Unit test")]
            public void MultipleDaysInSameMonth_IgnoreNonWorkingDays(int teamMembershipId, DateTime startDate, DateTime endDate, int usageTypeId, string comment)
            {
                var options = Helper.GetContextOptions();

                SetupCalendarTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var data = new CalendarUpdateDto
                    {
                        TeamMembershipId = teamMembershipId,
                        StartDate = startDate,
                        EndDate = endDate,
                        UsageTypeId = usageTypeId,
                        Comment = comment
                    };

                    service.UpdateCalendar(data);

                    var calendarEntries = service.GetCalendarEntries(teamMembershipId, startDate);

                    var workingDays = service.GetWorkingWeekDays();
                    var holidays = service.GetHolidaysOfMonth(startDate);

                    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                    {
                        var updatedEntry = calendarEntries.Where(e => e.Date == date).FirstOrDefault();

                        if (!workingDays.Contains((int)date.DayOfWeek))
                        {
                            ValidateCalendarEntry(updatedEntry, date, 4, "");
                        }
                        else if (holidays.Any(h => h.Date == date))
                        {
                            ValidateCalendarEntry(updatedEntry, date, 5, "");
                        }
                        else
                        {
                            ValidateCalendarEntry(updatedEntry, date, usageTypeId, comment);
                        }                      
                    }
                }
            }

            [Theory]
            [InlineData(1, "2017-1-1", "2017-12-31", 3, "Out of office - Unit test")]
            [InlineData(1, "2020-1-1", "2020-12-31", 2, "Work from home - Unit test")]
            public void EntireYear_IgnoreNonWorkingDays(int teamMembershipId, DateTime startDate, DateTime endDate, int usageTypeId, string comment)
            {
                var options = Helper.GetContextOptions();

                SetupCalendarTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var data = new CalendarUpdateDto
                    {
                        TeamMembershipId = teamMembershipId,
                        StartDate = startDate,
                        EndDate = endDate,
                        UsageTypeId = usageTypeId,
                        Comment = comment
                    };

                    service.UpdateCalendar(data);

                    var calendarEntries = service.GetCalendarEntries(teamMembershipId, startDate);
                    int month = startDate.Month;

                    var workingDays = service.GetWorkingWeekDays();
                    var holidays = service.GetHolidays(startDate, endDate);

                    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                    {
                        if (month != date.Month)
                        {
                            month = date.Month;
                            calendarEntries = service.GetCalendarEntries(teamMembershipId, date);
                        }

                        var updatedEntry = calendarEntries.Where(e => e.Date == date).FirstOrDefault();

                        if (!workingDays.Contains((int)date.DayOfWeek))
                        {
                            ValidateCalendarEntry(updatedEntry, date, 4, "");
                        }
                        else if (holidays.Any(h => h.Date == date))
                        {
                            ValidateCalendarEntry(updatedEntry, date, 5, "");
                        }
                        else
                        {
                            ValidateCalendarEntry(updatedEntry, date, usageTypeId, comment);
                        }
                    }
                }
            }

            [Fact]
            public void WhenPassingInvalidDateRange_ThrowsInvalidDateRangeExceptionn()
            {
                var options = Helper.GetContextOptions();

                SetupCalendarTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var data = new CalendarUpdateDto
                    {
                        TeamMembershipId = 1,
                        StartDate = new DateTime(2017, 6, 2),
                        EndDate = new DateTime(2017, 6, 1),
                        UsageTypeId = 2,
                        Comment = "Unit Testing"
                    };

                    Assert.Throws<InvalidDateRangeException>(() => service.UpdateCalendar(data));
                }
            }
        }

        public class GetHolidays
        {
            [Theory]
            [InlineData("2017-1-1","2017-6-1",0)]
            [InlineData("2017-6-1", "2017-7-1", 1)]
            [InlineData("2017-6-8", "2017-6-8", 1)]
            [InlineData("2017-6-1", "2017-8-1", 2)]
            [InlineData("2017-6-1", "2018-1-1", 4)]
            public void WhenPassingDateRange_ReturnHolidays(DateTime stateDate, DateTime endDate, int expectedHolidayCount)
            {
                var options = Helper.GetContextOptions();

                SetupHolidaysTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var holidays = service.GetHolidays(stateDate, endDate);

                    Assert.Equal(expectedHolidayCount, holidays.Count);
                }
            }
        }

        public class GetHolidaysOfMonth
        {
            [Theory]
            [InlineData("2017-5-1", 0)]
            [InlineData("2017-6-1", 1)]
            [InlineData("2017-7-1", 1)]
            [InlineData("2017-8-1", 0)]
            [InlineData("2017-12-1", 2)]
            public void WhenPassingMonth_ReturnHolidays(DateTime month, int expectedHolidayCount)
            {
                var options = Helper.GetContextOptions();

                SetupHolidaysTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var holidays = service.GetHolidaysOfMonth(month);

                    Assert.Equal(expectedHolidayCount, holidays.Count);
                }
            }

            [Theory]
            [InlineData("2017-6-1", "2017-6-8")]
            [InlineData("2017-7-1", "2017-7-6")]
            [InlineData("2017-12-1", "2017-12-25")]
            [InlineData("2017-12-1", "2017-12-26")]
            public void WhenPassingMonth_ReturnExactlyHolidays(DateTime month, DateTime expectedHoliday)
            {
                var options = Helper.GetContextOptions();

                SetupHolidaysTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var holidays = service.GetHolidaysOfMonth(month);

                    Assert.True(holidays.Any(h => h.Date == expectedHoliday));
                }
            }
        }

        public class GetWorkingWeekDays
        {
            [Theory]
            [InlineData(0, false)]
            [InlineData(1, true)]
            [InlineData(2, true)]
            [InlineData(3, true)]
            [InlineData(4, true)]
            [InlineData(5, true)]
            [InlineData(6, false)]
            public void ReturnWorkingWeekdays(int weekDay, bool isWeekDay)
            {
                var options = Helper.GetContextOptions();

                SetupSettingsTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var workingWeekDays = service.GetWorkingWeekDays();

                    Assert.Equal(isWeekDay, workingWeekDays.Contains(weekDay));
                }
            }
        }
      
        public class GetUsageTypes
        {
            [Fact]
            public void WhenDataExists_ReturnsAll()
            {
                var options = Helper.GetContextOptions();

                SetupUsageTypesTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateCalendarService(context);

                    var usageTypes = service.GetUsageTypes();

                    Assert.Equal(5, usageTypes.Count);
                    Assert.True(usageTypes.Any(u => u.Abbreviation == "IO"));
                    Assert.True(usageTypes.Any(u => u.Abbreviation == "WFH"));
                    Assert.False(usageTypes.Any(u => u.Abbreviation == ""));
                }
            }
        }

        #region Initialize Service

        public static CalendarService CreateCalendarService(DataContext context)
        {
            var settingsService = new SettingsService(context);
            var teamService = new TeamService(context);
            var calendarService = new CalendarService(context, settingsService, teamService);
            return calendarService;
        }

        #endregion

        #region Setup Test Data

        public static void SetupCalendarTestData(DbContextOptions<DataContext> options)
        {
            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();

                context.Teams.AddRange(GetTeams());
                context.Users.AddRange(GetUsers());
                context.TeamMemberships.AddRange(GetMemberships());
                context.CalendarEntries.AddRange(GetCalendarEntriesList());
                context.Settings.AddRange(GetSettings());
                context.UsageTypes.AddRange(GetUsageTypesList());
                context.Holidays.AddRange(GetHolidaysList());
                context.TeamDefaultUsageTypes.AddRange(GetTeamDefaultUsageType());
                context.GlobalDefaultUsageTypes.AddRange(GetGlobalDefaultUsageType());
                context.TeamManagers.AddRange(GetTeamManagers());

                context.SaveChanges();
            }
        }

        public static void SetupHolidaysTestData(DbContextOptions<DataContext> options)
        {
            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();

                context.Holidays.AddRange(GetHolidaysList());

                context.SaveChanges();
            }
        }

        public static void SetupSettingsTestData(DbContextOptions<DataContext> options)
        {
            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();
                                
                context.Settings.AddRange(GetSettings());
               
                context.SaveChanges();
            }
        }

        public static void SetupUsageTypesTestData(DbContextOptions<DataContext> options)
        {
            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();
               
                context.UsageTypes.AddRange(GetUsageTypesList());

                context.SaveChanges();
            }
        }

        #endregion

        #region Test Data Preparation

        private static List<Team> GetTeams()
        {
            var list = new List<Team> {
                CreateTeam(1, "Team 1", 5, true, true, null),
                CreateTeam(2, "Team 2", 10, true, false, null),
                CreateTeam(3, "Team 3", 15, false, true, null),
                CreateTeam(4, "Team 4", 20, true, true, null)
            };

            return list;
        }

        private static List<ApplicationUser> GetUsers()
        {
            var list = new List<ApplicationUser> {
                CreateUser(1, "Alex", "Smith", true, "alex@yopmail.com"),
                CreateUser(2, "Glenn", "Maxwell", true, "glen@yopmail.com"),
                CreateUser(3, "Adam", "Gilchrist", true, "adam@yopmail.com"),
                CreateUser(4, "Steve", "Smith", true, "steve@yopmail.com"),
                CreateUser(5, "Mike", "Pence", true, "mike@yopmail.com"),
                CreateUser(6, "Yash", "Varma", true, "yash@yopmail.com"),
                CreateUser(7, "Andrew", "Flintop", false, "andrew@yopmail.com"),
                CreateUser(8, "Ben", "Stoke", true, "ben@yopmail.com"),
                CreateUser(9, "Lionel", "Messi", true, "lionel@yopmail.com"),
                CreateUser(10, "Leo", "Tolstoy", true, "leo@yopmail.com"),
                CreateUser(100, "Sauron", "Manager", true, "Sauron@yopmail.com"),
                CreateUser(101, "Prodo", "Manager", true, "Prodo@yopmail.com"),
                CreateUser(102, "Bilbo", "Manager", true, "Bilbo@yopmail.com"),
                CreateUser(103, "Piping", "Manager", true, "Piping@yopmail.com")
            };

            return list;
        }

        private static List<TeamMembership> GetMemberships()
        {
            var list = new List<TeamMembership> {
                CreateMembership(1, 1, 1, new DateTime(2017, 6, 1), null),
                CreateMembership(2, 1, 2, new DateTime(2017, 5, 1), null),
                CreateMembership(3, 1, 3, new DateTime(2017, 4, 1), new DateTime(2017, 7, 31)),
                CreateMembership(4, 1, 4, new DateTime(2017, 2, 1), null),
                CreateMembership(5, 1, 5, new DateTime(2017, 8, 1), null),
                CreateMembership(6, 1, 6, new DateTime(2017, 5, 1), new DateTime(2017, 6, 1)),
                CreateMembership(7, 1, 7, new DateTime(2017, 5, 1), new DateTime(2017, 5, 25)),

                CreateMembership(8, 2, 8, new DateTime(2017, 6, 1), null),
                CreateMembership(9, 2, 9, new DateTime(2017, 6, 1), null),
                CreateMembership(10, 2, 10, new DateTime(2017, 3, 1), new DateTime(2017, 12, 31)),

                CreateMembership(11, 3, 6, new DateTime(2017, 6, 2), null),
                CreateMembership(12, 3, 3, new DateTime(2017,8, 1), null),
            };

            return list;
        }

        private static List<CalendarEntry> GetCalendarEntriesList()
        {
            var calendarEntries = new List<CalendarEntry> {
                CreateCalendarEntry(1, 1, new DateTime(2017, 6, 6), "Unit Testing-Working From Home", 2),
                CreateCalendarEntry(2, 1, new DateTime(2017, 6, 7), "Unit Testing-Working From Home", 2),
                CreateCalendarEntry(3, 1, new DateTime(2017, 6, 15), "Unit Testing-In Office", 1),
                CreateCalendarEntry(4, 1, new DateTime(2017, 6, 22), "Unit Testing-Out Of Office", 3),
                CreateCalendarEntry(5, 1, new DateTime(2017, 7, 5), "Unit Testin-Working From Home", 2),
                CreateCalendarEntry(6, 1, new DateTime(2017, 8, 10), "Unit Testing-Working From Home", 2),
                CreateCalendarEntry(7, 1, new DateTime(2017, 12, 14), "Unit Testing-Out Of Office", 3),
                CreateCalendarEntry(8, 2, new DateTime(2017, 5, 8), "Unit Testing-Working From Home", 2),
                CreateCalendarEntry(9, 2, new DateTime(2017, 6, 6), "Unit Testing-Working From Home", 2),
                CreateCalendarEntry(10, 2, new DateTime(2017, 7, 8), "Unit Testing-Out Of Office", 3),
            };

            return calendarEntries;
        }

        private static List<Setting> GetSettings()
        {
            var settingsList = new List<Setting> {
                CreateSetting("WorkingWeekDays", "1,2,3,4,5"),
                CreateSetting("UnEditableUsageTypes", "NBD,MH")                
            };

            return settingsList;
        }

        private static List<UsageType> GetUsageTypesList()
        {
            var listUsageTypes = new List<UsageType> {
                CreateUsageType(1, "In Office", "Working from office", "#f5f5f5", "IO", true, true),
                CreateUsageType(2, "Work From Home", "Working from office", "#4169e1", "WFH", true, true),
                CreateUsageType(3, "Out Of Office", "Not coming to office", "#696969", "OO", true, true),
                CreateUsageType(4, "Non Business Day", "Not a working day", "#b8c0cb", "NBD", true, false),
                CreateUsageType(5, "Mercantile Holiday", "Mercantile hiliday", "#b8c0cb", "MH", true, false)
            };

            return listUsageTypes;
        }

        private static List<Holiday> GetHolidaysList()
        {
            var listHolidays = new List<Holiday> {
                CreateHoliday(1, new DateTime(2017, 6, 8), "Full moon poya day"),
                CreateHoliday(2, new DateTime(2017, 7, 6), "Ramadan Festival"),
                CreateHoliday(3, new DateTime(2017, 12, 25), "Christmus"),
                CreateHoliday(4, new DateTime(2017, 12, 26), "Boxing day")
            };

            return listHolidays;
        }

        private static List<TeamDefaultUsageType> GetTeamDefaultUsageType()
        {
            var listTeamDefaultUsageTypes = new List<TeamDefaultUsageType>
            {
                CreateTeamDefaultUsageType(1, 2, 2, new DateTime(2017, 1, 1), null),
                CreateTeamDefaultUsageType(2, 3, 2, new DateTime(2017, 1, 1), new DateTime(2017, 6, 30)),
                CreateTeamDefaultUsageType(3, 3, 1, new DateTime(2017, 7, 1), null)
            };

            return listTeamDefaultUsageTypes;
        }

        private static List<GlobalDefaultUsageType> GetGlobalDefaultUsageType()
        {
            var listGlobalDefaultUsageTypes = new List<GlobalDefaultUsageType>
            {
                CreateGlobalDefaultUsageType(1, 1, new DateTime(2017, 1, 1), null)
            };

            return listGlobalDefaultUsageTypes;
        }

        private static TeamManager[] GetTeamManagers()
        {
            var listManagers = new TeamManager[] {
                CreateTeamManager(1, 1, 100, new DateTime(2017, 1, 1), new DateTime(2017, 4, 30)),
                CreateTeamManager(2, 1, 101, new DateTime(2017, 3, 1), null),
                CreateTeamManager(3, 2, 100, new DateTime(2017, 5, 1), null),
                CreateTeamManager(4, 3, 102, new DateTime(2017, 2, 1), null),
                CreateTeamManager(5, 3, 103, new DateTime(2017, 3, 1), null)
            };

            return listManagers;
        }

        private static Team CreateTeam(int id, string name, int deskCount, bool active, bool emailNotificationEnabled, int? parentTeamId)
        {
            return new Team
            {
                Id = id,
                Name = name,
                Active = active,
                DeskCount = deskCount,
                EmailNotificationEnabled = emailNotificationEnabled,
                ParentTeamId = parentTeamId
            };
        }

        private static ApplicationUser CreateUser(int id, string firstName, string lastName, bool active, string email)
        {
            return new ApplicationUser
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Active = active,
                Email = email,
            };
        }

        private static TeamMembership CreateMembership(int id, int teamId, int personId, DateTime startDate, DateTime? endDate)
        {
            return new TeamMembership
            {
                Id = id,
                TeamId = teamId,
                UserId = personId,
                StartDate = startDate,
                EndDate = endDate,
            };
        }

        private static TeamManager CreateTeamManager(int id, int teamId, int personId, DateTime startDate, DateTime? endDate)
        {
            return new TeamManager
            {
                Id = id,
                TeamId = teamId,
                UserId = personId,
                StartDate = startDate,
                EndDate = endDate
            };
        }

        private static CalendarEntry CreateCalendarEntry(int id, int teamMembershipId, DateTime date, string comments, int usageTypeId)
        {
            return new CalendarEntry
            {
                //Id = id,
                TeamMembershipId = teamMembershipId,
                Date = date,
                Comments = comments,
                UsageTypeId = usageTypeId
            };
        }

        private static Setting CreateSetting(string name, string value)
        {
            return new Setting
            {
                Name = name,
                Value = value
            };
        }

        private static UsageType CreateUsageType(int id, string name, string description, string colorCode, string abbreviation, bool active, bool selectable)
        {
            return new UsageType
            {
                Id = 0,
                Name = name,
                Abbreviation = abbreviation,
                Active = active,
                ColorCode = colorCode,
                Description = description,
                Selectable = selectable
            };
        }

        private static Holiday CreateHoliday(int id, DateTime date, string reason)
        {
            return new Holiday
            {
                Id = id,
                Date = date,
                Reason = reason
            };
        }

        private static TeamDefaultUsageType CreateTeamDefaultUsageType(int id, int teamId, int usageTypeId, DateTime startDate, DateTime? endDate)
        {
            return new TeamDefaultUsageType
            {
                Id = id,
                TeamId = teamId,
                UsageTypeId = usageTypeId,
                StartDate = startDate,
                EndDate = endDate
            };
        }

        private static GlobalDefaultUsageType CreateGlobalDefaultUsageType(int id, int usageTypeId, DateTime startDate, DateTime? endDate)
        {
            return new GlobalDefaultUsageType
            {
                Id = id,
                UsageTypeId = usageTypeId,
                StartDate = startDate,
                EndDate = endDate
            };
        }

        #endregion

        #region Validation

        public static void ValidateCalendarEntry(CalendarEntryDto calendarEntry, DateTime expectedDate, int expectedUsageTypeId, string expectedComment)
        {
            Assert.Equal(expectedDate, calendarEntry.Date);
            Assert.Equal(expectedUsageTypeId, calendarEntry.UsageTypeId);
            Assert.Equal(expectedComment, calendarEntry.Comment);
        }
 
        #endregion
    }
}

