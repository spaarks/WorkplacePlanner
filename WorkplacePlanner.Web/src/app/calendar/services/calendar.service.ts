import { Injectable } from '@angular/core';
import 'rxjs/add/operator/toPromise';

import { DataService } from '../../shared/services/data.service';
import { CalendarEntry } from '../models/calendar-entry';
import { DeskUsageEntry } from '../models/desk-usage-entry';
import { UsageType } from '../models/usage-type';
import { CalendarLegend } from '../models/calendar-legend';

@Injectable()
export class CalendarService {

    constructor(private dataService: DataService) {}

    getCalenderEntries(year: number, month: number): Promise<CalendarEntry[]> {
        return this.dataService.get('calendarEntries', '')
            .toPromise()
            .then(response => this.fillEmptyEntries(year, month, response.json().data as CalendarEntry[]));
            // .catch(this.handleError);
    }

    updateCalender(fromDate: Date, endDate: Date, usageTypeId : number, personTeamRoleId: number, personId: number) {
        console.log('updated called');
    }

    getCalendarLegends(): Promise<CalendarLegend[]> {
        return this.dataService.get('calendarLegends', '')
            .toPromise()
            .then(response => response.json().data as CalendarLegend[]);
    }

    getUsageTypes() : Promise<UsageType[]> {
        return this.dataService.get('usageTypes', '')
            .toPromise()
            .then(response => response.json().data as UsageType[])
    }

    getSelectableUsageTypes() : Promise<UsageType[]> {
        return this.getUsageTypes()
                    .then((u) => u.filter(u => u.selectable));
    }

    private fillEmptyEntries(year: number, month: number, calendarEntries: CalendarEntry[]) : CalendarEntry[] {
        var usageEntries : DeskUsageEntry[] = [];
        var days = this.getDaysInMonth(year, month);

        calendarEntries.forEach(entry => {
            usageEntries = [];
            days.forEach(day => { 
                   
                   var usage = entry.deskUsages.find(u => (new Date(u.date)).setHours(0,0,0,0) == day.setHours(0,0,0,0)) 
                   
                   if(usage) {
                        usageEntries.push(usage);
                   } 
                   else
                   {
                        usageEntries.push(this.createDefaultDeskUsageEntry(day));
                   }

            });
            
            entry.deskUsages = usageEntries;
        });

        return calendarEntries;
    }

    private createDefaultDeskUsageEntry(day: Date)
    {
        var defaultUsage = new DeskUsageEntry();
        defaultUsage.id = -1;
        defaultUsage.date = day;
        defaultUsage.comment = '';

        if(day.getDay() == 6 || day.getDay() == 0 )
        {
            defaultUsage.usageTypeId = 4; //Default usage type TODO
            defaultUsage.usageTypeCode = "nbd";
        }
       else
       {
            defaultUsage.usageTypeId = 1; //Default usage type TODO
            defaultUsage.usageTypeCode = "io";
       }
        
        return defaultUsage;
    }

    private getDaysInMonth(year: number, month: number) : Date[] {
        var date = new Date(year, month, 1);
        var days: Date[] = [];
       
        while (date.getMonth() === month){
            
            days.push(new Date(date));
            date.setDate(date.getDate() + 1);
        }
        
        return days;
    }
}