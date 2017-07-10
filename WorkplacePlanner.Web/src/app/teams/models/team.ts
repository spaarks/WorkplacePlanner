import { User } from '../../users/models/user'

export class Team {
    id: number;
    name: string;
    deskCount: number;
    emailNotificationEnabled : boolean;
    managers: User[];
    parentTeamId: number;
    active: boolean;
}
