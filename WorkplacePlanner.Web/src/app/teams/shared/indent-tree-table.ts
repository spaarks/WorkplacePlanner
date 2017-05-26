import { Directive, ElementRef, Input, Renderer , OnInit} from '@angular/core'

@Directive({
    selector: '[ft-indent-tree-table]'
})

export class IndentTreeTableDirective implements OnInit {

    @Input() level: number;

    constructor(private el: ElementRef, private renderer: Renderer) { }       

    private setIndentation(): void {        
        var paddingLeft = 6 + this.level * 15;
        this.renderer.setElementStyle(this.el.nativeElement, 'paddingLeft', paddingLeft + 'px');
        //this.renderer.setElementClass(this.el.nativeElement, 'calendar-today', true);        
    }

    ngOnInit():void {
        this.setIndentation();
    }
}