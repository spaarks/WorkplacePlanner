import { Injectable } from '@angular/core';
import 'rxjs/add/operator/toPromise';

import { TeamMembership } from '../models/team-membership';
import { MembershipDeleteDto } from '../models/membership-delete-dto';
import { DataService } from '../../shared/services/data.service';
import { HelperService } from '../../shared/services/helper.service';
import { TeamMembersXs } from '../models/team-members-xs';

@Injectable()
export class MemberService {
    constructor(private dataService: DataService,
        private helperService: HelperService) {

    }

    public getCurrentTeamMembers(teamId: number): Promise<TeamMembership[]> {
        return this.dataService.get('memberships', '', [teamId.toString(), this.helperService.formatDate(new Date())])
            .toPromise()
            .then(res => res.json() as TeamMembership[]);
    }

    public addMembers(data: TeamMembersXs) : Promise<any> {
        return this.dataService.put("memberships", "add", data)
            .toPromise();
    }

    public removeMembers(data: MembershipDeleteDto): Promise<any> {
        console.log(data);
        return this.dataService.put("memberships", "remove", data)
            .toPromise();
    }
}
