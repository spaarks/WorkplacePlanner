using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkplacePlanner.Data.Entities
{
    public class Person : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string  LastName { get; set; }

        [Required]
        [MaxLength(200)]
        public string Email { get; set; }

        public bool Active { get; set; }
    }
}
