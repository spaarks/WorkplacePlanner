import { InMemoryDbService } from 'angular-in-memory-web-api'

export class InMemoryDataService implements InMemoryDbService {

    private getDeskUsageEntry(id: number, month: number, day: number, comment: string, usageTypeId: number): any {
        return {
            id: id,
            date: new Date(2016, month - 1, day),
            comment: comment,
            usageTypeId: usageTypeId,
            usageTypeCode: this.getUsageCode(usageTypeId)
        };
    }

    private getUsageCode(id: number): string {
        switch (id) {
            case 1: return "io";
            case 2: return "wfh";
            case 3: return "oo";
            case 4: return "nbd";
            default: return "io";
        }
    }

    createDb() {

        let person1 = { id: 1, name: 'Sanath Jayasuriya' };
        let person2 = { id: 2, name: 'Kumar Sangakkara' };
        let person3 = { id: 3, name: 'Mahela Jayawardene' };
        let person4 = { id: 4, name: 'Ricky Ponting' };
        let person5 = { id: 5, name: 'Sachin Tendulkar' };
        let person6 = { id: 6, name: 'Jack Kallis' };
        let person7 = { id: 7, name: 'Dale Steyn' };
        let person8 = { id: 8, name: 'AB De villias' };
        let person9 = { id: 9, name: 'Virat Kohli' };
        let person10 = { id: 10, name: 'Lasith Malinga' };

        let teams = [
            { id: 1, name: 'Team 1', desks: 3, active: true, emailNotification: true, managers: [person1, person2] },
            { id: 2, name: 'Team 2', desks: 15, active: true, emailNotification: false, managers: [person3, person4] },
            { id: 3, name: 'Team 3', desks: 20, active: true, emailNotification: true, managers: [person5, person6] },
            { id: 4, name: 'Team 4', desks: 20, active: true, emailNotification: true, managers: [person5, person6] },
            { id: 5, name: 'Team 5', desks: 25, active: true, emailNotification: false, parentTeamId: 1, managers: [person7, person8] },
            { id: 6, name: 'Team 6', desks: 25, active: true, emailNotification: false, parentTeamId: 5, managers: [person7, person8] },
            { id: 7, name: 'Team 7', desks: 25, active: true, emailNotification: false, parentTeamId: 6, managers: [person7, person8] },
            { id: 8, name: 'Team 11', desks: 20, active: true, emailNotification: true, parentTeamId: 3, managers: [person5, person6] }
        ];

        let usageTypes = [
            { id: 1, code: 'IO', description: 'In Office', selectable: true, colourCode: '#f5f5f5' },
            { id: 2, code: 'WFH', description: 'Work From Home', selectable: true, colourCode: '#4169e1' },
            { id: 3, code: 'OO', description: 'Out Of Office', selectable: true, colourCode: '#696969' },
            { id: 4, code: 'NBD', description: 'Non Business Day', selectable: false, colourCode: '#b8c0cb' },
        ];

        let calendarLegends = [
            { colorCode: '#f5f5f5', description: 'In Office', code: 'IO' },
            { colorCode: '#4169e1', description: 'Work From Home', code: 'WFH' },
            { colorCode: '#696969', description: 'Out Of Office', code: 'OO' },
            { colorCode: '#b8c0cb', description: 'Non Business Day', code: 'NBD' },
            { colorCode: 'red', description: 'Desk Over Use', code: '' },
            { colorCode: 'yellow', description: 'Desk Fully Use', code: '' },
            { colorCode: 'green', description: 'Desk Under Use', code: '' }
        ]

        let calendarEntries = [
            {
                person: person1,
                deskUsages: [
                    this.getDeskUsageEntry(1, 11, 8, 'Test Wijitha', 2),
                    this.getDeskUsageEntry(2, 11, 9, '', 3),
                    this.getDeskUsageEntry(3, 11, 10, '', 3),
                    this.getDeskUsageEntry(4, 11, 15, '', 3),
                    this.getDeskUsageEntry(5, 11, 16, '', 3),

                    this.getDeskUsageEntry(1, 12, 8, 'Presentation', 2),
                    this.getDeskUsageEntry(1, 12, 9, 'Test Wijitha', 2),
                    this.getDeskUsageEntry(1, 12, 12, 'Testing', 3),
                    this.getDeskUsageEntry(1, 12, 19, 'No Desks', 2),
                    this.getDeskUsageEntry(1, 12, 28, 'Training', 3),
                    this.getDeskUsageEntry(1, 12, 30, '', 3),
                    this.getDeskUsageEntry(1, 12, 6, 'Comming to office', 1),
                ]

            },
            {
                person: person2,
                deskUsages: [
                    this.getDeskUsageEntry(6, 11, 7, '', 2),
                    this.getDeskUsageEntry(7, 11, 9, '', 2),
                    this.getDeskUsageEntry(8, 11, 10, '', 3),
                    this.getDeskUsageEntry(9, 11, 15, '', 2),
                    this.getDeskUsageEntry(10, 11, 16, '', 3)
                ]

            },
            {
                person: person3,
                deskUsages: [
                    this.getDeskUsageEntry(11, 11, 3, '', 3),
                    this.getDeskUsageEntry(12, 11, 9, '', 3),
                    this.getDeskUsageEntry(13, 11, 10, '', 3),
                    this.getDeskUsageEntry(14, 11, 14, '', 3),
                    this.getDeskUsageEntry(15, 11, 17, '', 3),
                    this.getDeskUsageEntry(15, 11, 22, '', 3),
                    this.getDeskUsageEntry(15, 11, 25, '', 3, )
                ]

            },
            {
                person: person4,
                deskUsages: [
                    this.getDeskUsageEntry(16, 11, 2, '', 2),
                    this.getDeskUsageEntry(17, 11, 4, '', 3),
                    this.getDeskUsageEntry(18, 11, 10, '', 2),
                    this.getDeskUsageEntry(19, 11, 15, '', 2),
                    this.getDeskUsageEntry(20, 11, 18, '', 2),
                    this.getDeskUsageEntry(20, 11, 28, '', 2),
                    this.getDeskUsageEntry(20, 11, 29, '', 3)
                ]
            },
            {
                person: person5,
                deskUsages: [
                    this.getDeskUsageEntry(16, 11, 2, '', 2),
                    this.getDeskUsageEntry(17, 11, 4, '', 2),
                    this.getDeskUsageEntry(18, 11, 10, '', 3),
                    this.getDeskUsageEntry(19, 11, 15, '', 3),
                    this.getDeskUsageEntry(20, 11, 18, '', 2)
                ]
            },
            {
                person: person6,
                deskUsages: [
                    this.getDeskUsageEntry(16, 11, 2, '', 2),
                    this.getDeskUsageEntry(17, 11, 4, '', 2),
                    this.getDeskUsageEntry(18, 11, 10, '', 3),
                    this.getDeskUsageEntry(19, 11, 15, '', 3),
                    this.getDeskUsageEntry(20, 11, 18, '', 2)
                ]
            },
            {
                person: person7,
                deskUsages: [
                    this.getDeskUsageEntry(16, 11, 2, '', 2),
                    this.getDeskUsageEntry(17, 11, 4, '', 2),
                    this.getDeskUsageEntry(18, 11, 10, '', 3),
                    this.getDeskUsageEntry(19, 11, 15, '', 3),
                    this.getDeskUsageEntry(20, 11, 18, '', 2)
                ]
            },
            {
                person: person8,
                deskUsages: [
                    this.getDeskUsageEntry(16, 11, 2, '', 2),
                    this.getDeskUsageEntry(17, 11, 4, '', 2),
                    this.getDeskUsageEntry(18, 11, 9, '', 3),
                    this.getDeskUsageEntry(19, 11, 15, '', 3),
                    this.getDeskUsageEntry(20, 11, 28, '', 2)
                ]
            },
            {
                person: person9,
                deskUsages: [
                    this.getDeskUsageEntry(16, 11, 2, '', 2),
                    this.getDeskUsageEntry(17, 11, 4, '', 2),
                    this.getDeskUsageEntry(18, 11, 10, '', 1),
                    this.getDeskUsageEntry(19, 11, 15, '', 3),
                    this.getDeskUsageEntry(20, 11, 30, '', 2)
                ]
            }
        ];



        let teamMembers = [
            { id: 1, personDetails: 'sanath.jayasuriya@company.com', roleTypeId: 2 }
        ];

        let unassignedToTeams = [
            { id: 2, personDetails: 'virat.kohli@company.com', roleTypeId: 2 },
            { id: 3, personDetails: 'dale.steyn@company.com', roleTypeId: null },
            { id: 4, personDetails: 'lasith.malinga@company.com', roleTypeId: 3 },
            { id: 5, personDetails: 'kumar.sangakkara@company.com', roleTypeId: null },
            { id: 6, personDetails: 'mahela.jayawardene@company.com', roleTypeId: null },
            { id: 7, personDetails: 'ricky.ponting@company.com', roleTypeId: 4 }
        ];

        let monthlyDeskOccupancy = [
            { month: 'Jan', io: 90, oo: 6, wfh: 4 },
            { month: 'Feb', io: 88, oo: 5, wfh: 7 },
            { month: 'Mar', io: 89, oo: 5, wfh: 6 },
            { month: 'Apr', io: 83, oo: 7, wfh: 10 },
            { month: 'May', io: 81, oo: 4, wfh: 15 },
            { month: 'Jun', io: 82, oo: 6, wfh: 12 },
            { month: 'Jul', io: 75, oo: 5, wfh: 20 },
            { month: 'Aug', io: 78, oo: 7, wfh: 15 },
            { month: 'Sep', io: 72, oo: 6, wfh: 22 },
            { month: 'Oct', io: 73, oo: 5, wfh: 22 },
            { month: 'Nov', io: 77, oo: 6, wfh: 17 },
            { month: 'Dec', io: 70, oo: 6, wfh: 24 }
        ];

        let deskUtilisations = [
            { month: 'Jan', unUsedDesks: 30, deskUsedPercentage: 80, usedDesks: 120, desksAllocated: 150 },
            { month: 'Feb', unUsedDesks: 40, deskUsedPercentage: 73, usedDesks: 110, desksAllocated: 150 },
            { month: 'Mar', unUsedDesks: 35, deskUsedPercentage: 77, usedDesks: 115, desksAllocated: 150 },
            { month: 'Apr', unUsedDesks: 10, deskUsedPercentage: 93, usedDesks: 140, desksAllocated: 150 },
            { month: 'May', unUsedDesks: 26, deskUsedPercentage: 84, usedDesks: 134, desksAllocated: 160 },
            { month: 'Jun', unUsedDesks: 10, deskUsedPercentage: 94, usedDesks: 150, desksAllocated: 160 },
            { month: 'Jul', unUsedDesks: 0, deskUsedPercentage: 100, usedDesks: 160, desksAllocated: 160 },
            { month: 'Aug', unUsedDesks: 15, deskUsedPercentage: 91, usedDesks: 145, desksAllocated: 160 }
        ]

        return { teams, calendarEntries, teamMembers, unassignedToTeams, usageTypes, calendarLegends, monthlyDeskOccupancy, deskUtilisations }
    }
}
