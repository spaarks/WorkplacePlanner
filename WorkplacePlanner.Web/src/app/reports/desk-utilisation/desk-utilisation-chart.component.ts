import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

import { TeamDeskUtilisation } from '../models/team-desk-utilisation';
import { ChartsService } from '../services/charts.service';
import { Team } from '../../teams/models/team';

@Component({
    moduleId: module.id,
    selector: 'ft-desk-utilisation-chart',
    templateUrl: 'desk-utilisation-chart.component.html'
})

export class DeskUtilisationChartComponent implements OnInit {
    teamDesksUtilisations: TeamDeskUtilisation[];
    team: Team;
    data: any;
    year: Date;

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
            }],
            yAxes: [{
                ticks: {
                    beginAtZero: true,
                }
            }]
        }
    };

    constructor(private chartsService: ChartsService, private location: Location) { }

    ngOnInit() {
        this.chartsService.getTeamDeskUtilisations(0, null)
            .then(data => this.teamDesksUtilisations = data)
            .then(data => this.populateChartData());
    }

    private populateChartData() {
        var labels = this.teamDesksUtilisations.map(o => o.month);

        var seriesArr = [];

        seriesArr.push(this.createSeries('No of Unused Desks', this.teamDesksUtilisations.map(o => o.unUsedDesks), '#1E88E5', 'bar'));
        seriesArr.push(this.createSeries('No of Desks Used', this.teamDesksUtilisations.map(o => o.usedDesks), 'orange', 'line'));
        seriesArr.push(this.createSeries('No of Desks Allocated', this.teamDesksUtilisations.map(o => o.desksAllocated), 'green', 'line'));

        this.data = {
            labels: labels,
            datasets: seriesArr
        }
    }

    private createSeries(label: string, data: number[], backgroundColor: string, type: string) {
        return {
            label: label,
            data: data,
            backgroundColor: backgroundColor,
            fill: false,
            type: type,
            borderColor: backgroundColor,
            lineTension: 0
        }
    }

    yearChanged(date: Date) {
        this.year = date;
    }

    back() {
        this.location.back();
    }
}