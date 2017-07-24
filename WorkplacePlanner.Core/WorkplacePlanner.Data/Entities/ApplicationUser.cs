using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkplacePlanner.Data.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public bool Active { get; set; }

        public ICollection<TeamMembership> TeamMemberships { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int LastUpdatedBy { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
