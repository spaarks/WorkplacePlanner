import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';

import { SelectItem } from 'primeng/primeng'

import { TeamService } from '../services/team.service';
import { TeamXs } from '../models/team-xs';
import { TreeTableNode } from '../../shared/models/tree-table-node';

@Component({
    moduleId: module.id,
    selector: 'ft-team-picker',
    templateUrl: 'team-picker.component.html',
    styleUrls: ['team-picker.component.css']
})
export class TeamPickerComponent {
    teams: TreeTableNode[];
    selectItems: SelectItem[];
    selectedTeam: TreeTableNode;

    @Input() showEmptyRow: boolean = false;
    @Input() selectedTeamId: number;
    @Output() teamChanged: EventEmitter<number>;

    constructor(private teamService: TeamService) {
        this.teamChanged = new EventEmitter<number>();
    }

    ngOnInit(): void {
        this.loadTeams();
    }

    loadTeams(): void {
        this.teamService.getAllActiveTeamsTree()
            .then(data => this.teams = data)
            .then(() => {
                this.selectItems = this.teams.map((value, x, index) => { return { label: value.data.name, value: value } });
                if (this.showEmptyRow) {
                    this.selectItems.splice(0, 0, { label: '', value: this.getEmptyTreeNode() });
                }

                if (this.selectedTeamId > 0) {
                    this.selectedTeam = this.selectItems.find(i => i.value.data != null && i.value.data.id == this.selectedTeamId).value;
                }
            })
    }

    getEmptyTreeNode(): TreeTableNode {
        return { level: 0, hasChildren: false, data: null, expanded: true };
    }

    selectionChanged(x): void {
        if (this.selectedTeam.data != null) {
            this.teamChanged.emit(this.selectedTeam.data.id);
        } else {
            this.teamChanged.emit(null);
        }
    }
}