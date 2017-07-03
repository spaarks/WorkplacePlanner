import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';

import { TeamXs } from '../models/team-xs';

@Component({
    moduleId: module.id,
    selector: 'ft-team-picker',
    templateUrl: 'team-picker.component.html'
})
export class TeamPickerComponent {

    teams: TeamXs[];
    selectedTeam : TeamXs;
    @Input() selectedTeamId: number;
    @Output() teamChanged: EventEmitter<number>;

   constructor() {
       this.teamChanged = new EventEmitter<number>();
   }

   ngOnInit(): void {
        
   }
}