using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkplacePlanner.Data;
using WorkplacePlanner.Data.Entities;
using WorkPlacePlanner.Domain.Dtos.Membership;
using WorkPlacePlanner.Domain.Dtos.User;

namespace WorkPlacePlanner.Domain.Services
{
    public class MembershipService : IMembershipService
    {
        DataContext _dataContext;

        public MembershipService(DataContext context)
        {
            _dataContext = context;
        }

        public void Create(TeamMembersXsDto members)
        {
            TerminateExistingMemberhips(members.UserIds, members.StartDate.AddDays(-1));

            foreach (var personId in members.UserIds)
            {
                var membership = new TeamMembership
                {
                    TeamId = members.TeamId,
                    UserId = personId,
                    StartDate = members.StartDate,
                    EndDate = members.EndDate
                };

                _dataContext.TeamMemberships.Add(membership);
            }

            _dataContext.SaveChanges();
        }

       
        public void Delete(MembershipDeleteDto data)
        {
            var memberships = _dataContext.TeamMemberships.Where(m => data.MembershipIds.Contains(m.Id)).ToList();

            foreach (var membership in memberships)
            {
                membership.EndDate = data.TerminationDate;
            }

            _dataContext.SaveChanges();
        }

        public ICollection<TeamMembershipDto> GetMembersByTeam(int teamId, DateTime date)
        {
            var list = _dataContext.TeamMemberships
                        .Where(m => m.TeamId == teamId
                                && m.StartDate <= date
                                && (m.EndDate == null || m.EndDate >= date)
                                && m.User.UserData.Active)
                        .Select(m => new TeamMembershipDto
                        {
                            Id = m.Id,
                            TeamId = teamId,
                            User = new UserDto
                            {
                                Id = m.User.Id,
                                FirstName = m.User.UserData.FirstName,
                                LastName = m.User.UserData.LastName,
                                Email = m.User.Email,
                                Active = m.User.UserData.Active
                            }
                        })
                        .OrderBy(m => m.User.FirstName)
                        .ThenBy(m => m.User.LastName)
                        .ToList();

            return list;
        }

        #region Private Methods

        private void TerminateExistingMemberhips(int[] personIds, DateTime endDate)
        {
            var memberships = _dataContext.TeamMemberships
                                .Where(m => personIds.Contains(m.UserId)
                                        && (m.EndDate == null || m.EndDate > endDate));

            foreach (var membership in memberships)
            {
                membership.EndDate = endDate;

                if (membership.StartDate > membership.EndDate)
                    _dataContext.TeamMemberships.Remove(membership);
            }

            _dataContext.SaveChanges();
        }

        #endregion
    }
}
