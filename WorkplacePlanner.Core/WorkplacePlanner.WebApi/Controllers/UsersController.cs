using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkplacePlanner.Services;
using WorkPlacePlanner.Domain.Dtos.User;
using WorkPlacePlanner.Domain.Services;
using Microsoft.AspNetCore.Authorization;

namespace WorkplacePlanner.WebApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        IUserService _userService;

        public UsersController(IUserService personService)
        {
            _userService = personService;
        }

        // GET: api/User/active
        [HttpGet("active")]
        public IEnumerable<UserDto> Get()
        {
            var list = _userService.GetAllActive();
            return list;
        }

        [HttpGet("WithTeam")]
        public IEnumerable<UserLDto> GetWthCurrentTeam()
        {
            var list = _userService.GetAllWithCurrentTeam();
            return list;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public UserDto Get(int id)
        {
            var user = _userService.Get(id);
            return user;
        }
    }
}
