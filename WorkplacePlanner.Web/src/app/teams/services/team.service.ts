import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';

import { Team } from '../models/team';
import { TeamXs } from '../models/team-xs';
import { User } from '../../users/models/user'
import { DataService } from '../../shared/services/data.service';
import { TreeNode } from 'primeng/components/common/api';
import { TreeTableNode } from '../../shared/models/tree-table-node';

@Injectable()
export class TeamService {
    constructor(private dataService: DataService) { }

    public create(team: Team): Promise<number> {
        return this.dataService.create('teams', '', team)
            .toPromise()
            .then(res => res.json() as number);
    }

    public get(id: number): Promise<Team> {
        console.log(id);
        return this.dataService.getById('teams', '', id)
            .toPromise()
            .then(response => response.json() as Team);
    }

    public getAll(): Promise<Team[]> {
        return this.dataService.get('teams')
            .toPromise()
            .then((response) => response.json() as Team[]);
    }

    public getAllActiveTeams() : Promise<TeamXs[]> {
        return this.dataService.get('teams','activeTeams')
            .toPromise()
            .then((response) => response.json() as TeamXs[]);
    }

    public getAllActiveTeamsTree() : Promise<TreeTableNode[]> {
        return this.getAllActiveTeams()
                .then(data => this.createTeamTree(data, null));
    }

    public getManagersList(team: Team): string {
        if (team && team.managers) {
            return team.managers.map(function (element) {
                return element.firstName + ' ' + element.lastName;
            }).join(', ');
        } else {
            return '';
        }
    }

    public getSubTeams(parentId: number): Promise<Team[]> {
        return this.dataService.getById('teams', 'subteams', parentId)
            .toPromise()
            .then(response => response.json() as Team[]);
    }

    public getTeamsTree(parentTeamId?: number): Promise<TreeTableNode[]> {
        var promise = parentTeamId == null ? this.getAll() : this.getSubTeams(parentTeamId);
        return promise.then(data => this.createTeamTree(data, parentTeamId));
    }

    public update(team: Team): Promise<any> {
        return this.dataService.update('teams', '', team)
            .toPromise();
    }

    private createTeamTree(teams: any[], parentTeamId: number): TreeTableNode[] {
        var teamTree = this.appendSubTeams(teams, parentTeamId, 0);
        return teamTree;
    }

    private appendSubTeams(teams: Team[], parentTeamId: number, level: number): TreeTableNode[] {
        let subTeams: TreeTableNode[] = []

        var childTeams = teams.filter(team => team.parentTeamId == parentTeamId);

        if (childTeams.length > 0) {
            childTeams.forEach(team => {
                var node = new TreeTableNode();
                node.data = team;
                node.level = level;
                var supperChileTeams = this.appendSubTeams(teams, team.id, level + 1);
                node.hasChildren = supperChileTeams.length > 0;
                node.expanded = true;
                subTeams.push(node);
                subTeams = subTeams.concat(supperChileTeams);
            });
        }
        return subTeams;
    }
}