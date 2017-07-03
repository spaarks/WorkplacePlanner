using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkPlacePlanner.Domain.Services;
using WorkPlacePlanner.Domain.Dtos.Team;

namespace WorkplacePlanner.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Teams")]
    public class TeamsController : Controller
    {
        ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        // GET: api/Teams
        [HttpGet]
        public IEnumerable<TeamDto> Get()
        {
            var teams = _teamService.GetAll();
            return teams;
        }

        // GET: api/Teams/5
        [HttpGet("{id}")]
        public TeamDto Get(int id)
        {
            var team = _teamService.Get(id);
            return team;
        }

        [HttpGet("SubTeams/{parentId}")]
        public IEnumerable<TeamDto> GetSubTeams(int parentId)
        {
            var teams = _teamService.GetSubTeams(parentId);
            return teams;
        }
        
        // POST: api/Teams
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Teams/5
        [HttpPut]
        public void Put([FromBody]TeamDto data)
        {
            _teamService.Update(data);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}