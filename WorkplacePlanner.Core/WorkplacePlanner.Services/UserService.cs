using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkplacePlanner.Data;
using WorkplacePlanner.Data.Entities;
using WorkplacePlanner.Utills.CustomExceptions;
using WorkPlacePlanner.Domain.Dtos.User;
using WorkPlacePlanner.Domain.Dtos.Team;
using WorkPlacePlanner.Domain.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace WorkplacePlanner.Services
{
    public class UserService : IUserService
    {
        DataContext _dataContext;

        public UserService(DataContext context)
        {
            _dataContext = context;
        }

        public int CreateUserData(UserDto data)
        {
            if (_dataContext.UserData.Any(u => u.UserId == data.Id))
                throw new DuplicateKeyException("UserData", "UserId", data.Id.ToString());

            var userData = new UserData
            {
                UserId = data.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
                Active = data.Active
            };

            _dataContext.UserData.Add(userData);
            _dataContext.SaveChanges();

            return userData.Id;
        } 

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public UserDto Get(int id)
        {

            var x = _dataContext.Users.Where(u => u.Id == id).Select(u => u.UserData).FirstOrDefault();

            var user = _dataContext.Users.Where(u => u.Id == id)
                        .Select(u => new UserDto
                        {
                            Id = u.Id,
                            FirstName = u.UserData.FirstName,
                            LastName = u.UserData.LastName,
                            Email = u.Email,
                            Active = u.UserData.Active
                        }).FirstOrDefault();

            if (user == null)
                throw new RecordNotFoundException("Person", id);

            return user;
        }

        public ICollection<UserDto> GetAll()
        {
            var peopleList = _dataContext.Users
                        .Select(p => new UserDto
                        {
                            Id = p.Id,
                            FirstName = p.UserData.FirstName,
                            LastName = p.UserData.LastName,
                            Email = p.Email,
                            Active = p.UserData.Active
                        }).ToList();

            return peopleList;
        }

        public ICollection<UserDto> GetAllActive()
        {
            var peopleList = _dataContext.Users
                .Where(u => u.UserData.Active)
                       .Select(u => new UserDto
                       {
                           Id = u.Id,
                           FirstName = u.UserData.FirstName,
                           LastName = u.UserData.LastName,
                           Email = u.Email,
                           Active = u.UserData.Active
                       }).ToList();

            return peopleList;
        }

        public ICollection<UserLDto> GetAllWithCurrentTeam()
        {
            var peopleList = _dataContext.Users
                        .Where(p => p.UserData.Active)
                        .Select(p => new UserLDto
                        {
                            Id = p.Id,
                            FirstName = p.UserData.FirstName,
                            LastName = p.UserData.LastName,
                            Email = p.Email,
                            Active = p.UserData.Active,
                            Team = p.TeamMemberships
                            .Where(m =>
                                m.StartDate <= DateTime.Now
                                && (m.EndDate == null || m.EndDate >= DateTime.Now))
                            .Select(m => new TeamXsDto
                            {
                                Id = m.TeamId,
                                Name = m.Team.Name
                            }).FirstOrDefault()
                        })
                        .OrderBy(p => p.FirstName)
                        .ThenBy(p => p.LastName)
                        .ToList();

            return peopleList;
        }

        public void Update(UserDto data)
        {
            var user = _dataContext.Users.Find(data.Id);
            if (user == null)
                throw new RecordNotFoundException("User", data.Id);

            var userData = _dataContext.UserData.Where(ud => ud.UserId == data.Id).FirstOrDefault();
            if(userData == null)
                throw new RecordNotFoundException("UserData for user", data.Id);

            user.Email = data.Email;
            userData.FirstName = data.FirstName;
            userData.LastName = data.LastName;
            userData.Active = data.Active;
            userData.Email = data.Email;

            _dataContext.SaveChanges();
        }      
    }
}
