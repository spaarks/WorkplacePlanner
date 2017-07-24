using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkplacePlanner.Data;
using WorkplacePlanner.Data.Entities;
using WorkplacePlanner.Services;
using WorkplacePlanner.Utills.CustomExceptions;
using WorkPlacePlanner.Domain.Dtos.Team;
using Xunit;

namespace WorkplacePlanner.Test
{
    public class TeamServiceTests
    {
        public class Create
        {
            [Theory]
            [InlineData("Team 1", 5, true, true, null)]
            [InlineData("Team 2", 10, false, true, null)]
            [InlineData("Team 3", 15, false, true, null)]
            [InlineData("Team 4", 250, true, false, null)]
            [InlineData("Team 5", 250, false, false, 3)]
            public void WhenTeamsExists_PassingCorrectData_CreateSuccessfully(string name, int deskCount, bool active, bool emailNotificationEnabled, int? parentTeamId)
            {
                var options = Helper.GetContextOptions();

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    
                    var teamDto = new TeamDto
                    {
                        Name = name,
                        DeskCount = deskCount,
                        Active = active,
                        EmailNotificationEnabled = emailNotificationEnabled,
                        ParentTeamId = parentTeamId
                    };

                    int newTeamId = service.Create(teamDto);

                    var newTeam = service.Get(newTeamId);

                    ValidateTeam(newTeam, newTeamId, name, deskCount, active, emailNotificationEnabled, parentTeamId, 0);
                }
            }

            [Theory]
            [InlineData("Team 10", 5, true, true, null)]
            [InlineData("Team 20", 10, false, true, null)]
            [InlineData("Team 30", 25, false, true, null)]
            [InlineData("Team 40", 500, true, false, null)]
            [InlineData("Team 50", 250, false, false, 3)]
            public void WhenTeamsNotExists_WhenPassingCorrectData_CreateSuccessfully( string name, int deskCount, bool active, bool emailNotificationEnabled, int? parentTeamId)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);

                    var teamDto = new TeamDto
                    {
                        Name = name,
                        DeskCount = deskCount,
                        Active = active,
                        EmailNotificationEnabled = emailNotificationEnabled,
                        ParentTeamId = parentTeamId
                    };

                    int newTeamId = service.Create(teamDto);

                    var newTeam = service.Get(newTeamId);

                    ValidateTeam(newTeam, newTeamId, name, deskCount, active, emailNotificationEnabled, parentTeamId, 0);
                }
            }
        }

        public class Delete
        {
            [Theory]
            [InlineData(1)]
            [InlineData(3)]
            [InlineData(4)]
            public void WhenTeamExists_DeleteSuccessfully(int id)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    service.Delete(id);
                    Assert.Throws<RecordNotFoundException>(() => service.Get(id));
                }
            }

            [Theory]
            [InlineData(100)]
            [InlineData(101)]
            [InlineData(102)]
            public void WhenTeamNotExists_ThrowsException(int id)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    Assert.Throws<ArgumentNullException>(() => service.Delete(id));
                }
            }

            [Fact]
            public void WhenEmpty_ThrowsException()
            {
                var options = Helper.GetContextOptions();

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    Assert.Throws<ArgumentNullException>(() => service.Delete(1));
                }
            }
        }

        public class Get
        {
            [Theory]
            [InlineData(1, "IPG", 4, true, true, null, 2)]
            [InlineData(2, "TexaPro", 8, true, false, 1, 0)]
            [InlineData(5, "RedEngine", 3, true, true, 4, 1)]
            public void WhenValidIdGiven_ReturnsTeam(int id, string name, int deskCount, bool active, bool emailNotificationEnabled, int? parentTeamId, int managerCount)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    var team = service.Get(id);

                    ValidateTeam(team, id, name, deskCount, active, emailNotificationEnabled, parentTeamId, managerCount);
                }
            }

            [Theory]
            [InlineData(10)]
            [InlineData(11)]
            [InlineData(12)]
            public void WhenInvalidIdGiven_ThrowException(int id)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    Assert.Throws<RecordNotFoundException>(() => service.Get(id));
                }
            }

            [Fact]
            public void WhenNoDataExists_ThrowsException()
            {
                var options = Helper.GetContextOptions();

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    Assert.Throws<RecordNotFoundException>(() => service.Get(1));
                }
            }

            [Fact]
            public void WhenManagersExists_ReturnTeamWithCorrectManagers()
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    var team = service.Get(1);

                    ValidateTeam(team, 1, "IPG", 4, true, true, null, 2);

                    var managerEmails = team.Managers.Select(m => m.Email).ToArray();
                    Assert.True(managerEmails.Contains("glen@yopmail.com"));
                    Assert.True(managerEmails.Contains("adam@yopmail.com"));
                    Assert.False(managerEmails.Contains("alex@yopmail.com"));
                }
            }

            [Fact]
            public void WhenManagersNotExists_ReturnTeamWithoutManagers()
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    var team = service.Get(2);

                    ValidateTeam(team, 2, "TexaPro", 8, true, false, 1, 0);
                    Assert.Equal(0, team.Managers.Length);
                }
            }
        }

        public class GetAll
        {
            [Fact]
            public void WhenTeamsExists_ReturnsAll()
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    var teams = service.GetAll();

                    Assert.NotNull(teams);
                    Assert.Equal(8, teams.Count);

                    //Check for managers in one team
                    var team = teams.Where(t => t.Id == 1).FirstOrDefault();

                    ValidateTeam(team, 1, "IPG", 4, true, true, null, 2);

                    var managerEmails = team.Managers.Select(m => m.Email).ToArray();
                    Assert.True(managerEmails.Contains("glen@yopmail.com"));
                    Assert.True(managerEmails.Contains("adam@yopmail.com"));
                    Assert.False(managerEmails.Contains("alex@yopmail.com"));
                }
            }

            [Fact]
            public void WhenTeamsNotExists_ReturnsEmptyList()
            {
                var options = Helper.GetContextOptions();

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    var teams = service.GetAll();

                    Assert.NotNull(teams);
                    Assert.Equal(0, teams.Count);
                }
            }           
        }

        public class GetAllActiveTeams
        {
            [Fact]
            public void WhenTeamsExists_ReturnAllActiveTeams()
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    var teams = service.GetAllActiveTeams();

                    Assert.Equal(7, teams.Count);                    
                }
            }

            [Fact]
            public void DoNotReturnInactiveTeams()
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    var teams = service.GetAllActiveTeams();

                    var team = teams.Where(t => t.Name == "CIC").FirstOrDefault();
                    Assert.Null(team);
                }
            }


            [Theory]
            [InlineData(1, null)]
            [InlineData(2, 1)]
            [InlineData(5, 4)]
            [InlineData(6, 4)]
            [InlineData(7, 5)]
            public void WhenTeamsExists_ReturnCorrentParentTeamId(int teamId, int? expectedParentId)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    var teams = service.GetAllActiveTeams();

                    var team = teams.Where(t => t.Id == teamId).FirstOrDefault();

                    Assert.Equal(expectedParentId, team.ParentTeamId);
                }
            }
        }

        public class GetSubTeams
        {
            [Theory]
            [InlineData(1, 1)]
            [InlineData(2, 0)]
            [InlineData(4, 4)]
            [InlineData(5, 2)]
            [InlineData(7, 1)]
            public void WhenPassingParentId_ReturnAllSubTeams(int parentId, int subTeamCount)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    var teams = service.GetSubTeams(parentId);

                    Assert.Equal(subTeamCount, teams.Count);
                }
            }


            [Fact]
            public void WhenPassingParentId_ReturnSubTeamsWithManagers()
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    var teams = service.GetSubTeams(4);

                    Assert.Equal(4, teams.Count);

                    var team = teams.Where(t => t.Id == 5).FirstOrDefault();

                    ValidateTeam(team, 5, "RedEngine", 3, true, true, 4, 1);

                    var managerEmails = team.Managers.Select(m => m.Email).ToArray();
                    Assert.True(managerEmails.Contains("adam@yopmail.com"));
                    Assert.False(managerEmails.Contains("alex@yopmail.com"));
                }
            }
        }
        
        public class GetDefaultUsageType
        {
            [Theory]
            [InlineData(2, "2017-3-1", 2)]
            [InlineData(3, "2017-3-1", 2)]
            [InlineData(3, "2017-4-1", 3)]
            [InlineData(3, "2017-6-30", 3)]
            [InlineData(3, "2017-8-30", 1)]
            [InlineData(3, "2020-8-30", 1)]
            public void WhenDefaultValuesExists_ReturnUsageType(int teamId, DateTime date, int expectedUsageTypeId)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateTeamService(context);
                    Assert.Equal(expectedUsageTypeId, service.GetDefaultUsageType(teamId, date));
                }
            }

            [Theory]
            [InlineData(1, "2017-3-1", 1)]
            [InlineData(3, "2017-1-15", 1)]
            [InlineData(4, "2017-6-15", 1)]
            public void WhenTeamDefaultNotExists_ReturnsGlobalDefaultUsageType(int teamId, DateTime date, int expectedUsageTypeId)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateTeamService(context);
                    Assert.Equal(expectedUsageTypeId, service.GetDefaultUsageType(teamId, date));
                }
            }

            [Theory]
            [InlineData(1, "2016-1-1")]
            [InlineData(2, "2016-4-1")]
            [InlineData(3, "2016-6-15")]
            public void WhenDefaultUsageTypeNotExists_ThrowsNullReferenceException(int teamId, DateTime date)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateTeamService(context);
                    Assert.Throws<TeamDefaultUsageTypeMissingException>(() => service.GetDefaultUsageType(teamId, date));
                }
            }
        }

        public class Update
        {
            [Theory]
            [InlineData(1, "Team 1", 5, true, true, null, 2)]
            [InlineData(2, "Team 5", 5, true, true, null, 0)]
            [InlineData(3, "Spaarks", 5, true, false, null, 0)]
            [InlineData(4, "IPG Mobile", 5, true, false, 1, 1)]
            public void WhenTeamExists_UpdateSuccessfully(int id, string name, int deskCount, bool active, bool emailNotificationEnabled, int? parentTeamId, int managerCount)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);

                    var teamDto = new TeamDto
                    {
                        Id = id,
                        Name = name,
                        DeskCount = deskCount,
                        Active = active,
                        EmailNotificationEnabled = emailNotificationEnabled,
                        ParentTeamId = parentTeamId
                    };

                    service.Update(teamDto);

                    var newTeam = service.Get(id);

                    ValidateTeam(newTeam, id, name, deskCount, active, emailNotificationEnabled, parentTeamId, managerCount);
                }
            }

            [Theory]
            [InlineData(100, "Bottonwood", 5, true, true, null)]
            [InlineData(102, "Spaarks", 5, true, true, 1)]
            public void WhenTeamNotExists_ThrowsException(int id, string name, int deskCount, bool active, bool emailNotificationEnabled, int? parentTeamId)
            {
                var options = Helper.GetContextOptions();

                SetupTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);

                    var teamDto = new TeamDto
                    {
                        Id = id,
                        Name = name,
                        DeskCount = deskCount,
                        Active = active,
                        EmailNotificationEnabled = emailNotificationEnabled,
                        ParentTeamId = parentTeamId
                    };

                    Assert.Throws<NullReferenceException>(() => service.Update(teamDto));
                }
            }
        }

        #region Setup Test Data

        private static TeamService CreateTeamService(DataContext context)
        {
            var teamService = new TeamService(context);
            return teamService;
        }

        private static void SetupTestData(DbContextOptions<DataContext> options)
        {
            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();

                context.Teams.AddRange(GetTeams());
                context.Users.AddRange(GetUsers());
                context.TeamManagers.AddRange(GetTeamManagers());
                context.TeamDefaultUsageTypes.AddRange(GetTeamDefaultUsageType());
                context.GlobalDefaultUsageTypes.AddRange(GetGlobalDefaultUsageType());

                context.SaveChanges();
            }            
        }

        private static List<Team> GetTeams()
        {
            var listTeams = new List<Team>
            {
                CreateTeam(1, "IPG", 4, true, true, null),
                CreateTeam(2, "TexaPro", 8, true, false, 1),
                CreateTeam(3, "CIC", 2, false, true, null),
                CreateTeam(4, "Bonafied", 9, true, true, null),
                CreateTeam(5, "RedEngine", 3, true, true, 4),
                CreateTeam(6, "STC", 3, true, false, 4),
                CreateTeam(7, "Drop Point", 3, true, false, 5),
                CreateTeam(8, "Drop Point", 3, true, false, 7)
            };

            return listTeams;
        }

        private static List<ApplicationUser> GetUsers()
        {
            var list = new List<ApplicationUser> {
                CreateUser(1, "Alex", "Smith", true, "alex@yopmail.com"),
                CreateUser(2, "Glenn", "Maxwell", true, "glen@yopmail.com"),
                CreateUser(3, "Adam", "Gilchrist", true, "adam@yopmail.com"),
                CreateUser(4, "Steve", "Smith", true, "steve@yopmail.com"),
                CreateUser(5, "Mike", "Pence", true, "mike@yopmail.com")
            };

            return list;
        }

        private static TeamManager[] GetTeamManagers()
        {
            var listManagers = new TeamManager[] {
                CreateTeamManager(1, 1, 1, new DateTime(2017, 1, 1), new DateTime(2017, 4, 30)),
                CreateTeamManager(2, 1, 2, new DateTime(2017, 3, 1), null),
                CreateTeamManager(3, 4, 1, new DateTime(2017, 5, 1), null),
                CreateTeamManager(4, 5, 3, new DateTime(2017, 2, 1), null),
                CreateTeamManager(5, 7, 4, new DateTime(2017, 3, 1), null),
                CreateTeamManager(6, 8, 5, new DateTime(2017, 3, 1), null),
                CreateTeamManager(7, 1, 3, new DateTime(2017, 3, 1), null)
            };

            return listManagers;
        }

        private static List<TeamDefaultUsageType> GetTeamDefaultUsageType()
        {
            var listTeamDefaultUsageTypes = new List<TeamDefaultUsageType>
            {
                CreateTeamDefaultUsageType(1, 2, 2, new DateTime(2017, 1, 1), null),
                CreateTeamDefaultUsageType(2, 3, 2, new DateTime(2017, 2, 1), new DateTime(2017, 3, 31)),
                CreateTeamDefaultUsageType(3, 3, 3, new DateTime(2017, 4, 1), new DateTime(2017, 6, 30)),
                CreateTeamDefaultUsageType(5, 3, 1, new DateTime(2017, 7, 1), null)
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

        private static Team CreateTeam(int id, string name, int deskCount, bool active, bool emailNotificationEnabled, int? parentTeamId)
        {
            return new Team
            {
               // Id = id,
                Name = name,
                Active = active,
                DeskCount = deskCount,
                EmailNotificationEnabled = emailNotificationEnabled,
                ParentTeamId = parentTeamId
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

        #endregion

        #region Validating Data

        private static void ValidateTeam(TeamDto team, int? id, string name, int deskCount, bool active, bool emailNotificationEnabled, int? parentTeamId, int managerCount)
        {
            Assert.NotNull(team);

            if (id != null)
                Assert.Equal(id, team.Id);

            Assert.Equal(name, team.Name);
            Assert.Equal(deskCount, team.DeskCount);
            Assert.Equal(active, team.Active);
            Assert.Equal(emailNotificationEnabled, team.EmailNotificationEnabled);
            Assert.Equal(parentTeamId, team.ParentTeamId);
            Assert.Equal(managerCount, team.Managers.Length);
        }

        #endregion
    }
}
