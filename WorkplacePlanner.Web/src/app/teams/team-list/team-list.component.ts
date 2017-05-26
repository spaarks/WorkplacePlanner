import { Component, OnInit, Input, OnChanges, SimpleChange } from '@angular/core';
import { Router } from '@angular/router';

import { TeamService } from '../services/team.service';
import { Team } from '../models/team';
import { TreeTableNode } from '../../shared/models/tree-table-node';

@Component({
    moduleId: module.id,
    selector: 'ft-team-list',
    templateUrl: 'team-list.component.html',
    styleUrls: ['team-list.component.css']
})

export class TeamListComponent implements OnInit, OnChanges {

    teams : TreeTableNode[];

    @Input() parentTeamId?: number;

    constructor(private router: Router, private teamService: TeamService) {}

    ngOnInit(): void {
        this.getTeams();
    }

    ngOnChanges(changes: { [propName: string]: SimpleChange }): void {
        this.getTeams();
    }

    private getTeams(): void {
        this.teamService.getTeamsTree(this.parentTeamId).then(teams => this.teams = teams);
    }

    constructManagerList(team: Team): string {
        return this.teamService.getManagersList(team);
    }

    isExpanded(parentId: number){        
        var parentNode = this.teams.find(n => n.data.id == parentId);
        if(parentNode){
            return parentNode.expanded && this.isExpanded(parentNode.data.parentTeamId);
        }        
        return true;
    }
}