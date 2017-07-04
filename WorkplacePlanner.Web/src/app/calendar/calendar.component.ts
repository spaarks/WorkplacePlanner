import { Component, OnInit } from '@angular/core';

import { CalendarService } from './services/calendar.service';
import { CalendarEntry } from './models/calendar-entry';
import { CalendarRow } from './models/calendar-row';
import { TeamService } from '../teams/services/team.service';
import { Team } from '../teams/models/team';
import { Person } from '../teams/models/person';
import { CalendarLegend } from './models/calendar-legend';
import { UsageType } from './models/usage-type';
import { CalendarUpdateDto } from './models/calendar-update-dto';

@Component({
    moduleId: module.id,
    selector: 'ft-calendar',
    templateUrl: 'calendar.component.html',
    styleUrls: ['calendar.component.css']
})

export class CalendarComponent implements OnInit {
    calendarRows: CalendarRow[];
    selectedMonth: Date = new Date();
    team: Team;

    editPerson: Person = null;
    editCalendarEntry: CalendarEntry = null;

    usageTypes: UsageType[];
    editableUsageTypes: UsageType[];
    calendarLegends: CalendarLegend[];

    today: Date = new Date();
    usageTypeId_InOffice: number;

    isUpdateCalendar: boolean = false;
    editingEndDate: Date;

    defaultTeamId: number = 1;

    constructor(private calendarService: CalendarService, private teamService: TeamService) { }

    ngOnInit(): void {
        this.calendarService.getUsageTypes()
            .then((data) => (this.usageTypes = data))
            .then(() => {
                this.editableUsageTypes = this.usageTypes.filter(u => u.selectable == true)

                this.usageTypeId_InOffice = this.usageTypes.find(u => u.abbreviation.toUpperCase() == "IO").id;

                this.calendarLegends = [];

                this.usageTypes.forEach(u => {
                    var legend = new CalendarLegend();
                    legend.code = u.abbreviation;
                    legend.colorCode = u.colorCode;
                    legend.description = u.description;

                    this.calendarLegends.push(legend);
                });

                this.calendarLegends = this.calendarLegends.concat(this.calendarService.getUsageStates());
            });

            this.loadTeamAndCalendar(this.defaultTeamId);
    }

    loadTeamAndCalendar(teamId: number) {
         this.teamService.get(teamId)
            .then(team => this.team = team)
            .then(() => { this.loadCalendar(); });
    }

    loadCalendar() {
        this.calendarService.getCalendar(this.team.id, this.selectedMonth)
            .then(entries => (this.calendarRows = entries));
    }

    getDeskUsageCount(day: Date): number {
        var count = this.calendarRows
            .filter(el => el.calendarEntries
                .findIndex(u => (new Date(u.date)).setHours(0, 0, 0, 0) == (new Date(day)).setHours(0, 0, 0, 0) && u.usageTypeId == this.usageTypeId_InOffice) >= 0).length;
        return count;
    }

    getTeamManagers(): string {
        var managers = this.teamService.getManagersList(this.team);
        return '[Managers: ' + (managers != '' ? managers : 'None') + ']';
    }

    editCalendar(content, row: CalendarRow, editingEntry: CalendarEntry) {
        if (row.hasPermissionToEdit && editingEntry.editable) {
            this.editPerson = row.person;
            this.editCalendarEntry = Object.assign({}, editingEntry);
            this.editingEndDate = editingEntry.date;
            this.isUpdateCalendar = true;
        }
    }

    updateCalendar() {
        let data = new CalendarUpdateDto();
        data.teamMembershipId = this.editCalendarEntry.teamMembershipId;
        data.usageTypeId = this.editCalendarEntry.usageTypeId;
        data.comment = this.editCalendarEntry.comment;
        data.startDate = this.editCalendarEntry.date;
        data.endDate = this.editingEndDate;

        this.calendarService.updateCalendar(data)
            .then(() => {
                var row = this.calendarRows.find(r => r.membershipId == data.teamMembershipId);
                this.calendarService.getCalenderEntries(data.teamMembershipId, this.selectedMonth)
                    .then(data => row.calendarEntries = data);

            });

        this.isUpdateCalendar = false;
    }

    monthChanged(date: Date) {
        this.selectedMonth = date;
        this.loadCalendar();
    }

    teamChanged(teamId: number) {
        this.loadTeamAndCalendar(teamId);
    }

    isToday(date: Date): boolean {
        var isToday = this.today.setHours(0, 0, 0, 0) == (new Date(date)).setHours(0, 0, 0, 0);
        return isToday;
    }

    getUsageColorCode(usageTypeId: number): string {
        return this.usageTypes.find(u => u.id == usageTypeId).colorCode;
    }
}