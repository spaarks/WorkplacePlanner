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

        public UserDto Get(int id)
        {
            var user = _dataContext.Users.Where(u => u.Id == id)
                        .Select(u => new UserDto
                        {
                            Id = u.Id,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Email = u.Email,
                            Active = u.Active
                        }).FirstOrDefault();

            if (user == null)
                throw new RecordNotFoundException("User", id);

            return user;
        }

        public ICollection<UserDto> GetAll()
        {
            var peopleList = _dataContext.Users
                        .Select(p => new UserDto
                        {
                            Id = p.Id,
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            Email = p.Email,
                            Active = p.Active
                        }).ToList();

            return peopleList;
        }

        public ICollection<UserDto> GetAllActive()
        {
            var peopleList = _dataContext.Users
                .Where(u => u.Active)
                       .Select(u => new UserDto
                       {
                           Id = u.Id,
                           FirstName = u.FirstName,
                           LastName = u.LastName,
                           Email = u.Email,
                           Active = u.Active
                       }).ToList();

            return peopleList;
        }

        public ICollection<UserLDto> GetAllWithCurrentTeam()
        {
            var peopleList = _dataContext.Users
                        .Where(p => p.Active)
                        .Select(p => new UserLDto
                        {
                            Id = p.Id,
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            Email = p.Email,
                            Active = p.Active,
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
    }
}
