import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, Params, Router } from '@angular/router';

import { Team } from '../models/team';
import { TeamService } from '../services/team.service';
import { MessageService } from '../../core/services/message.service';

@Component({
    moduleId: module.id,
    selector: 'ft-edit-team',
    templateUrl: 'team-edit.component.html'
})

export class TeamEditComponent implements OnInit {
    team: Team;

    constructor(private route: ActivatedRoute,
        private router: Router,
        private teamService: TeamService,
        private location: Location,
        private messageService: MessageService) { }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            let id = +params['id'];
            let parentTeamId = +params['parentTeamId'];
            if (this.router.url.split(";")[0].endsWith('new')) {
                this.team = new Team();
                this.team.parentTeamId = parentTeamId;
                this.team.active = true;
            }
            else {
                this.teamService.get(id)
                    .then(team => this.team = team);
            }
        });
    }

    private addSubTeam(): void {
        let link = ['/teams/new', { parentTeamId: this.team.id }];
        this.router.navigate(link);
    }

    onSubmit(): void {
        if (this.team.id) {
            this.teamService.update(this.team)
                .then(() => this.messageService.showSuccess('Team updated.'));
        } else {
            this.teamService.create(this.team)
                .then(teamId => this.redirectToNewTeam(teamId))
                .then(() => this.messageService.showSuccess('Team created.'));;
        }
    }

    parentTeamChanged(parentTeamId: number) {
        this.team.parentTeamId = parentTeamId;
    }

    private redirectToNewTeam(teamId: number): void {
        let link = ['/teams/edit', teamId];
        this.router.navigate(link);
    }

    back() {
        this.location.back();
    }
}

