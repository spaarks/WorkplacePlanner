using System;
using System.Collections.Generic;
using System.Text;
using WorkPlacePlanner.Domain.Dtos.Team;

namespace WorkPlacePlanner.Domain.Dtos.User
{
    public class UserLDto : UserDto
    {
        public TeamXsDto Team { get; set; }
    }
}
