import { Injectable } from '@angular/core';
import 'rxjs/add/operator/toPromise';

import { Membership } from '../models/membership';
import { DataService } from '../../shared/services/data.service';

@Injectable()
export class MemberService {
    constructor(private dataService: DataService) {}

    getTeamMembers(id:number): Promise<Membership[]> {
       return this.dataService.get('teamMembers', '')
           .toPromise()
           .then(response => response.json().data as Membership[]);
   }

   getUnassignedToTeams(): Promise<Membership[]> {
           return this.dataService.get('unassignedToTeams', '')
               .toPromise()
               .then(response => response.json().data as Membership[]);
       }

}
