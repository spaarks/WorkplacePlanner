import { Person } from './person'

export class Team {
    id: number;
    name: string;
    deskCount: number;
    emailNotificationEnabled : boolean;
    managers: Person[];
    parentTeamId: number;
    active: boolean;
}
