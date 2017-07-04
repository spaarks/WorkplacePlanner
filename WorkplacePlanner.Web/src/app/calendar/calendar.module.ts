import { NgModule } from '@angular/core';

import { FormsModule } from '@angular/forms';
import { DialogModule, ConfirmDialog } from 'primeng/primeng';

import {  SharedModule } from '../shared/shared.module';
import { TeamsModule } from '../teams/teams.module';

import { CalendarComponent } from './calendar.component';
import { CalendarService } from './services/calendar.service';
import { CalendarRoutingModule } from './calendar-routing.module';
import { ShowDeskUsageDirective } from './shared/show-desk-usage.directive';


@NgModule({
    imports: [
        CalendarRoutingModule,
        SharedModule,
        TeamsModule
    ],
    declarations: [ 
        CalendarComponent,        
        ShowDeskUsageDirective
    ],
    providers: [
        CalendarService
    ]
})

export class CalendarModule {
    
}

