using System;
using System.Collections.Generic;
using System.Text;

namespace WorkPlacePlanner.Domain.Dtos.Calendar
{
    public class CalendarUpdateDto
    {
        public int UsageTypeId { get; set; }

        public int TeamMembershipId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Comment { get; set; }
    }
}
