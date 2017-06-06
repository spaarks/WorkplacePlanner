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

namespace WorkplacePlanner.Test
{
    public class CalendarServiceTests
    {

        public class GetCalendar
        {
            [Theory]
            [InlineData(1, "2016-1-1", 0, 0)]
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
                    var service = new CalendarService(context);

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
            public void WhenValidDataExists_ReturnsCorrectUsageTypeCounts(int teamId, DateTime month, string personEmail, int ioCount, int wfhCount, int ooCount, int nbdCount, int mhCount)
            {
                var options = Helper.GetContextOptions();

                SetupCalendarTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new CalendarService(context);

                    var calendarRows = service.GetCalendar(teamId, month);
                    var calendarRow = calendarRows.Where(r => r.Person.Email == personEmail).FirstOrDefault();

                    Assert.Equal(ioCount, calendarRow.CalendarEntries.Where(e => e.UsageTypeId == 1).Count());
                    Assert.Equal(wfhCount, calendarRow.CalendarEntries.Where(e => e.UsageTypeId == 2).Count());
                    Assert.Equal(ooCount, calendarRow.CalendarEntries.Where(e => e.UsageTypeId == 3).Count());
                    Assert.Equal(nbdCount, calendarRow.CalendarEntries.Where(e => e.UsageTypeId == 4).Count());
                    Assert.Equal(mhCount, calendarRow.CalendarEntries.Where(e => e.UsageTypeId == 5).Count());
                }
            }

            [Theory]
            [InlineData(1, "2017-6-1", "alex@yopmail.com", "2017-6-6", 2)]
            [InlineData(1, "2017-6-1", "alex@yopmail.com", "2017-6-7", 2)]
            [InlineData(1, "2017-6-1", "alex@yopmail.com", "2017-6-10", 4)]
            [InlineData(1, "2017-6-1", "alex@yopmail.com", "2017-6-15", 1)]
            [InlineData(1, "2017-6-1", "alex@yopmail.com", "2017-6-22", 3)]
            [InlineData(1, "2017-6-1", "alex@yopmail.com", "2017-6-24", 4)]
            [InlineData(1, "2017-8-1", "alex@yopmail.com", "2017-8-10", 2)]
            [InlineData(1, "2017-8-1", "alex@yopmail.com", "2017-8-14", 1)]
            [InlineData(1, "2017-7-1", "glen@yopmail.com", "2017-7-8", 3)]
            public void WhenValidDataExists_ReturnsExactCalendersEntries(int teamId, DateTime month, string personEmail, DateTime date, int usageTypeId)
            {
                var options = Helper.GetContextOptions();

                SetupCalendarTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new CalendarService(context);

                    var calendarRows = service.GetCalendar(teamId, month);
                    var calendarRow = calendarRows.Where(r => r.Person.Email == personEmail).FirstOrDefault();

                    var calendarEntry = calendarRow.CalendarEntries.Where(e => e.Date == date).FirstOrDefault();

                    Assert.Equal(usageTypeId, calendarEntry.UsageTypeId);
                }
            }
        }

        #region Setup Test Data

        public static void SetupCalendarTestData(DbContextOptions<DataContext> options)
        {
            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();

                context.Teams.AddRange(GetTeams());
                context.People.AddRange(GetPeople());
                context.TeamMemberships.AddRange(GetMemberships());
                context.CalendarEntries.AddRange(GetCalendarEntries());
                context.Settings.AddRange(GetSettings());
                context.UsageTypes.AddRange(GetUsageTypes());
                context.Holidays.AddRange(GetHolidays());

                context.SaveChanges();
            }
        }

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

        private static List<Person> GetPeople()
        {
            var list = new List<Person> {
                CreatePerson(1, "Alex", "Smith", true, "alex@yopmail.com"),
                CreatePerson(2, "Glenn", "Maxwell", true, "glen@yopmail.com"),
                CreatePerson(3, "Adam", "Gilchrist", true, "adam@yopmail.com"),
                CreatePerson(4, "Steve", "Smith", true, "steve@yopmail.com"),
                CreatePerson(5, "Mike", "Pence", true, "mike@yopmail.com"),
                CreatePerson(6, "Yash", "Varma", true, "yash@yopmail.com"),
                CreatePerson(7, "Andrew", "Flintop", false, "andrew@yopmail.com"),
                CreatePerson(8, "Ben", "Stoke", true, "ben@yopmail.com"),
                CreatePerson(9, "Lionel", "Messi", true, "lionel@yopmail.com"),
                CreatePerson(10, "Leo", "Tolstoy", true, "leo@yopmail.com")
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

        private static List<CalendarEntry> GetCalendarEntries()
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
                CreateSetting("WorkingWeekDays", "1,2,3,4,5")
            };

            return settingsList;
        }

        private static List<UsageType> GetUsageTypes()
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

        private static List<Holiday> GetHolidays()
        {
            var listHolidays = new List<Holiday> {
                CreateHoliday(1, new DateTime(2017, 6, 8), "Full moon poya day"),
                CreateHoliday(2, new DateTime(2017, 7, 6), "Ramadan Festival"),
                CreateHoliday(3, new DateTime(2017, 12, 25), "Christmus"),
                CreateHoliday(4, new DateTime(2017, 12, 26), "Boxing day")
            };

            return listHolidays;
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

        private static Person CreatePerson(int id, string firstName, string lastName, bool active, string email)
        {
            return new Person
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
                PersonId = personId,
                StartDate = startDate,
                EndDate = endDate,
            };
        }

        private static CalendarEntry CreateCalendarEntry(int id, int teamMembershipId, DateTime date, string comments, int usageTypeId)
        {
            return new CalendarEntry
            {
                Id = id,
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
    }

    #endregion
}
