using System;
using System.Collections.Generic;
using System.Text;

namespace WorkplacePlanner.Data.Entities
{
    public class BaseEntity
    {
        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int LastUpdatedBy { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
