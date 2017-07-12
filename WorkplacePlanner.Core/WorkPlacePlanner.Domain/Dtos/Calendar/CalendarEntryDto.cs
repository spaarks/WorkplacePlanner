using System;
using System.Collections.Generic;
using System.Text;

namespace WorkPlacePlanner.Domain.Dtos.Calendar
{
    public interface ICloneable<T>
    {
        T Clone();
    }

    public class CalendarEntryDto : ICloneable<CalendarEntryDto> 
    {
        public int Id { get; set; }

        public int TeamMembershipId { get; set; }

        public DateTime Date { get; set; }

        public string Comment { get; set; }

        public int UsageTypeId { get; set; }

        public bool Editable { get; set; }

        public CalendarEntryDto Clone()
        {
            return new CalendarEntryDto
            {
                Id = this.Id,
                TeamMembershipId = this.TeamMembershipId,
                Date = this.Date,
                Comment = this.Comment,
                UsageTypeId = this.UsageTypeId,
                Editable = this.Editable
            };
        }

        public override bool Equals(object obj)
        {
            CalendarEntryDto calendarEntryDto2 = (CalendarEntryDto)obj;

            if (Object.ReferenceEquals(this, calendarEntryDto2)) return true;
             
            return this != null && calendarEntryDto2 != null 
                && this.TeamMembershipId == calendarEntryDto2.TeamMembershipId
                && this.Date.Equals(calendarEntryDto2.Date);
        }

        public override int GetHashCode()
        {
            int hashDate = this.Date == null ? 0 : this.Date.GetHashCode();
            int hashmembershipId = this.TeamMembershipId.GetHashCode();
            return hashDate ^ hashmembershipId;
        }
    }
}
