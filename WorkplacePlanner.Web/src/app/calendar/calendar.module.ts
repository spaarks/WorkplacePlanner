import { NgModule } from '@angular/core';
//import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { DialogModule, ConfirmDialog } from 'primeng/primeng';

import {  SharedModule } from '../shared/shared.module';


import { CalendarComponent } from './calendar.component';
import { CalendarService } from './services/calendar.service';
import { CalendarRoutingModule } from './calendar-routing.module';
import { ShowDeskUsageDirective } from './shared/show-desk-usage.directive';
import { CalendarUpdateComponent } from './calendar-update/calendar-update.component';

@NgModule({
    imports: [
        CalendarRoutingModule,
        SharedModule
    ],
    declarations: [ 
        CalendarComponent,        
        CalendarUpdateComponent,        
        ShowDeskUsageDirective
    ],
    providers: [
        CalendarService
    ]
})

export class CalendarModule {
    
}

