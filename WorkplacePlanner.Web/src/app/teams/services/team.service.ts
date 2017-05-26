import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';

import { Team } from '../models/team';
import { Person } from '../models/person'
import { DataService } from '../../shared/services/data.service';
import { TreeNode } from 'primeng/components/common/api';
import { TreeTableNode } from '../../shared/models/tree-table-node';

@Injectable()
export class TeamService {
    constructor(private dataService: DataService) {}

    //TODO: This is just for testing purposes. Move this to appropriate module.
    getTeams(parentTeamId: number): Promise<Team[]> {
        return this.dataService.get('teams', '')
            .toPromise()
            .then(response => response.json().data as Team[])
            .then(teams => this.filterTeams(teams, parentTeamId)); //TODO           
            // .catch(this.handleError);
    }

    filterTeams(teams: Team[], parentTeamId: number) {
        var childTeams = this.getChildTeamIds(teams, parentTeamId);
        return teams.filter(t => parentTeamId == null || childTeams.findIndex(ct => ct == t.id) >= 0 );
    }

    getTeamsTree(parentTeamId?: number): Promise<TreeTableNode[]> {
        return this.getTeams(parentTeamId).then(data => this.createTeamTree(data, parentTeamId));
    }

    getTeam(id: number) { //Modify this after pluging the web api
        return this.getTeams(null)
            .then(teams => teams.find(t => t.id == id));
    }

    update(team: Team): Promise<Team> {
       return this.dataService.update('teams', '', team.id, team)
            .toPromise()
            .then(() => team);            
    }

    create(team: Team): Promise<Team> {
       return this.dataService.create('teams', '', team)
            .toPromise()
            .then(() => team);            
    }

    getChildTeamIds(teams: Team[], parentTeamId: number): number[] {
        var ids: number[] = [];

        var subTeams = teams.filter(t => parentTeamId == null || t.parentTeamId == parentTeamId);
        if( subTeams.length > 0 ) {
            subTeams.forEach(element => {
                ids.push(element.id);
                var childIds = this.getChildTeamIds(teams, element.id);
                ids = ids.concat(childIds);
            });                        
        }
        return ids;     
    }

    getManagersList(team: Team): string {
        if(team && team.managers){
            return team.managers.map(function(element){
                return element.name;
            }).join(', ');
        }else {
            return '';
        }
    }

    private createTeamTree(teams: Team[], parentTeamId: number) : TreeTableNode[]{
        var teamTree = this.appendSubTeams(teams, parentTeamId, 0);
        return teamTree;
    }

    private appendSubTeams(teams: Team[], parentTeamId: number, level: number): TreeTableNode[] {
        let subTeams: TreeTableNode[] = []

        var childTeams = teams.filter(team => team.parentTeamId == parentTeamId);

        if(childTeams.length > 0 ) {
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