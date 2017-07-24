using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WorkplacePlanner.Data.Entities;
using WorkplacePlanner.Utills.ConfigSettings;
using WorkplacePlanner.Utills.CustomExceptions;
using WorkPlacePlanner.Domain.Dtos.Account;
using WorkPlacePlanner.Domain.Dtos.User;
using WorkPlacePlanner.Domain.Services;

namespace WorkplacePlanner.Services
{
    public class AccountService : IAccountService
    {
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        AppSettings _appSettings;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }

        public async Task<AuthToken> CreateAuthTokan(LoginDto loginData)
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

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.AuthTokenKey));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: _appSettings.AuthTokenIssuerUrl,
                        audience: _appSettings.AuthTokenIssuerUrl,
                        claims: claims,
                        expires: DateTime.UtcNow.AddYears(1),
                        signingCredentials: creds);

                    var authToken = new AuthToken
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo
                    };

                    return authToken;
                }
            }

            throw new AuthenticationException();
        }

        public async Task<UserLDto> GetLoggedInUser(ClaimsPrincipal userClaims)
        {
            var user = await _userManager.GetUserAsync(userClaims);

            var userDto = new UserLDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Active = true,
                Roles = new string[] { "Admin", "Manager" }
            };

            return userDto;
        }

        public async Task<int> Register(UserDto data)
        {
            var user = new ApplicationUser
            {
                UserName = data.Email,
                Email = data.Email,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Active = data.Active,
            };

            var result = await _userManager.CreateAsync(user, "Pwd123"); //TODO: Modify this logic based on the project required.

            if (result.Succeeded)
            {
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
    }
}
