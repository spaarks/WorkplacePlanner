using System;
using System.Collections.Generic;
using System.Text;

namespace WorkPlacePlanner.Domain.Dtos.Membership
{
    public class TeamMembersXsDto
    {
        public int TeamId { get; set; }

        public int[] UserIds { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
