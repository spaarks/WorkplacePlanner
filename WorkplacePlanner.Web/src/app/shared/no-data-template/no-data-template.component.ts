import { Component, Input } from '@angular/core';

@Component({
    moduleId : module.id,
    selector: 'ft-no-data-template',
    template: `<p> <i> [{{message}}] </i></p>`
})

export class NoDataTemplateComponent {

    @Input() message: string = "No Data Found";

}