﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkplacePlanner.Data.Entities
{
    public class Team : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [ForeignKey("ParentTeam")]
        public int? ParentTeamId { get; set; }

        public int DeskCount { get; set; }

        public bool Active { get; set; }

        public bool EmailNotificationEnabled { get; set; }

        public virtual Team ParentTeam { get; set; }

        public ICollection<TeamManager> Managers { get; set; }

        public ICollection<Team> SubTeams { get; set; }
    }
}
