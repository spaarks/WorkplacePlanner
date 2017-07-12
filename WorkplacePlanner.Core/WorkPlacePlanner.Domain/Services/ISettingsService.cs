using System;
using System.Collections.Generic;
using System.Text;
using WorkPlacePlanner.Domain.Dtos.Settings;

namespace WorkPlacePlanner.Domain.Services
{
    public interface ISettingsService
    {
        void Create(SettingDto data);

        void Delete(string key);

        string Get(string key);

        List<SettingDto> GetAll();

        void Update(SettingDto data);
    }
}
