import { Component, OnInit, Input } from '@angular/core';

import { TeamMembership } from './models/team-membership';
import { MembershipDeleteDto } from './models/membership-delete-dto';
import { TeamMembersXs } from './models/team-members-xs';
import { MemberService } from './services/member.service';
import { UserL } from '../users/models/user-l';
import { UserService } from '../users/services/user.service';
import { HelperService } from '../shared/services/helper.service';
import { MessageService } from '../core/services/message.service';

@Component({
  moduleId: module.id,
  selector: 'ft-members',
  templateUrl: 'members.component.html',
  styleUrls: ['members.component.css']
})

export class MembersComponent implements OnInit {

  allUsers: UserL[] = [];
  teamMembers: TeamMembership[] = [];

  selectedTeamId: number = 1;

  usersToAdd: string[] = [];
  membersToRemove: string[] = [];

  showAddUsersDialog: boolean = false;

  userFilter: string = "";

  constructor(private memberService: MemberService,
    private userService: UserService,
    private helperService: HelperService,
    private messageService: MessageService) { }

  ngOnInit(): void {
    this.loadAllUsers();
    this.loadTeamMembers();
  }

  loadAllUsers(): void {
    this.userService.GetAllWithTeam()
      .then(data => this.allUsers = data);
  }

  loadTeamMembers(): void {
    this.memberService.getCurrentTeamMembers(this.selectedTeamId)
      .then(data => this.teamMembers = data);
  }

  getAllUsers(): UserL[] {
    var filteredList = this.allUsers
      .filter(u => this.teamMembers.findIndex(m => m.user.id == u.id) < 0);

    return filteredList;
  }

  addMembers(): void {

    if (this.usersToAdd.length > 0) {
      var teamMembers = new TeamMembersXs();
      teamMembers.teamId = this.selectedTeamId;
      teamMembers.userIds = this.usersToAdd.map(id => +id);
      teamMembers.startDate = this.helperService.formatDate(new Date());

      this.memberService.addMembers(teamMembers)
        .then(() => {
          this.loadTeamMembers();
          this.usersToAdd = [];
        })
        .then(() => {
          this.messageService.showSuccess("New members added to the team");
        })
        .then(() => this.loadAllUsers()); //This can be futher enhanced by just refreshing only the affected members
    } else {
      this.messageService.showWarn("Select users to add");
    }
  }

  public removeMembers(): void {

    if(this.membersToRemove.length > 0) {
    var terminationDate = new Date();
    terminationDate.setDate(terminationDate.getDate() - 1);

    var data = new MembershipDeleteDto();
    data.membershipIds = this.membersToRemove.map(id => +id);
    data.terminationDate = this.helperService.formatDate(terminationDate);

    this.memberService.removeMembers(data)
      .then(() => {
        this.loadTeamMembers();
        this.loadAllUsers(); //This can be futher enhanced by just refreshing only the affected members
        this.membersToRemove = [];
      })
      .then(() => this.messageService.showSuccess("Members removed from the team"));
    }else {
      this.messageService.showWarn("Select members to removen");
    }
  }

  public teamChanged(teamId: number) {
    this.selectedTeamId = teamId;
    this.loadTeamMembers();
  }

  public addNewUsers(): void {
    this.showAddUsersDialog = true;
  }

  public userUpdated(id: number): void {
    console.log(id);
    this.userService.getUser(id)
      .then(user => this.allUsers.splice(0, 0, user));
  }
}
