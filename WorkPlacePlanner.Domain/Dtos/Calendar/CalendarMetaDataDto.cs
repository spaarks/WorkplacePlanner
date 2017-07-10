using System;
using System.Collections.Generic;
using System.Text;
using WorkPlacePlanner.Domain.Dtos.User;

namespace WorkPlacePlanner.Domain.Dtos.Calendar
{
    public class CalendarMetaDataDto
    {
        public List<UsageTypeDto> UsageTypes { get; set; }

        public List<UserDto> TeamManagers { get; set; }
    }
}
