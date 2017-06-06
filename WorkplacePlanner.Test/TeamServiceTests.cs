using Microsoft.EntityFrameworkCore;
using System;
using WorkplacePlanner.Data;
using WorkplacePlanner.Data.Entities;
using WorkplacePlanner.Services;
using WorkPlacePlanner.Domain.Dtos.Team;
using Xunit;

namespace WorkplacePlanner.Test
{
    public class TeamServiceTests
    {
        public class Create
        {
            [Theory]
            [InlineData(1, "Team 1", 5, true, true, null)]
            [InlineData(1, "Team 2", 10, false, true, null)]
            [InlineData(1, "Team 3", 15, false, true, null)]
            [InlineData(1, "Team 4", 250, true, false, null)]
            [InlineData(1, "Team 5", 250, false, false, 3)]
            public void WhenTeamsExists_PassingCorrectData_CreateSuccessfully(int id, string name, int deskCount, bool active, bool emailNotificationEnabled, int? parentTeamId)
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

                    service.Create(teamDto);

                    var newTeam = service.Get(id);

                    ValidateTeam(newTeam, id, name, deskCount, active, emailNotificationEnabled, parentTeamId);
                }
            }

            [Theory]
            [InlineData(6, "Team 10", 5, true, true, null)]
            [InlineData(6, "Team 20", 10, false, true, null)]
            [InlineData(6, "Team 30", 25, false, true, null)]
            [InlineData(6, "Team 40", 500, true, false, null)]
            [InlineData(6, "Team 50", 250, false, false, 3)]
            public void WhenTeamsNotExists_WhenPassingCorrectData_CreateSuccessfully(int id, string name, int deskCount, bool active, bool emailNotificationEnabled, int? parentTeamId)
            {
                var options = Helper.GetContextOptions();

                CreateTestTeams(options);

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

                    service.Create(teamDto);

                    var newTeam = service.Get(id);

                    ValidateTeam(newTeam, id, name, deskCount, active, emailNotificationEnabled, parentTeamId);
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

                CreateTestTeams(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    service.Delete(id);
                    var deletedTeam = service.Get(id);

                    Assert.Null(deletedTeam);
                }
            }

            [Theory]
            [InlineData(100)]
            [InlineData(101)]
            [InlineData(102)]
            public void WhenTeamNotExists_ThrowsException(int id)
            {
                var options = Helper.GetContextOptions();

                CreateTestTeams(options);

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
            [InlineData(1, "IPG", 4, true, true, null )]
            [InlineData(2, "TexaPro", 8, true, false, null)]
            [InlineData(5, "RedEngine", 3, true, true, 4)]
            public void WhenValidIdGiven_ReturnsTeam(int id, string name, int deskCount, bool active, bool emailNotificationEnabled, int? parentTeamId)
            {
                var options = Helper.GetContextOptions();

                CreateTestTeams(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    var team = service.Get(id);

                    ValidateTeam(team, id, name, deskCount, active, emailNotificationEnabled, parentTeamId);
                }
            }

            [Theory]
            [InlineData(10)]
            [InlineData(11)]
            [InlineData(12)]
            public void WhenInvalidIdGiven_ReturnNull(int id)
            {
                var options = Helper.GetContextOptions();

                CreateTestTeams(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    var team = service.Get(id);

                    Assert.Null(team);
                }
            }

            [Fact]
            public void WhenNoDataExists_ReturnNull()
            {
                var options = Helper.GetContextOptions();

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    var team = service.Get(1);

                    Assert.Null(team);
                }
            }
        }

        public class GetAll
        {
            [Fact]
            public void WhenTeamsExists_ReturnsAll()
            {
                var options = Helper.GetContextOptions();

                CreateTestTeams(options);

                using (var context = new DataContext(options))
                {
                    var service = new TeamService(context);
                    var teams = service.GetAll();

                    Assert.NotNull(teams);
                    Assert.Equal(5, teams.Count);
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

        public class Update
        {
            [Theory]
            [InlineData(1, "Team 1", 5, true, true, null)]
            [InlineData(2, "Team 5", 5, true, true, null)]
            [InlineData(3, "Spaarks", 5, true, false, null)]
            [InlineData(4, "IPG Mobile", 5, true, false, 1)]
            public void WhenTeamExists_UpdateSuccessfully(int id, string name, int deskCount, bool active, bool emailNotificationEnabled, int? parentTeamId)
            {
                var options = Helper.GetContextOptions();

                CreateTestTeams(options);

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

                    ValidateTeam(newTeam, id, name, deskCount, active, emailNotificationEnabled, parentTeamId);
                }
            }

            [Theory]
            [InlineData(100, "Bottonwood", 5, true, true, null)]
            [InlineData(102, "Spaarks", 5, true, true, 1)]
            public void WhenTeamNotExists_ThrowsException(int id, string name, int deskCount, bool active, bool emailNotificationEnabled, int? parentTeamId)
            {
                var options = Helper.GetContextOptions();

                CreateTestTeams(options);

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

        private static void CreateTestTeams(DbContextOptions<DataContext> options)
        {
            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();

                context.Teams.Add(CreateTeam("IPG", 4, true, true, null));
                context.Teams.Add(CreateTeam("TexaPro", 8, true, false, null));
                context.Teams.Add(CreateTeam("CIC", 2, false, true, null));
                context.Teams.Add(CreateTeam("Bonafied", 9, true, true, null));
                context.Teams.Add(CreateTeam("RedEngine", 3, true, true, 4));

                context.SaveChanges();
            }
        }

        private static Team CreateTeam(string name, int deskCount, bool active, bool emailNotificationEnabled, int? parentTeamId)
        {
            return new Team
            {
                Name = name,
                Active = active,
                DeskCount = deskCount,
                EmailNotificationEnabled = emailNotificationEnabled,
                ParentTeamId = parentTeamId
            };
        }

        #endregion

        #region Validating Data

        private static void ValidateTeam(TeamDto team, int? id, string name, int deskCount, bool active, bool emailNotificationEnabled, int? parentTeamId)
        {
            Assert.NotNull(team);

            if (id != null)
                Assert.Equal(id, team.Id);

            Assert.Equal(name, team.Name);
            Assert.Equal(deskCount, team.DeskCount);
            Assert.Equal(active, team.Active);
            Assert.Equal(emailNotificationEnabled, team.EmailNotificationEnabled);
            Assert.Equal(parentTeamId, team.ParentTeamId);
        }

        #endregion
    }
}
