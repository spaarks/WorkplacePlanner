using System;
using System.Collections.Generic;
using System.Text;
using WorkPlacePlanner.Domain.Dtos.User;

namespace WorkPlacePlanner.Domain.Dtos.Membership
{
    public class TeamMembershipDto
    {
        public int Id { get; set; }

        public int TeamId { get; set; }

        public UserDto User { get; set; }

    }
}
