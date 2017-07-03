import { Injectable } from '@angular/core';
import 'rxjs/add/operator/toPromise';

import { DataService } from '../../shared/services/data.service';
import { CalendarRow } from '../models/calendar-row';
import { CalendarEntry } from '../models/calendar-entry';
import { UsageType } from '../models/usage-type';
import { CalendarLegend } from '../models/calendar-legend';
import { QueryStringParam } from '../../shared/models/query-string-param';
import { CalendarUpdateDto } from '../models/calendar-update-dto';

@Injectable()
export class CalendarService {

    constructor(private dataService: DataService) { }

    getCalendar(teamId: number, month: Date): Promise<CalendarRow[]> {
        return this.dataService.get("calendar", '', [teamId.toString(), this.formatDate(month)])
            .toPromise()
            .then(response => response.json() as CalendarRow[]);
    }

    getCalenderEntries(teamMembershipId: number, month: Date): Promise<CalendarEntry[]> {
        return this.dataService.get('calendar', 'entries', [teamMembershipId.toString(), this.formatDate(month)])
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
        return this.dataService.update('calendar', '', data)
            .toPromise();
    }

    private formatDate(date): string {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [year, month, day].join('-');
    }
}