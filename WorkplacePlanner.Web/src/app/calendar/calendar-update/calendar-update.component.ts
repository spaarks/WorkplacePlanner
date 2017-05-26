import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';

import { CalendarService } from '../services/calendar.service';
import { Person } from '../../teams/models/person';
import { DeskUsageEntry } from '../models/desk-usage-entry';
import { UsageType } from '../../core/models/usage-type';
import { CommonDataService } from '../../core/services/common-data.service';

@Component({
    moduleId: module.id,
    selector: 'ft-calendar-update',
    templateUrl: 'calendar-update.component.html'
})

export class CalendarUpdateComponent implements OnInit {
    @Input() deskUsageEntry: DeskUsageEntry;
    @Input() person: Person;
    @Output() onUpdated: EventEmitter<any>;

    usageTypes: UsageType[];

    selectedUsageTypeId: number;
    selectedEndDate: Date;
    comment: string;

    constructor(private calendarService: CalendarService, private commonDataService: CommonDataService) { 
        this.onUpdated = new EventEmitter<boolean>();
    }

    ngOnInit() {
        this.usageTypes = this.commonDataService.getSelectableUsageTypes();
        this.selectedUsageTypeId = this.deskUsageEntry.usageTypeId;
        this.selectedEndDate = this.deskUsageEntry.date;
        this.comment = this.deskUsageEntry.comment;        
    }

    onSubmit() {
        //this.calendarService.updateCalender(this.deskUsageEntry.date, this.selectedEndDate, this.selectedUsageTypeId, null, this.person.id);
        var status = true;

        var obj = { personId: this.person.id, startDate: this.deskUsageEntry.date, endDate: this.selectedEndDate, usageTypeId: this.selectedUsageTypeId, comment: this.comment };
        
        event.preventDefault();
        
        this.onUpdated.emit(obj);
    }
}