using System;
using System.Collections.Generic;
using System.Text;
using WorkPlacePlanner.Domain.Dtos.Person;

namespace WorkPlacePlanner.Domain.Dtos.Calendar
{
    public class CalendarRawDto
    {
        public PersonDto Person { get; set; }

        public List<CalendarEntryDto> CalendarEntries { get; set; }
        
        public bool HasPermissionToEdit { get; set; }

        public int MembershipId { get; set; }
    }
}
