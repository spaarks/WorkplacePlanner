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

namespace WorkplacePlanner.Services
{
    public class UserService : IUserService
    {
        DataContext _dataContext;

        public UserService(DataContext context)
        {
            _dataContext = context;
        }

        public int Create(UserDto data)
        {
            if (_dataContext.Users.Any(u => u.Email == data.Email))
                throw new DuplicateKeyException(data.Email);

            var person = new User
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
                Active = data.Active
            };

            _dataContext.Users.Add(person);
            _dataContext.SaveChanges();

            return person.Id;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public UserDto Get(int id)
        {
            var person = _dataContext.Users.Find(id);

            if (person == null)
                throw new RecordNotFoundException("Person", id);

            var userDto = new UserDto
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                Active = person.Active
            };

            return userDto;
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
                .Where(p => p.Active)
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

        public void Update(UserDto data)
        {
            var person = _dataContext.Users.Find(data.Id);

            if (person == null)
                throw new RecordNotFoundException("Person", data.Id);

            person.FirstName = data.FirstName;
            person.LastName = data.LastName;
            person.Email = data.Email;
            person.Active = data.Active;

            _dataContext.SaveChanges();
        }

      
    }
}
