import { NgModule } from '@angular/core';
import { Routes, RouterModule} from '@angular/router';

import { ReportsComponent } from './reports.component';
import { MonthlyOccupancyChartComponent } from './monthly-occupancy/monthly-occupancy-chart.component';
import { DeskUtilisationChartComponent } from './desk-utilisation/desk-utilisation-chart.component';
import { WeeklyDeskUsageChartComponent } from './weekly-desk-usage/weekly-desk-usage-chart.component';


const routes: Routes = [
    {
        path: '',
        component: ReportsComponent,
        data: {
            title: "Reports"
        }
    },
    {
        path: 'monthly-occupancy',
        component: MonthlyOccupancyChartComponent,
        data: {
            title: "Monthly Occupancy"
        }
    },
    {
        path: 'desk-utilisation',
        component: DeskUtilisationChartComponent,
        data: {
            title: "Desk Utilisation"
        }
    },
    {
        path: 'weekly-desk-usage',
        component: WeeklyDeskUsageChartComponent,
        data: {
            title: "Weekly Desk Usage"
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ReportsRouteModule {}
