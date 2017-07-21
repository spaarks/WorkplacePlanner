using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkplacePlanner.Data.Entities
{
    public class TeamMembership : BaseEntity
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Team")]
        public int TeamId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Team Team { get; set; }

        public ICollection<CalendarEntry> CalendarEntries { get; set; }
    }
}
