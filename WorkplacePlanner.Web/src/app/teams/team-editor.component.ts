import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';

import { TeamService } from './services/team.service';
import { Team } from './models/team';
import { TreeTableNode } from '../shared/models/tree-table-node';

@Component({
    moduleId: module.id,
    selector: 'ft-teams-list',
    templateUrl: 'team-editor.component.html',
    styleUrls: ['team-editor.component.css']
})

export class TeamEditorComponent implements OnInit {

    constructor(private router: Router, private teamService: TeamService) {}

    ngOnInit(): void {
        
    }

    private addTeam(): void {
        let link = ['/teams/new'];
        this.router.navigate(link);
    }
}