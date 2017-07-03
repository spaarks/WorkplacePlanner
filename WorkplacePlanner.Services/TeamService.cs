﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkplacePlanner.Data;
using WorkplacePlanner.Data.Entities;
using WorkplacePlanner.Utills.CustomExceptions;
using WorkPlacePlanner.Domain.Dtos.Person;
using WorkPlacePlanner.Domain.Dtos.Team;
using WorkPlacePlanner.Domain.Services;

namespace WorkplacePlanner.Services
{
    public class TeamService : ITeamService
    {
        private DataContext _dataContext;

        public TeamService(DataContext context)
        {
            _dataContext = context;
        }

        public void Create(TeamDto data)
        {
            var team = new Team()
            {
                Name = data.Name,
                Active = data.Active,
                EmailNotificationEnabled = data.EmailNotificationEnabled,
                DeskCount = data.DeskCount,
                ParentTeamId = data.ParentTeamId
            };

            _dataContext.Teams.Add(team);
            _dataContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var team = _dataContext.Teams.Find(id);
            _dataContext.Teams.Remove(team);
            _dataContext.SaveChanges();
        }

        public TeamDto Get(int id)
        {
            var team = _dataContext.Teams.Where(t => t.Id == id)
                    .Select(t => new TeamDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Active = t.Active,
                        DeskCount = t.DeskCount,
                        EmailNotificationEnabled = t.EmailNotificationEnabled,
                        ParentTeamId = t.ParentTeamId,
                        Managers = t.Managers
                                    .Where(m => m.StartDate <= DateTime.Now
                                            && (m.EndDate == null || m.EndDate >= DateTime.Now))
                                    .Select(m => new PersonDto
                                    {
                                        Id = m.Person.Id,
                                        FirstName = m.Person.FirstName,
                                        LastName = m.Person.LastName,
                                        Email = m.Person.Email
                                    }).ToArray()                                         
                    }).FirstOrDefault();

            return team;
        }

        public ICollection<TeamDto> GetAll()
        {
            var teamList = _dataContext.Teams
                   .Select(t => new TeamDto
                   {
                       Id = t.Id,
                       Name = t.Name,
                       Active = t.Active,
                       DeskCount = t.DeskCount,
                       EmailNotificationEnabled = t.EmailNotificationEnabled,
                       ParentTeamId = t.ParentTeamId,
                       Managers = t.Managers
                                    .Where(m => m.StartDate <= DateTime.Now
                                            && (m.EndDate == null || m.EndDate >= DateTime.Now))
                                    .Select(m => new PersonDto
                                    {
                                        Id = m.Person.Id,
                                        FirstName = m.Person.FirstName,
                                        LastName = m.Person.LastName,
                                        Email = m.Person.Email
                                    }).ToArray()
                   }).ToList();

            return teamList;
        }

        public ICollection<TeamDto> GetSubTeams(int parentId)
        {
            var teamList = _dataContext.Teams
                 .Where(t => t.Id == parentId)
                 .SelectMany(t => t.SubTeams)
                 .Select(st => new TeamDto
                 {
                     Id = st.Id,
                     Name = st.Name,
                     Active = st.Active,
                     DeskCount = st.DeskCount,
                     EmailNotificationEnabled = st.EmailNotificationEnabled,
                     ParentTeamId = st.ParentTeamId,
                     Managers = st.Managers
                                    .Where(m => m.StartDate <= DateTime.Now
                                            && (m.EndDate == null || m.EndDate >= DateTime.Now))
                                    .Select(m => new PersonDto
                                    {
                                        Id = m.Person.Id,
                                        FirstName = m.Person.FirstName,
                                        LastName = m.Person.LastName,
                                        Email = m.Person.Email
                                    }).ToArray()
                 }).ToList();

            List<TeamDto> subTeams = new List<TeamDto>();
            foreach (var team in teamList)
            {
                subTeams.AddRange(GetSubTeams(team.Id));
            }

            teamList.AddRange(subTeams);

            return teamList;
        }       

        public int GetDefaultUsageType(int teamId, DateTime date)
        {
            var teamDefault = _dataContext.TeamDefaultUsageTypes
                                .Where(d => d.TeamId == teamId
                                        && d.StartDate <= date
                                        && (d.EndDate == null || d.EndDate >= date))
                                .FirstOrDefault();

            if (teamDefault != null)
                return teamDefault.UsageTypeId;

            var globalDefault = _dataContext.GlobalDefaultUsageTypes
                               .Where(d => d.StartDate <= date
                                       && (d.EndDate == null || d.EndDate >= date))
                               .FirstOrDefault();

            if (globalDefault == null)
                throw new TeamDefaultUsageTypeMissingException();

            return globalDefault.UsageTypeId;
        }

        public void Update(TeamDto data)
        {
            var team = _dataContext.Teams.Find(data.Id);

            team.Name = data.Name;
            team.DeskCount = data.DeskCount;
            team.Active = data.Active;
            team.EmailNotificationEnabled = data.EmailNotificationEnabled;
            team.ParentTeamId = data.ParentTeamId;

            _dataContext.SaveChanges();
        }
    }
}
