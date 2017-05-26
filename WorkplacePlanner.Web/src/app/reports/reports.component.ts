import { Component } from '@angular/core'

@Component({
    moduleId: module.id,
    selector: 'ft-reports',
    templateUrl: 'reports.component.html'
})

export class ReportsComponent {

      reports: any[] = [
          {
              description: 'Monthly Occupancy Status',
              link:'monthly-occupancy'
          },
          {
              description: 'Desk Utilisation',
              link:'desk-utilisation'
          },
          {
              description: 'Weekly Desk Usage',
              link:'weekly-desk-usage'
          }
      ]
}