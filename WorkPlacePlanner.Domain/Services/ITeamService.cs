using System;
using System.Collections.Generic;
using System.Text;
using WorkPlacePlanner.Domain.Dtos.Team;

namespace WorkPlacePlanner.Domain.Services
{
    public interface ITeamService
    {
        void Create(TeamDto data);

        void Delete(int id);

        TeamDto Get(int id);

        ICollection<TeamDto> GetAll();

        int GetDefaultUsageType(int teamId, DateTime month);

        ICollection<TeamDto> GetSubTeams(int parentId);

        void Update(TeamDto data);
    }
}
