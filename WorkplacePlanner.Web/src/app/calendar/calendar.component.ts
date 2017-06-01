import { Component, OnInit } from '@angular/core';

import { CalendarService } from './services/calendar.service';
import { CalendarEntry } from './models/calendar-entry';
import { DeskUsageEntry } from './models/desk-usage-entry';
import { TeamService } from '../teams/services/team.service';
import { Team } from '../teams/models/team';
import { Person } from '../teams/models/person';
import { CalendarLegend } from './models/calendar-legend';
import { UsageType } from './models/usage-type';

@Component({
    moduleId: module.id,
    selector: 'ft-calendar',
    templateUrl: 'calendar.component.html',
    styleUrls: ['calendar.component.css']
})

export class CalendarComponent implements OnInit {
    calendarEntries: CalendarEntry[];
    selectedMonth: Date = new Date();
    team: Team;

    editPerson: Person = null;
    editDeskUsageEntry: DeskUsageEntry = null;

    calendarLegends: CalendarLegend[];

    today: Date = new Date();

    usageTypes: UsageType[];

    constructor(private calendarService: CalendarService, private teamService: TeamService) { }

    ngOnInit(): void {
        this.teamService.getTeam(1) //TODO   
            .then(team => this.team = team);

        this.calendarService.getCalendarLegends()
            .then(legends => this.calendarLegends = legends);

        this.calendarService.getUsageTypes()
            .then((data) => this.usageTypes = data);

        this.loadCalendar();
    }

    loadCalendar() {
        this.calendarService.getCalenderEntries(this.selectedMonth.getFullYear(), this.selectedMonth.getMonth())
            .then(entries => this.calendarEntries = entries);
    }

    getDeskUsageCount(day: Date): number {
        var count = this.calendarEntries
            .filter(el => el.deskUsages
                .findIndex(u => (new Date(u.date)).setHours(0, 0, 0, 0) == (new Date(day)).setHours(0, 0, 0, 0) && u.usageTypeCode == "io") >= 0).length;
        //console.log(count);        
        return count;
    }

    getTeamManagers(): string {
        var managers = this.teamService.getManagersList(this.team);
        return '[Managers: ' + (managers != '' ? managers : 'None') + ']';
    }

    isUpdateCalendar: boolean = false;
    editingEndDate: Date;

    editUsageType(content, person: Person, deskusageEntry: DeskUsageEntry) {

        if (deskusageEntry.usageTypeCode == "nbd")
            return;

        this.editPerson = person;
        this.editDeskUsageEntry = Object.assign({}, deskusageEntry);
        this.editingEndDate = deskusageEntry.date;
        this.isUpdateCalendar = true;
    }

    updateCalendar() {
        console.log('Calendar Updated');
        //this.calendarService.updateCalender(this.deskUsageEntry.date, this.selectedEndDate, this.selectedUsageTypeId, null, this.person.id);
        this.isUpdateCalendar = false;

        var obj = {
            personId: this.editPerson.id,
            startDate: this.editDeskUsageEntry.date, 
            endDate: this.editingEndDate, 
            usageTypeId: this.editDeskUsageEntry.usageTypeId, 
            comment: this.editDeskUsageEntry.comment
        };

        this.updateCalenderForDemo(obj);
    }

    monthChanged(date: Date) {
        this.selectedMonth = date;
        this.loadCalendar();
    }

    private calendarUpdated(data: any): void {
        this.updateCalenderForDemo(data);
    }

    private updateCalenderForDemo(data: any) {
        var entry = this.calendarEntries.find(e => e.person.id == data.personId);
        var sDate = new Date(data.startDate);
        var eDate = new Date(data.endDate);

        while (sDate <= eDate) {
            var usageEntry = entry.deskUsages.find(u => (new Date(u.date)).setHours(0, 0, 0, 0) == sDate.setHours(0, 0, 0, 0));
            if (usageEntry) {
                if (usageEntry.usageTypeCode != "nbd") {
                    usageEntry.usageTypeId = data.usageTypeId;
                    usageEntry.usageTypeCode = this.usageTypes.filter(u => u.id == data.usageTypeId)[0].code;
                    usageEntry.comment = data.comment;
                }
            }
            sDate.setDate(sDate.getDate() + 1);
        }
    }

    isToday(date: Date): boolean {
        var isToday = this.today.setHours(0, 0, 0, 0) == (new Date(date)).setHours(0, 0, 0, 0);
        return isToday;
    }

    getUsageColorCode(usageTypeId: number): string {
        return this.usageTypes.filter(u => u.id == usageTypeId)[0].colourCode;
    }

    display: boolean = false;

    showDialog() {
        this.display = true;
    }
}