using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkplacePlanner.Data.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public UserData UserData { get; set; }

        public ICollection<TeamMembership> TeamMemberships { get; set; }
    }
}
