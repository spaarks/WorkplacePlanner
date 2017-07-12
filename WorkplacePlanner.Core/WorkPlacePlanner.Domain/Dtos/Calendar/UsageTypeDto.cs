using System;
using System.Collections.Generic;
using System.Text;

namespace WorkPlacePlanner.Domain.Dtos.Calendar
{
    public class UsageTypeDto
    {
        public int Id { get; set; }

        public string Abbreviation { get; set; }

        public string ColorCode { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public bool Selectable { get; set; }

    }
}
