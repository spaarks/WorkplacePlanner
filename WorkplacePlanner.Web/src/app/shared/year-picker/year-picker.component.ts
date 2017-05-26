import { Component, EventEmitter, Output } from '@angular/core';

@Component({
    moduleId: module.id,
    selector: 'ft-year-picker',
    templateUrl: 'year-picker.component.html'
})

export class YearPickerComponent {
    opened: boolean = false;
    date: Date = new Date();

    @Output() yearChanged: EventEmitter<Date>;

    constructor(){
        this.yearChanged = new EventEmitter<Date>();
    }

    public open(): void {
        this.opened = !this.opened;
    }

    public raiseYearChange(): void {
        this.yearChanged.emit(this.date);
    }

    public yearTraverse(increment: number): void {
        this.date = new Date(this.date.getFullYear() + increment, 0);
        this.raiseYearChange();
    }   

    hidePopup(event) {
        this.date = event;
        this.opened = false;
        this.raiseYearChange();        
    }
}