import { Injectable } from '@angular/core';
import 'rxjs/add/operator/toPromise';

import { DataService } from '../../shared/services/data.service';
import { HelperService } from '../../shared/services/helper.service';
import { CalendarRow } from '../models/calendar-row';
import { CalendarEntry } from '../models/calendar-entry';
import { UsageType } from '../models/usage-type';
import { CalendarLegend } from '../models/calendar-legend';
import { QueryStringParam } from '../../shared/models/query-string-param';
import { CalendarUpdateDto } from '../models/calendar-update-dto';

@Injectable()
export class CalendarService {

    constructor(private dataService: DataService, private helperService: HelperService) { }

    getCalendar(teamId: number, month: Date): Promise<CalendarRow[]> {
        return this.dataService.get("calendar", '', [teamId.toString(), this.helperService.formatDate(month)])
            .toPromise()
            .then(response => response.json() as CalendarRow[]);
    }

    getCalenderEntries(teamMembershipId: number, month: Date): Promise<CalendarEntry[]> {
        return this.dataService.get('calendar', 'entries', [teamMembershipId.toString(), this.helperService.formatDate(month)])
            .toPromise()
            .then(response => response.json() as CalendarEntry[]);
    }

    getUsageTypes(): Promise<UsageType[]> {
        return this.dataService.get('calendar', 'usageTypes')
            .toPromise()
            .then(response => response.json() as UsageType[])
    }

    getUsageStates(): CalendarLegend[] {
        let usageStates: CalendarLegend[] = [
            { colorCode: 'red', description: 'Desk Over Use', code: '' },
            { colorCode: 'yellow', description: 'Desk Fully Use', code: '' },
            { colorCode: 'green', description: 'Desk Under Use', code: '' }
        ];
        return usageStates;
    }

    updateCalendar(data: CalendarUpdateDto): Promise<any> {
        return this.dataService.put('calendar', '', data)
            .toPromise();
    }
}