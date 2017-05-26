import { Injectable } from '@angular/core';

import { DataService } from '../../shared/services/data.service';
import { UsageType } from '../models/usage-type';
import { CalendarLegend } from '../models/calendar-legend';

@Injectable()
export class CommonDataService {

    usageTypes: UsageType[];

    calendarLegends: CalendarLegend[];

    constructor(private dataService: DataService) {
        console.log('common data service');
        this.setUsageTypes();
    }

    getUsageTypes(): UsageType[] {
        if (this.usageTypes)
            this.setUsageTypes();
        return this.usageTypes;
    }

    getSelectableUsageTypes(): UsageType[] {
        return this.getUsageTypes().filter(u => u.selectable == true);
    }

    private setUsageTypes() {
        this.dataService.get('usageTypes', '')
            .toPromise()
            .then(response => response.json().data as UsageType[])
            .then((data) => {
                this.usageTypes = data;
            });
    }

    getCalendarLegends(): Promise<CalendarLegend[]> {
        return this.dataService.get('calendarLegends', '')
            .toPromise()
            .then(response => response.json().data as CalendarLegend[]);
    }

    private CalendarLegends(data: UsageType[]): CalendarLegend[] {
        return data.map(function (element) {
            return { colourCode: element.colourCode, description: element.description, code: element.code } as CalendarLegend
        });
    }

    getUsageType(id: number): UsageType {
        return this.usageTypes.find(u => u.id == id);
    }
}