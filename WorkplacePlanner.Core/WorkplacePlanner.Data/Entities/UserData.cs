using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkplacePlanner.Data.Entities
{
    public class UserData : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

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

        public virtual ApplicationUser User { get; set; }
    }
}
