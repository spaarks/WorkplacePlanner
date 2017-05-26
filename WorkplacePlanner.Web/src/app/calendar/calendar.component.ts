import { Component, OnInit } from '@angular/core';
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

import { CalendarService } from './services/calendar.service';
import { CalendarEntry } from './models/calendar-entry';
import { DeskUsageEntry } from './models/desk-usage-entry';
import { TeamService } from '../teams/services/team.service';
import { Team } from '../teams/models/team';
import { Person } from '../teams/models/person';
import { CommonDataService } from '../core/services/common-data.service';
import { CalendarLegend } from '../core/models/calendar-legend';

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
    updateModal: NgbModalRef;

    calendarLegends: CalendarLegend[];

    today: Date = new Date();


    constructor(private calenderService: CalendarService, private teamService: TeamService, private modalService: NgbModal, private commonDataService : CommonDataService) { }

    ngOnInit(): void {
        this.teamService.getTeam(1) //TODO   
            .then(team => this.team = team);

        this.commonDataService.getCalendarLegends()
            .then(legends => this.calendarLegends = legends)    

        this.loadCalendar();
    }

    loadCalendar() {
        this.calenderService.getCalenderEntries(this.selectedMonth.getFullYear(), this.selectedMonth.getMonth())
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

    editUsageType(content, person: Person, deskusageEntry: DeskUsageEntry) {

        if(deskusageEntry.usageTypeCode == "nbd")
            return;

        this.editPerson = person;
        this.editDeskUsageEntry = deskusageEntry;

        this.updateModal = this.modalService.open(content, { size: 'sm' });

        // .result.then((result) => {
        //     console.log('modal closed');
        // }, (reason) => {
        //     console.log('cross clicked');
        // });
    }

    monthChanged(date: Date) {
        this.selectedMonth = date;
        this.loadCalendar();
    }

    private calendarUpdated(data: any): void {
        this.updateCalenderForDemo(data);
        this.updateModal.close();
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
                    usageEntry.usageTypeCode = this.commonDataService.getUsageType(data.usageTypeId).code.toLowerCase();
                    usageEntry.comment = data.comment;
                }
            }
            sDate.setDate(sDate.getDate() + 1);
        }
    }

    isToday(date: Date) : boolean {
        var isToday = this.today.setHours(0,0,0,0) == (new Date(date)).setHours(0,0,0,0);
        return isToday;       
    }

    getUsageColorCode(usageTypeId: number) : string {
        return this.commonDataService.getUsageType(usageTypeId).colourCode;
    }
}