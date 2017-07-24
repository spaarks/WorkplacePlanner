using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WorkPlacePlanner.Domain.Dtos.Account;
using WorkPlacePlanner.Domain.Dtos.User;

namespace WorkPlacePlanner.Domain.Services
{
    public interface IAccountService
    {
        Task<UserLDto> GetLoggedInUser(ClaimsPrincipal userClaims);

        Task<AuthToken> CreateAuthTokan(LoginDto loginData);

        Task<int> Register(UserDto data);
    }
}
