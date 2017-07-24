using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkPlacePlanner.Domain.Dtos.User;

namespace WorkPlacePlanner.Domain.Services
{
    public interface IUserService
    {
        UserDto Get(int id);

        ICollection<UserDto> GetAll();

        ICollection<UserDto> GetAllActive();

        ICollection<UserLDto> GetAllWithCurrentTeam();
        
    }
}
