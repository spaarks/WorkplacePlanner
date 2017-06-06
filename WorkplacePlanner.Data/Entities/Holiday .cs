using System;
using System.Collections.Generic;
using System.Text;

namespace WorkplacePlanner.Data.Entities
{
    public class Holiday : BaseEntity
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Reason { get; set; }
    }
}
