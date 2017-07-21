using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkplacePlanner.Data;
using WorkplacePlanner.Data.Entities;
using WorkPlacePlanner.Domain.Dtos.Membership;
using WorkPlacePlanner.Domain.Services;
using Xunit;

namespace WorkplacePlanner.Test
{
    public class MembershipServiceTests
    {
        public class Create
        {
            [Theory]
            [InlineData(1, "100,101,102,103", "2017-7-5", null, 3)]
            [InlineData(2, "100,101", "2017-5-20", null, 1)]
            public void WhenUsersNotBelongsToATeam_AddsSuccessfully(int teamId, string personIds, DateTime startDate, DateTime? endDate, int currentMembersCount)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateMembershipService(context);

                    
                    int[] arrayPersonId = personIds.Split(',').Select(s => int.Parse(s)).ToArray();

                    var data = new TeamMembersXsDto
                    {
                        TeamId = teamId,
                        StartDate = startDate,
                        EndDate = endDate,
                        UserIds = arrayPersonId
                    };

                    service.Create(data);

                    var memberships = service.GetMembersByTeam(teamId, startDate);
                    Assert.Equal(currentMembersCount + arrayPersonId.Count(), memberships.Count);

                    memberships = service.GetMembersByTeam(teamId, startDate.AddDays(-1));
                    Assert.Equal(currentMembersCount, memberships.Count);
                }
            }

            [Theory]
            [InlineData(3, 1, "2017-8-1", null, "1,2" , 2, 3)]
            [InlineData(2, 1, "2017-7-1", "2017-12-31", "1,2", 3, 3)]
            public void WhenUsersBelongsToATeam_AddToNewTeam_RemoveFromOld_Successfully(int newTeamId, int oldTeamId, DateTime startDate, DateTime endDateIn, string personIds, int currentMembersCount, int oldTeamMemberCount)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                DateTime? endDate = null;
                if (endDateIn != DateTime.MinValue)
                    endDate = endDateIn;

                using (var context = new DataContext(options))
                {
                    var service = CreateMembershipService(context);

                    int[] arrayPersonId = personIds.Split(',').Select(s => int.Parse(s)).ToArray();

                    var data = new TeamMembersXsDto
                    {
                        TeamId = newTeamId,
                        StartDate = startDate,
                        EndDate = endDate,
                        UserIds = arrayPersonId
                    };

                    service.Create(data);

                    var memberships = service.GetMembersByTeam(newTeamId, startDate);
                    Assert.Equal(currentMembersCount + arrayPersonId.Length, memberships.Count);
                    
                    memberships = service.GetMembersByTeam(oldTeamId, startDate);
                    Assert.Equal(oldTeamMemberCount - arrayPersonId.Length, memberships.Count);
                }
            }
        }

        public class GetMembersByTeam
        {
            [Theory]
            [InlineData(1, "2017-6-5", 3)]
            [InlineData(1, "2017-12-1", 3)]
            [InlineData(2, "2017-5-1", 1)]
            [InlineData(2, "2017-6-1", 3)]
            [InlineData(2, "2017-2-28", 0)]
            public void WhenPassingDate_ReturnAllActiveMembers(int teamId, DateTime date, int expectedCount)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateMembershipService(context);

                    var memberships = service.GetMembersByTeam(teamId, date);

                    Assert.Equal(expectedCount, memberships.Count);
                }
            }

            [Theory]
            [InlineData(1, "2017-6-1", "alex@yopmail.com,glen@yopmail.com,adam@yopmail.com,yash@yopmail.com")]
            [InlineData(1, "2017-12-1", "alex@yopmail.com,glen@yopmail.com,mike@yopmail.com")]
            [InlineData(2, "2017-3-2", "leo@yopmail.com")]
            public void WhenPassingDate_ReturnAllActiveMembers_CheckExactUsers(int teamId, DateTime date, string emails)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateMembershipService(context);

                    var memberships = service.GetMembersByTeam(teamId, date);

                    string[] arrEmails = emails.Split(',');

                    foreach (var email in arrEmails)
                    {
                        var membership = memberships.Where(m => m.User.Email == email).FirstOrDefault();
                        Assert.NotNull(membership);
                    }                    
                }
            }

            [Theory]
            [InlineData(1, "2017-5-20", "andrew@yopmail.com,steve@yopmail.com")]
            public void WhenPassingDate_InactivePeopleNotReturned(int teamId, DateTime date, string emails)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateMembershipService(context);

                    var memberships = service.GetMembersByTeam(teamId, date);

                    string[] arrEmails = emails.Split(',');

                    foreach (var email in arrEmails)
                    {
                        var membership = memberships.Where(m => m.User.Email == email).FirstOrDefault();
                        Assert.Null(membership);
                    }
                }
            }

            [Theory]
            [InlineData(1, "2017-6-2", "yash@yopmail.com")]
            [InlineData(2, "2017-2-28", "leo @yopmail.com")]
            [InlineData(2, "2018-1-1", "leo @yopmail.com")]            
            public void WhenPassingDate_EndDateMembersNotReturned(int teamId, DateTime date, string emails)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateMembershipService(context);

                    var memberships = service.GetMembersByTeam(teamId, date);

                    string[] arrEmails = emails.Split(',');

                    foreach (var email in arrEmails)
                    {
                        var membership = memberships.Where(m => m.User.Email == email).FirstOrDefault();
                        Assert.Null(membership);
                    }
                }
            }
        }

        public class Delete
        {
            [Theory]
            [InlineData("1,2,3", "2017-7-6", 1, 0)]
            [InlineData("1,2,3", "2017-8-2", 1, 1)]
            [InlineData("10", "2017-10-1", 3, 2)]
            public void PassingCorrectIds_DeleteSuccessfully(string membershipIds, DateTime terminationDate, int teamId, int expectedNewCount)
            {
                int[] arryMemberhipIds = membershipIds.Split(',').Select(i => int.Parse(i)).ToArray();

                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateMembershipService(context);

                    var data = new MembershipDeleteDto
                    {
                        MembershipIds = arryMemberhipIds,
                        TerminationDate = terminationDate
                    };

                    service.Delete(data);

                    var memberships = service.GetMembersByTeam(teamId, terminationDate.AddDays(1));

                    Assert.Equal(expectedNewCount, memberships.Count);
                }
            }
        }

        #region Setup Test Data

        private static MembershipService CreateMembershipService(DataContext context)
        {
            var service = new MembershipService(context);
            return service;
        }

        private static void SetupTestData(DbContextOptions<DataContext> options)
        {
            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();

                context.Teams.AddRange(GetTeams());
                context.UserData.AddRange(GetPeople());
                context.TeamMemberships.AddRange(GetMemberships());

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

        private static List<UserData> GetPeople()
        {
            var list = new List<UserData> {
                CreatePerson(1, "Alex", "Smith", true, "alex@yopmail.com"),
                CreatePerson(2, "Glenn", "Maxwell", true, "glen@yopmail.com"),
                CreatePerson(3, "Adam", "Gilchrist", true, "adam@yopmail.com"),
                CreatePerson(4, "Steve", "Smith", false, "steve@yopmail.com"),
                CreatePerson(5, "Mike", "Pence", true, "mike@yopmail.com"),
                CreatePerson(6, "Yash", "Varma", true, "yash@yopmail.com"),
                CreatePerson(7, "Andrew", "Flintop", false, "andrew@yopmail.com"),
                CreatePerson(8, "Ben", "Stoke", true, "ben@yopmail.com"),
                CreatePerson(9, "Lionel", "Messi", true, "lionel@yopmail.com"),
                CreatePerson(10, "Leo", "Tolstoy", true, "leo@yopmail.com"),
                CreatePerson(100, "Sauron", "Khan", true, "Sauron@yopmail.com"),
                CreatePerson(101, "Prodo", "Kapoor", true, "Prodo@yopmail.com"),
                CreatePerson(102, "Bilbo", "Baggins", true, "Bilbo@yopmail.com"),
                CreatePerson(103, "Piping", "Monro", true, "Piping@yopmail.com")
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

        private static UserData CreatePerson(int id, string firstName, string lastName, bool active, string email)
        {
            return new UserData
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
                //Id = id,
                TeamId = teamId,
                UserId = personId,
                StartDate = startDate,
                EndDate = endDate,
            };
        }

        #endregion
    }
}
