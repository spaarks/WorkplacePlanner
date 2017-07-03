import { NgModule } from '@angular/core';
import { CommonModule }  from '@angular/common';
import { FormsModule }   from '@angular/forms';
//import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';

import { DatepickerModule, TooltipModule } from  'ngx-bootstrap/ng2-bootstrap'; // 'ng2-bootstrap/ng2-bootstrap';
import { DialogModule } from 'primeng/components/dialog/dialog';

//In-memory data service for initial phase of development and mockup
import { InMemoryWebApiModule } from 'angular-in-memory-web-api';
import { InMemoryDataService } from './services/in-memory-data.service';

import { PageHeaderComponent } from './page-header/page-header.component'; 
import { NoDataTemplateComponent } from './no-data-template/no-data-template.component';
import { DataService } from './services/data.service';
import { MonthPickerComponent } from './month-picker/month-picker.component';
import { YearPickerComponent } from './year-picker/year-picker.component';

@NgModule({
    imports: [
        //BrowserModule,
        CommonModule,
        DatepickerModule,
        DialogModule,       
        FormsModule,
        HttpModule,
        //InMemoryWebApiModule.forRoot(InMemoryDataService),
        TooltipModule.forRoot(),
        DatepickerModule.forRoot()
    ],    
    declarations: [
        MonthPickerComponent,
        NoDataTemplateComponent,
        PageHeaderComponent,
        YearPickerComponent
    ],
    providers: [
        DataService
    ],
    exports: [
        //BrowserModule,
        CommonModule,
        DatepickerModule,
        DialogModule,
        FormsModule,
        HttpModule,
        MonthPickerComponent,
        PageHeaderComponent,
        NoDataTemplateComponent,
        TooltipModule,
        YearPickerComponent    
  ]
})

export class SharedModule { }

