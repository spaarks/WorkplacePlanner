import { Component, EventEmitter, Output } from '@angular/core';

@Component({
    moduleId: module.id,
    selector: 'ft-month-picker',
    templateUrl: 'month-picker.component.html'
})

export class MonthPickerComponent {
    opened: boolean = false;
    date: Date = new Date();

    @Output() monthChanged: EventEmitter<Date>;

    constructor(){
        this.monthChanged = new EventEmitter<Date>();
    }

    public open(): void {
        this.opened = !this.opened;
    }

    public raiseMonthChange(): void {
        this.monthChanged.emit(this.date);
    }

    public monthTraverse(increment: number): void {
        this.date = new Date(this.date.getFullYear(), this.date.getMonth() + increment);
        this.raiseMonthChange();
    }   

    hidePopup(event) {
        this.date = event;
        this.opened = false;
        this.raiseMonthChange();        
    }
}