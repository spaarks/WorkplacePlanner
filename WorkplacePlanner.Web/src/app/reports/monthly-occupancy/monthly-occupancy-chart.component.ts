import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

import { ChartsService } from '../services/charts.service';
import { Team } from '../../teams/models/team';
import { TeamMonthlyDeskOccupancy } from '../models/team-monthly-desk-occupancy';

@Component({
    moduleId: module.id,
    selector: 'ft-monthly-occupancy-chart-page',
    templateUrl: 'monthly-occupancy-chart.component.html'
})

export class MonthlyOccupancyChartComponent implements OnInit {
    team: Team;
    year: Date;
    teamMonthlyDeskOccupancies: TeamMonthlyDeskOccupancy[];

    data: any;
    
    options: any = {
        title: {
            display: false,
            text: 'Occupancy Status Monthly Trend',
            fontSize: 16
        },
        legend: {
            position: 'bottom'
        },
        scales: {
            xAxes: [{
                stacked: true
            }],
            yAxes: [{
                stacked: true,
                ticks: {
                    beginAtZero: true,
                }
            }]
        }
    };

    constructor(private chartsService: ChartsService, private location: Location) {

    }

    private populateChartData() {
        var labels = this.teamMonthlyDeskOccupancies.map(o => o.month);

        var seriesArr = [];

        seriesArr.push(this.createSeries('In Office', this.teamMonthlyDeskOccupancies.map(o => o.io), '#1E88E5'));
        seriesArr.push(this.createSeries('WFH', this.teamMonthlyDeskOccupancies.map(o => o.wfh), 'orange'));
        seriesArr.push(this.createSeries('Out of Office', this.teamMonthlyDeskOccupancies.map(o => o.oo), 'red'));

        this.data = {
            labels: labels,
            datasets: seriesArr
        }
    }

    private createSeries(label: string, data: number[], backgroundColor: string) {
        return {
            label: label,
            data: data,
            backgroundColor: backgroundColor,
            fill: false
        }
    }

    yearChanged(date: Date){
        this.year = date;
    }

    ngOnInit() {
        this.chartsService.getMonthlyDeskOccupancy(0, this.year)
            .then(data => this.teamMonthlyDeskOccupancies = data)
            .then(data => this.populateChartData());

    }

    back() {
        this.location.back();
    }
}