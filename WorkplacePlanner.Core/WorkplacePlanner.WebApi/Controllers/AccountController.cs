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

namespace WorkplacePlanner.WebApi.Controllers
{    
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        IUserService _userService;
        IPasswordHasher<ApplicationUser> _passwordHasher;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserService userService, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordHasher = passwordHasher;
            _userService = userService;
        }

        //
        // POST: /Account/Register
        [Authorize]
        [HttpPost("Register")]
        public async Task<int> Register([FromBody] UserDto data)
        {
            var user = new ApplicationUser { UserName = data.Email, Email = data.Email };
            var result = await _userManager.CreateAsync(user, "Pwd123");

            if (result.Succeeded)
            {
                data.Id = user.Id;
                _userService.CreateUserData(data);

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                //await _signInManager.SignInAsync(user, isPersistent: false);
                //_logger.LogInformation(3, "User created a new account with password.");
                //return RedirectToLocal(returnUrl);
            }
            else
            {
                throw new WorkplacePlannerException("Error in registering user");
            }

            return user.Id;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<bool> Login([FromBody] LoginDto data)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(data.UserName, data.Password, data.RememberMe, lockoutOnFailure: false);

            return result.Succeeded;
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        [HttpPost("Login2")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateTokan([FromBody] LoginDto loginData)
        {
            var user = await _userManager.FindByNameAsync(loginData.UserName);
            if (user != null)
            {
               
                if (_signInManager.CheckPasswordSignInAsync(user, loginData.Password, false).Result.Succeeded)
                {
                    var userClaims = await _userManager.GetClaimsAsync(user);

                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.UserName)
                    }.Union(userClaims);

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeyGoesHere_GetThisFromAppSettings"));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: "localhost:4200",
                        audience: "localhost:4200",
                        claims: claims,
                        expires: DateTime.UtcNow.AddYears(1),
                        signingCredentials: creds);

                    var res = new {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    };

                    return Ok(res);
                }
            }

            return BadRequest("Failed to login");
        }

        [Authorize]
        [HttpGet("loggedInUser")]
        public async Task<UserLDto> GetLoggedInUser()
        {
            var user = await _userManager.GetUserAsync(this.User);

            var userDto = new UserLDto
            {
                Id = user.Id,
                FirstName = "FirstName",
                LastName = "Last Name",
                Email = user.Email,
                Active = true,
                Roles = new string[] { "Admin", "Manager" }
            };

            return userDto;
        }
    }
}
