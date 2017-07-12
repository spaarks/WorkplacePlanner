using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkplacePlanner.Data.Entities
{
    public class TeamManager : BaseEntity
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int PersonId { get; set; }

        [ForeignKey("Team")]
        public int TeamId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public virtual User User { get; set; }

        public virtual Team Team { get; set; }
    }
}
