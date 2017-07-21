using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkPlacePlanner.Domain.Dtos.User;

namespace WorkPlacePlanner.Domain.Services
{
    public interface IUserService
    {
        int CreateUserData(UserDto data);

        void Delete(int id);

        UserDto Get(int id);

        ICollection<UserDto> GetAll();

        ICollection<UserDto> GetAllActive();

        ICollection<UserLDto> GetAllWithCurrentTeam();

        void Update(UserDto data);
    }
}
