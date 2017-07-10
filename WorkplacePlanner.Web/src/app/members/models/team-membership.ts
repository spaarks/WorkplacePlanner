import { User } from '../../users/models/user';

export class TeamMembership {
    id: number;
    teamId: number;
    user: User;    
}
