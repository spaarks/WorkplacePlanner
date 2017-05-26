import { Directive, ElementRef, Input, Renderer , OnInit, OnChanges, SimpleChange} from '@angular/core'

@Directive({
    selector: '[ft-show-desk-usage]'
})

export class ShowDeskUsageDirective implements OnInit, OnChanges {

    @Input() currentDeskUsage: number;
    @Input() teamDeskTotal: number;

    constructor(private el: ElementRef, private renderer: Renderer) { }       
    
    ngOnInit():void {
        this.setBackgroud();
    }

    private setBackgroud(): void {
        this.el.nativeElement.innerHTML = this.currentDeskUsage.toString();
        this.renderer.setElementStyle(this.el.nativeElement, 'backgroundColor', this.getBackgroundColor())
    }

    private getBackgroundColor(): string {
        var diff = this.currentDeskUsage - this.teamDeskTotal;
        if(diff > 0) {
            return 'red';
        } else if (diff < 0) {
            return 'green';
        } else {
            return 'yellow';
        }
    }

     ngOnChanges(changes: { [propName: string]: SimpleChange }): void {
         this.setBackgroud();
    }
}