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

                    int id = service.Create(newPerson);

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

                    int id = service.Create(newPerson);

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

                context.SaveChanges();
            }
        }

        private static List<User> GetUsers()
        {
            var list = new List<User> {
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
                CreateUser(102, "Bilbo", "Manager", false, "Bilbo@yopmail.com"),
                CreateUser(103, "Piping", "Manager", true, "Piping@yopmail.com")
            };

            return list;
        }

        private static User CreateUser(int id, string firstName, string lastName, bool active, string email)
        {
            return new User
            {
                //Id = id,
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
