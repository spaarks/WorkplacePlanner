import { NgModule } from '@angular/core';
import { ChartModule } from 'primeng/primeng';

import { SharedModule } from '../shared/shared.module';

import { WeeklyDeskUsageChartComponent } from './weekly-desk-usage/weekly-desk-usage-chart.component';
import { MonthlyOccupancyChartComponent } from './monthly-occupancy/monthly-occupancy-chart.component';
import { DeskUtilisationChartComponent } from './desk-utilisation/desk-utilisation-chart.component';
import { ReportsComponent } from './reports.component';
import { ReportsRouteModule } from './reports-routing.module';
import { ChartsService } from './services/charts.service';

@NgModule({
    imports: [
        SharedModule,
        ReportsRouteModule,
        ChartModule
    ],
    declarations: [
        DeskUtilisationChartComponent,
        WeeklyDeskUsageChartComponent,
        MonthlyOccupancyChartComponent,
        ReportsComponent
    ],
    providers : [
        ChartsService
    ],
    exports: [
       
    ]
})
export class ReportsModule {}