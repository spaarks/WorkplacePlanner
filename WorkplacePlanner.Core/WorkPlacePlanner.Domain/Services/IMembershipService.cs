using System;
using System.Collections.Generic;
using System.Text;
using WorkPlacePlanner.Domain.Dtos.Membership;

namespace WorkPlacePlanner.Domain.Services
{
    public interface IMembershipService
    {
        void Create(TeamMembersXsDto members);

        void Delete(MembershipDeleteDto data);

        ICollection<TeamMembershipDto> GetMembersByTeam(int teamId, DateTime date);

    }
}
