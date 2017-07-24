using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkplacePlanner.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using WorkPlacePlanner.Domain.Dtos.User;
using WorkplacePlanner.Utills.CustomExceptions;
using WorkPlacePlanner.Domain.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WorkPlacePlanner.Domain.Dtos.Account;

namespace WorkplacePlanner.WebApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        // POST: /Account/Register
       
        [HttpPost("Register")]
        public async Task<int> Register([FromBody] UserDto data)
        {
            var userId = await _accountService.Register(data);
            return userId;
        }

        //POST: /Account/Login
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<AuthToken> Login([FromBody] LoginDto loginData)
        {
            var authToken = await _accountService.CreateAuthTokan(loginData);
            return authToken;
        }

        //GET: /Account/LoggedInUser
        [HttpGet("LoggedInUser")]
        public async Task<UserLDto> GetLoggedInUser()
        {
            var user = await _accountService.GetLoggedInUser(User);
            return user;
        }
    }
}
