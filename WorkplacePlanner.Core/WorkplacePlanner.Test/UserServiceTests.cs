using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WorkplacePlanner.Data;
using WorkplacePlanner.Data.Entities;
using WorkplacePlanner.Services;
using WorkplacePlanner.Utills.CustomExceptions;
using WorkPlacePlanner.Domain.Dtos.User;
using Xunit;

namespace WorkplacePlanner.Test
{
    public class UserServiceTests
    {
        public class Create
        {
            [Theory]
            [InlineData("Albert", "Martin", "albert@yopmail.com", true)]
            [InlineData("Jimmy", "Anderson", "jimmy@yopmail.com", true)]
            [InlineData("Janaka", "Kumara", "janaka@yopmail.com", true)]
            public void WhenUsersNotExists_CreatedSuccessfully(string firstName, string lastName, string email, bool active)
            {
                var options = Helper.GetContextOptions();

                using (var context = new DataContext(options))
                {
                    var service = CreateUserService(context);

                    var newPerson = new UserDto
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        Active = active
                    };

                    int id = service.CreateUserData(newPerson);

                    var person = service.Get(id);

                    ValidateUser(person, id, firstName, lastName, email, active);
                }
            }

            [Theory]
            [InlineData("Albert", "Martin", "albert@yopmail.com", true)]
            [InlineData("Jimmy", "Anderson", "jimmy@yopmail.com", false)]
            [InlineData("Janaka", "Kumara", "janaka@yopmail.com", true)]
            public void WhenUsersExists_CreatedSuccessfully(string firstName, string lastName, string email, bool active)
            {
                var options = Helper.GetContextOptions();

                SetupUsersTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateUserService(context);

                    var newPerson = new UserDto
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        Active = active
                    };

                    int id = service.CreateUserData(newPerson);

                    var person = service.Get(id);

                    ValidateUser(person, id, firstName, lastName, email, active);
                }
            }
        }

        public class Get
        {
            [Theory]
            [InlineData(1, "Alex", "Smith", "alex@yopmail.com", true)]
            [InlineData(2, "Glenn", "Maxwell", "glen@yopmail.com", true)]
            [InlineData(7, "Andrew", "Flintop", "andrew@yopmail.com", false)]
            [InlineData(9, "Lionel", "Messi", "lionel@yopmail.com", true)]
            public void WhenValidIdGiven_ReturnsUser(int id, string firstName, string lastName, string email, bool active)
            {
                var options = Helper.GetContextOptions();

                SetupUsersTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateUserService(context);

                    var person = service.Get(id);

                    ValidateUser(person, id, firstName, lastName, email, active);
                }
            }

            [Theory]
            [InlineData(100)]
            [InlineData(1000)]
            [InlineData(1001)]
            public void WhenInValidIdGiven_ThrowsException(int id)
            {
                var options = Helper.GetContextOptions();

                SetupUsersTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateUserService(context);
                    Assert.Throws<RecordNotFoundException>(() => service.Get(id));                    
                }
            }
        }

        public class GetAll
        {
            [Fact]
            public void WhenUsersExists_ReturnAll()
            {
                var options = Helper.GetContextOptions();

                SetupUsersTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateUserService(context);
                    var people = service.GetAll();

                    Assert.Equal(14, people.Count);
                }
            }

            [Fact]
            public void WhenUsersNotExists_ReturnNothing()
            {
                var options = Helper.GetContextOptions();

                using (var context = new DataContext(options))
                {
                    var service = CreateUserService(context);
                    var people = service.GetAll();

                    Assert.Equal(0, people.Count);
                }
            }
        }

        public class GetAllActive
        {
            [Fact]
            public void WhenUsersExists_ReturnsAllActive()
            {
                var options = Helper.GetContextOptions();

                SetupUsersTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateUserService(context);
                    var people = service.GetAllActive();

                    Assert.Equal(12, people.Count);
                }
            }

            [Fact]
            public void WhenUsersNotExists_ReturnNothing()
            {
                var options = Helper.GetContextOptions();

                using (var context = new DataContext(options))
                {
                    var service = CreateUserService(context);
                    var people = service.GetAllActive();

                    Assert.Equal(0, people.Count);
                }
            }
        }

        public class Update
        {
            [Theory]
            [InlineData(1, "Alexi", "Smithi", "alexi@yopmail.com", true)]
            [InlineData(2, "Glenx", "Maxwell", "glenx@yopmail.com", false)]
            [InlineData(7, "Andrew", "Flintop", "andrew@yopmail.com", true)]
            [InlineData(9, "Lionel", "Messi", "lionel@yopmail.com", true)]
            public void WhenUserExists_UpdateSuccessfully(int id, string firstName, string lastName, string email, bool active)
            {
                var options = Helper.GetContextOptions();

                SetupUsersTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateUserService(context);

                    var personDto = new UserDto
                    {
                        Id = id,
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        Active = active
                    };


                    service.Update(personDto);

                    var updatedperson = service.Get(id);

                    ValidateUser(updatedperson, id, firstName, lastName, email, active);
                }
            }

            [Fact]
            public void WhenUsernNotExists_ThrowsException()
            {
                var options = Helper.GetContextOptions();

                SetupUsersTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = CreateUserService(context);

                    var personDto = new UserDto
                    {
                        Id = 1000,
                        FirstName = "mike",
                        LastName = "harry",
                        Email = "mike@yopmail.com",
                        Active = true
                    };

                    Assert.Throws<RecordNotFoundException>(() => service.Update(personDto));
                }
            }
        }

        private static UserService CreateUserService(DataContext context)
        {
            var service = new UserService(context);
            return service;
        }

        #region Test Data Setup

        private static void SetupUsersTestData(DbContextOptions<DataContext> options)
        {
            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();

                context.Users.AddRange(GetUsers());
                context.UserData.AddRange(GetUserData());

                context.SaveChanges();
            }
        }

        private static List<UserData> GetUserData()
        {
            var list = new List<UserData> {
                CreateUserData(1, 1, "Alex", "Smith", true, "alex@yopmail.com"),
                CreateUserData(2, 2, "Glenn", "Maxwell", true, "glen@yopmail.com"),
                CreateUserData(3, 3, "Adam", "Gilchrist", true, "adam@yopmail.com"),
                CreateUserData(4, 4, "Steve", "Smith", true, "steve@yopmail.com"),
                CreateUserData(5, 5, "Mike", "Pence", true, "mike@yopmail.com"),
                CreateUserData(6, 6, "Yash", "Varma", true, "yash@yopmail.com"),
                CreateUserData(7, 7, "Andrew", "Flintop", false, "andrew@yopmail.com"),
                CreateUserData(8, 8, "Ben", "Stoke", true, "ben@yopmail.com"),
                CreateUserData(9, 9, "Lionel", "Messi", true, "lionel@yopmail.com"),
                CreateUserData(10, 10, "Leo", "Tolstoy", true, "leo@yopmail.com"),
                CreateUserData(100, 11, "Sauron", "Manager", true, "Sauron@yopmail.com"),
                CreateUserData(101, 12, "Prodo", "Manager", true, "Prodo@yopmail.com"),
                CreateUserData(102, 13, "Bilbo", "Manager", false, "Bilbo@yopmail.com"),
                CreateUserData(103, 14, "Piping", "Manager", true, "Piping@yopmail.com")
            };

            return list;
        }

        private static ApplicationUser[] GetUsers()
        {
            var list = new ApplicationUser[] {
                CreatUser(1, "alex@yopmail.com"),
                CreatUser(2, "glen@yopmail.com"),
                CreatUser(3, "adam@yopmail.com"),
                CreatUser(4, "steve@yopmail.com"),
                CreatUser(5, "mike@yopmail.com"),
                CreatUser(6, "yash@yopmail.com"),
                CreatUser(7, "andrew@yopmail.com"),
                CreatUser(8, "ben@yopmail.com"),
                CreatUser(9, "lionel@yopmail.com"),
                CreatUser(10, "leo@yopmail.com"),
                CreatUser(11, "Sauron@yopmail.com"),
                CreatUser(12, "Prodo@yopmail.com"),
                CreatUser(13, "Bilbo@yopmail.com"),
                CreatUser(14, "Piping@yopmail.com")
            };

            return list;
        }

        private static ApplicationUser CreatUser(int id, string email)
        {
            return new ApplicationUser
            {
                Email = email
            };
        }

        private static UserData CreateUserData(int id, int userId, string firstName, string lastName, bool active, string email)
        {
            return new UserData
            {
                //Id = id,
                UserId = userId,
                FirstName = firstName,
                LastName = lastName,
                Active = active,
                Email = email,
            };
        }

        #endregion

        #region Validate

        private static void ValidateUser(UserDto person, int id, string firstName, string lastName, string email, bool active)
        {
            Assert.NotNull(person);
            Assert.Equal(id, person.Id);
            Assert.Equal(firstName, person.FirstName);
            Assert.Equal(lastName, person.LastName);
            Assert.Equal(active, person.Active);
            Assert.Equal(email, person.Email);
        }

        #endregion

    }

}
