using System;
using System.Collections.Generic;
using System.Text;

namespace WorkplacePlanner.Data.Entities
{
    public class Setting : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string  Value { get; set; }
    }
}
