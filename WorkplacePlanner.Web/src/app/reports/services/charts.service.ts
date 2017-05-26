import { Injectable } from '@angular/core'
import 'rxjs/add/operator/toPromise';

import { DataService } from '../../shared/services/data.service';
import { TeamMonthlyDeskOccupancy } from '../models/team-monthly-desk-occupancy';
import { TeamDeskUtilisation } from '../models/team-desk-utilisation';

@Injectable()
export class ChartsService {

    constructor(private dataService: DataService) { }

    getMonthlyDeskOccupancy(teamId: number, month: Date) : Promise<TeamMonthlyDeskOccupancy[]> {
        return this.dataService.get('monthlyDeskOccupancy', '')
            .toPromise()
            .then(response => response.json().data as TeamMonthlyDeskOccupancy[]);
    } 

    getTeamDeskUtilisations(teamId: number, year: Date) : Promise<TeamDeskUtilisation[]> {
        return this.dataService.get('deskUtilisations', '')
            .toPromise()
            .then(response => response.json().data as TeamDeskUtilisation[]);
    }    
}