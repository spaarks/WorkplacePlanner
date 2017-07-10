using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkPlacePlanner.Domain.Services;
using WorkPlacePlanner.Domain.Dtos.Membership;

namespace WorkplacePlanner.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Memberships")]
    public class MembershipsController : Controller
    {
        IMembershipService _membershipService;

        public MembershipsController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        // GET: api/Memberships/1
        [HttpGet("{teamId}/{date}")]
        public IEnumerable<TeamMembershipDto> Get(int teamId, DateTime date)
        {
            var list = _membershipService.GetMembersByTeam(teamId, date);
            return list;
        }
        
        // PUT: api/Memberships/Add
        [HttpPut("Add")]
        public void AddMemberships([FromBody] TeamMembersXsDto data)
        {
            _membershipService.Create(data);
        }
        
        // PUT: api/Memberships/Remove
        [HttpPut("Remove")]
        public void RemoveMemberships([FromBody] MembershipDeleteDto data)
        {
            _membershipService.Delete(data);
        }
    }
}
