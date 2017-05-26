import { Component, OnInit, Input } from '@angular/core';

import { Membership } from './models/membership';
import { MemberService } from './services/member.service';

@Component({
    moduleId: module.id,
    selector: 'ft-members',
    templateUrl: 'members.component.html',
    styleUrls: ['members.component.css']
})

export class MembersComponent implements OnInit {

    listUnassigned: Array<Membership> = [];
    listTeamMembers: Array<Membership> = [];
    listRecycled: Array<Membership> = [];
    teamId: number = -1;
    teamName: string = '';
    counter: number = 0;
    filename: string = '';
    email: string = '';

    constructor(private memberService: MemberService) {}

    ngOnInit(): void {
      this.teamId = 1;
      this.teamName = 'Team 1';
      this.populateData();
    }

//line breaks are added to allow setting a breakpoint inside 'then' callback
    protected populateData() {

      var membersPromise = this.memberService.getTeamMembers(this.teamId);
      membersPromise
        .then(memberships =>
          this.listTeamMembers = memberships);

      var unassignedPromise = this.memberService.getUnassignedToTeams();
      unassignedPromise
        .then(memberships =>
          this.listUnassigned = memberships);
    }

    addedToRecycle($event) {
      console.log($event);
    }

    addedToUnassigned($event) {
      console.log($event);
    }

    addedToTeam($event) {
      console.log($event);
    }

    removedFromTeam($event) {
      console.log($event);
    }

    onAddEmail(event, data) {
      var m = new Membership();
      m.personDetails = data.email;
      m.id = -++this.counter;
      this.listUnassigned.splice(0, 0, m);
    }
}
