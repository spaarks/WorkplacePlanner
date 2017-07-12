using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkplacePlanner.Data.Entities
{
    public class CalendarEntry : BaseEntity
    {
        public int Id { get; set; }

        [ForeignKey("TeamMembership")]
        public int TeamMembershipId { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("UsageType")]
        public int UsageTypeId { get; set; }

        [MaxLength(500)]
        public string Comments { get; set; }

        public virtual TeamMembership TeamMembership { get; set; }

        public virtual UsageType UsageType { get; set; }
    }
}
