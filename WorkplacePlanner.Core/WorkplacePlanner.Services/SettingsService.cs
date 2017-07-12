using System;
using System.Collections.Generic;
using System.Linq;
using WorkplacePlanner.Data;
using WorkPlacePlanner.Domain.Dtos.Settings;
using WorkPlacePlanner.Domain.Services;

namespace WorkplacePlanner.Services
{
    public class SettingsService : ISettingsService
    {
        DataContext _dataContext;

        public SettingsService(DataContext context)
        {
            _dataContext = context;
        }

        public void Create(SettingDto data)
        {
            throw new NotImplementedException();
        }

        public void Delete(string key)
        {
            throw new NotImplementedException();
        }

        public string Get(string key)
        {
            var setting = _dataContext.Settings.Where(s => s.Name == key).FirstOrDefault();
            return setting.Value;
        }

        public List<SettingDto> GetAll()
        {
            var listSettings = _dataContext.Settings.Select(s => new SettingDto
            {
                Name = s.Name,
                Value = s.Value
            }).ToList();

            return listSettings;
        }

        public void Update(SettingDto data)
        {
            throw new NotImplementedException();
        }
    }
}