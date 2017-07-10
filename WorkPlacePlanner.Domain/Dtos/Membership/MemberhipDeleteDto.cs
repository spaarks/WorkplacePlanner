using System;
using System.Collections.Generic;
using System.Text;

namespace WorkPlacePlanner.Domain.Dtos.Membership
{
    public class MembershipDeleteDto
    {
        public int[] MembershipIds { get; set; }

        public DateTime TerminationDate { get; set; }
    }
}
