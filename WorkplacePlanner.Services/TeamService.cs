﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkplacePlanner.Data;
using WorkplacePlanner.Data.Entities;
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
                        ParentTeamId = t.ParentTeamId
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
                       ParentTeamId = t.ParentTeamId
                   }).ToList();

            return teamList;
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