import { Component, Input, EventEmitter, Output } from '@angular/core'

@Component({
    moduleId: module.id,
    selector: 'ft-page-header',
    templateUrl: 'page-header.component.html'
})

export class PageHeaderComponent {
    @Input() title: string;
    @Input() subTitle: string;
    @Input() buttonName: string;
    @Output() buttonClicked: EventEmitter<void>;

    constructor() {
        this.buttonClicked = new EventEmitter<void>();
    }
    
    buttonClick(event: any) {
        event.preventDefault();
        console.log(`Header button clicked`);
        this.buttonClicked.emit();
    }
}


