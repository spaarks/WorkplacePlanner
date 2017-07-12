using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using WorkplacePlanner.Data;
using WorkplacePlanner.Data.Entities;
using WorkplacePlanner.Services;
using Xunit;

namespace WorkplacePlanner.Test
{
    public class SettingsServiceTests
    {
        public class Get
        {
            [Theory]
            [InlineData("WorkingWeekDays", "1,2,3,4,5")]
            [InlineData("CompanyName", "Unit Test Company")]
            public void WhenSettingsExists_ReturnsValue(string key, string value)
            {
                var options = Helper.GetContextOptions();
                SetupSettingsTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new SettingsService(context);
                    Assert.Equal(value,  service.Get(key));
                }
            }
        }

        public class GetAll
        {
            [Fact]
            public void WhenSettingsExists_ReturnsAll()
            {
                var options = Helper.GetContextOptions();
                SetupSettingsTestData(options);

                using (var context = new DataContext(options))
                {
                    var service = new SettingsService(context);
                    var settings = service.GetAll();

                    Assert.Equal(4, settings.Count);
                }
            }
        }

        #region Setup Test Data
        
        public static void SetupSettingsTestData(DbContextOptions<DataContext> options)
        {
            using (var context = new DataContext(options))
            {
                context.Database.EnsureDeleted();
                context.Settings.AddRange(GetSettings());
                context.SaveChanges();
            }
        }

        private static List<Setting> GetSettings()
        {
            var settingsList = new List<Setting> {
                CreateSetting("WorkingWeekDays", "1,2,3,4,5"),
                CreateSetting("CompanyName", "Unit Test Company"),
                CreateSetting("Setting1", "Setting1 Value"),
                CreateSetting("Setting2", "Setting2 Value")
            };

            return settingsList;
        }

        private static Setting CreateSetting(string name, string value)
        {
            return new Setting
            {
                Name = name,
                Value = value
            };
        }

        #endregion
    }
}
