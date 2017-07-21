import { Injectable } from '@angular/core';

import { UserL } from '../../users/models/user-l';

@Injectable()
export class SessionService {

    public user: UserL;

}