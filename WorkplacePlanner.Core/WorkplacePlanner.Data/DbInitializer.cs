using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkplacePlanner.Data.Entities;

namespace WorkplacePlanner.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            context.EnsureUptoDate();

            if (context.Users.Any())
                return;

            var people = new User[]
                {
                    new User { FirstName = "Mitch", LastName="Marsh", Email="mitch@yupmail.com", Active=true},
                    new User { FirstName = "John", LastName="Smith", Email="john@yupmail.com", Active=true},
                    new User { FirstName = "Mike", LastName="Pell", Email="mike@yupmail.com", Active=true},
                    new User { FirstName = "Alex", LastName="Bevan", Email="alex@yupmail.com", Active=true},
                    new User { FirstName = "David", LastName="Hussey", Email="david@yupmail.com", Active=true},
                    new User { FirstName = "Adam", LastName="Marsh", Email="adam@yupmail.com", Active=true},
                    new User { FirstName = "Yash", LastName="Varma", Email="yash@yupmail.com", Active=true},
                    new User { FirstName = "Ravi", LastName="Weera", Email="ravi@yupmail.com", Active=true},
                    new User { FirstName = "Sachin", LastName="Smith", Email="sachin@yupmail.com", Active=true},
                    new User { FirstName = "Virat", LastName="Kohli", Email="virat@yupmail.com", Active=true}
                };

            context.Users.AddRange(people);
            context.SaveChanges();

            var teams = new Team[]
                {
                    new Team { Name = "Brickendon", DeskCount = 5, Active=true},
                    new Team { Name = "IPG", DeskCount = 8, Active=true},
                    new Team { Name = "Bonafied", DeskCount = 6, Active=true},
                    new Team { Name = "Texapro", DeskCount = 10, Active=true},
                    new Team { Name = "Red Engine", DeskCount = 12, Active=true, ParentTeamId=3},
                    new Team { Name = "Drop Point", DeskCount = 7, Active=true, ParentTeamId = 5}
                };

            context.Teams.AddRange(teams);
            context.SaveChanges();

            var teamManagers = new TeamManager[]
                {
                    new TeamManager{ TeamId = 1, PersonId = 1, StartDate = new DateTime(2017, 6, 1)},
                    new TeamManager{ TeamId = 2, PersonId = 2, StartDate = new DateTime(2017, 6, 1)},
                    new TeamManager{ TeamId = 3, PersonId = 3, StartDate = new DateTime(2017, 6, 1)},
                    new TeamManager{ TeamId = 4, PersonId = 4, StartDate = new DateTime(2017, 6, 1)}
                };

            context.TeamManagers.AddRange(teamManagers);
            context.SaveChanges();

            var teamMemberships = new TeamMembership[]
                   {
                            new TeamMembership{ TeamId = 1, PersonId = 1, StartDate = new DateTime(2017, 6, 1)},
                            new TeamMembership{ TeamId = 1, PersonId = 2, StartDate = new DateTime(2017, 6, 1)},
                            new TeamMembership{ TeamId = 2, PersonId = 3, StartDate = new DateTime(2017, 6, 1)},
                            new TeamMembership{ TeamId = 2, PersonId = 4, StartDate = new DateTime(2017, 6, 1)},
                            new TeamMembership{ TeamId = 3, PersonId = 5, StartDate = new DateTime(2017, 6, 1)},
                            new TeamMembership{ TeamId = 3, PersonId = 6, StartDate = new DateTime(2017, 6, 1)},
                            new TeamMembership{ TeamId = 3, PersonId = 7, StartDate = new DateTime(2017, 6, 1)},
                            new TeamMembership{ TeamId = 1, PersonId = 8, StartDate = new DateTime(2017, 6, 1)},
                            new TeamMembership{ TeamId = 1, PersonId = 9, StartDate = new DateTime(2017, 6, 1)},
                            new TeamMembership{ TeamId = 1, PersonId = 10, StartDate = new DateTime(2017, 6, 1)},
                   };

            context.TeamMemberships.AddRange(teamMemberships);
            context.SaveChanges();

            var usageTypes = new UsageType[]
                {
                    new UsageType{ Name = "In Office", Description="In office", Abbreviation="IO", ColorCode = "#f5f5f5", Selectable=true, Active=true},
                    new UsageType{ Name = "Work From Home", Description="Working from home", Abbreviation="WFH", ColorCode = "#4169e1", Selectable=true, Active=true},
                    new UsageType{ Name = "Out Of Office", Description="Out of office", Abbreviation="OO", ColorCode = "#696969", Selectable=true, Active=true},
                    new UsageType{ Name = "Non Business Day", Description="Non business day", Abbreviation="NBD", ColorCode = "#b8c0cb", Selectable=false, Active=true},
                    new UsageType{ Name = "Mercantile Holiday", Description="Mercantile holiday", Abbreviation="MH", ColorCode = "#b8c0cb", Selectable=false, Active=true}
                };

            context.UsageTypes.AddRange(usageTypes);
            context.SaveChanges();

            var globalDefaultUsageType = new GlobalDefaultUsageType { UsageTypeId = 1, StartDate = new DateTime(2016, 1, 1) };

            context.GlobalDefaultUsageTypes.Add(globalDefaultUsageType);
            context.SaveChanges();

            var calendarEntries = new CalendarEntry[]
                {
                    new CalendarEntry{ TeamMembershipId=1, UsageTypeId=2, Date= new DateTime(2017, 6, 2), Comments="Test data" },
                    new CalendarEntry{ TeamMembershipId=2, UsageTypeId=2, Date= new DateTime(2017, 6, 6), Comments="Test data" },
                    new CalendarEntry{ TeamMembershipId=3, UsageTypeId=3, Date= new DateTime(2017, 6, 7), Comments="Test data" },
                    new CalendarEntry{ TeamMembershipId=4, UsageTypeId=3, Date= new DateTime(2017, 6, 8), Comments="Test data" },
                    new CalendarEntry{ TeamMembershipId=5, UsageTypeId=3, Date= new DateTime(2017, 6, 6), Comments="Test data" },
                    new CalendarEntry{ TeamMembershipId=6, UsageTypeId=2, Date= new DateTime(2017, 6, 7), Comments="Test data" },
                    new CalendarEntry{ TeamMembershipId=7, UsageTypeId=2, Date= new DateTime(2017, 6, 8), Comments="Test data" },
                    new CalendarEntry{ TeamMembershipId=8, UsageTypeId=3, Date= new DateTime(2017, 6, 15), Comments="Test data" },
                    new CalendarEntry{ TeamMembershipId=9, UsageTypeId=3, Date= new DateTime(2017, 6, 16), Comments="Test data" },
                    new CalendarEntry{ TeamMembershipId=10, UsageTypeId=2, Date= new DateTime(2017, 6, 23), Comments="Test data" }
                };

            context.CalendarEntries.AddRange(calendarEntries);
            context.SaveChanges();

            var settings = new Setting[] {
                new Setting { Name = "WorkingWeekDays", Value = "1,2,3,4,5" },
                new Setting { Name = "UnEditableUsageTypes", Value = "NBD,MH" }
            };

            context.Settings.AddRange(settings);
            context.SaveChanges();

            var holidays = new Holiday[]
                {
                    new Holiday{ Date= new DateTime(2017, 5, 9), Reason= "Wesak poya day" },
                    new Holiday{ Date= new DateTime(2017, 6, 8), Reason= "Poson poya day" },
                    new Holiday{ Date= new DateTime(2017, 7, 8), Reason= "Asala poya day" }
                };

            context.Holidays.AddRange(holidays);
            context.SaveChanges();
        }
    }
}
