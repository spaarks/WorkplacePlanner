using System;
using System.Collections.Generic;
using System.Text;

namespace WorkPlacePlanner.Domain.Dtos.Team
{
    public class TeamXsDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentTeamId { get; set; }
    }
}
