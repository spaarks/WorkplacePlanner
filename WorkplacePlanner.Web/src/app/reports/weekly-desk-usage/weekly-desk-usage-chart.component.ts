import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

import { ChartsService } from '../services/charts.service'; 



@Component({
    moduleId: module.id,
    selector: 'ft-weekly-desk-usage-chart',
    templateUrl: 'weekly-desk-usage-chart.component.html'
})

export class WeeklyDeskUsageChartComponent implements OnInit {

    data: any;

    options: any;

    constructor(private chartsService: ChartsService, private location: Location) { }

    ngOnInit(): void {
        this.data = {
                labels: ['Mon','Tue','Wed', 'Thu', 'Fri'],
                datasets: [
                        {
                            label: 'Average Desk Usage',
                            data: [10, 7, 15, 9, 4],
                            //backgroundColor: '#42A5F5',
                            borderColor: '#1E88E5',
                            type: 'line',
                            fill: false 
                        },
                         {
                            label: 'Max. Allowed',
                            data: [7, 7, 7 ,7 ,7],
                            //backgroundColor: '#42A5F5',
                            borderColor: 'red',  
                            type: 'line',
                            fill: false                      
                        }
                ] 
                };

        this.options = {
            title: {
                display: true,
                text: 'Average Desk Usage per Week Day',
                fontSize: 16
            },
            legend: {
                position: 'bottom'
            },
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        };      
    }

    back() {
        this.location.back();
    }
}