using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkplacePlanner.Data.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string  LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        [MinLength(5)]
        public string Email { get; set; }

        public bool Active { get; set; }

        public ICollection<TeamMembership> TeamMemberships { get; set; }
    }
}
