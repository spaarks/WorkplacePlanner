using System;
using System.Collections.Generic;
using System.Text;
using WorkPlacePlanner.Domain.Dtos.User;

namespace WorkPlacePlanner.Domain.Dtos.Team
{
    public class TeamDto
    {
        public int Id { get; set; }

        public int? ParentTeamId { get; set; }

        public string Name { get; set; }

        public int DeskCount { get; set; }

        public bool Active { get; set; }

        public bool EmailNotificationEnabled { get; set; }

        public UserDto[] Managers { get; set; }
    }
}
