using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkplacePlanner.Data.Entities
{
    public class UsageType : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        [Required]
        [MaxLength(3)]
        public string Abbreviation { get; set; }

        [Required]
        [MaxLength(10)]
        public string ColorCode { get; set; }

        public bool Selectable { get; set; }

        public bool Active { get; set; }
    }
}
