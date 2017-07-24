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

        private static ApplicationUser[] GetUsers()
        {
            var list = new ApplicationUser[] {
                CreateUserData(1, "Alex", "Smith", true, "alex@yopmail.com"),
                CreateUserData(2, "Glenn", "Maxwell", true, "glen@yopmail.com"),
                CreateUserData(3, "Adam", "Gilchrist", true, "adam@yopmail.com"),
                CreateUserData(4, "Steve", "Smith", true, "steve@yopmail.com"),
                CreateUserData(5, "Mike", "Pence", true, "mike@yopmail.com"),
                CreateUserData(6, "Yash", "Varma", true, "yash@yopmail.com"),
                CreateUserData(7, "Andrew", "Flintop", false, "andrew@yopmail.com"),
                CreateUserData(8, "Ben", "Stoke", true, "ben@yopmail.com"),
                CreateUserData(9, "Lionel", "Messi", true, "lionel@yopmail.com"),
                CreateUserData(10, "Leo", "Tolstoy", true, "leo@yopmail.com"),
                CreateUserData(100, "Sauron", "Manager", true, "Sauron@yopmail.com"),
                CreateUserData(101, "Prodo", "Manager", true, "Prodo@yopmail.com"),
                CreateUserData(102, "Bilbo", "Manager", false, "Bilbo@yopmail.com"),
                CreateUserData(103, "Piping", "Manager", true, "Piping@yopmail.com")
            };

            return list;
        }

        private static ApplicationUser CreateUserData(int id, string firstName, string lastName, bool active, string email)
        {
            return new ApplicationUser
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
