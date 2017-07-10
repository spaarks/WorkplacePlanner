import { TeamXs } from '../../teams/models/team-xs';

export class UserL {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    active: boolean;
    team: TeamXs;
}