import { Person } from './person'

export class Team {
    id: number;
    name: string;
    desks: number;
    emailNotification : boolean;
    managers: Person[];
    parentTeamId: number;
}
