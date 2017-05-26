import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, Params, Router } from '@angular/router';

import { Team } from '../models/team';
import { TeamService } from '../services/team.service';

@Component({
    moduleId: module.id,
    selector: 'ft-edit-team',
    templateUrl: 'team-edit.component.html'
})

export class TeamEditComponent implements OnInit {
    team: Team;
    parentTeam: Team;
    
    model = 1;

    constructor(private route: ActivatedRoute, 
                private router: Router, 
                private teamService: TeamService,
                private location: Location) {}

    ngOnInit(): void {
        this.route.params.forEach((params: Params) =>
        {
            let id = +params['id'];
            let parentTeamId = +params['parentTeamId'];
            this.teamService.getTeam(id)
                        .then(team => this.team = (team != null ? team : new Team()))
                        .then(team => this.teamService.getTeam(parentTeamId > 0 ? parentTeamId : team.parentTeamId)
                        .then(team => this.parentTeam = team));           
        });
    }

    private addSubTeam(): void {
        let link = ['/teams/new', { parentTeamId: this.team.id }];
        this.router.navigate(link);
    }

    onSubmit(): void {
        if(this.team.id) {
            this.teamService.update(this.team).then(team => this.team = team);
        } else {
            this.team.parentTeamId =  this.parentTeam != null ? this.parentTeam.id : null;
            this.teamService.create(this.team).then(team => this.team = team);
            //this.teamService.create(this.team).then(team => this.redirectToNewTeam(team));
        }
    }

    private redirectToNewTeam(team: Team): void {
        //console.log(team.id + ' ID, Name :' + team.name);
        let link = ['/teams/edit', team.id];
        this.router.navigate(link);
    }

    back() {
        this.location.back();
    }
}

