import { Injectable } from '@angular/core';

import { LoginData } from '../models/login-data';
import { AuthToken } from '../models/auth-token';
import { DataService } from '../../shared/services/data.service';
import { UserL } from '../../users/models/user-l';
import { SessionService } from '../../core/services/session.service';

@Injectable()
export class AccountService {

  constructor(private dataService: DataService, private sessionService: SessionService) { }

  public login(loginData: LoginData) {
    return this.dataService.create("account", "login2", loginData)
      .toPromise()
      .then((res) => res.json() as AuthToken)
      .then((token) => {
        localStorage.removeItem("authToken");
        sessionStorage.removeItem("authToken");

        if (loginData.rememberMe) {
          localStorage.setItem("authToken", token.token);
        } else {
          sessionStorage.setItem("authToken", token.token);
        }

        this.setLoggedInUserInSession();
      });
  }

  public logout() {
    localStorage.removeItem("authToken");
    sessionStorage.removeItem("authToken");
    this.sessionService.user = null;
  }

  public isUserLoggedIn() : boolean {
     var authToken = localStorage.getItem("authToken");
        if (authToken == null)
            authToken = sessionStorage.getItem('authToken');

      return authToken != null;      
  }

  public getLoggedInUser(): Promise<UserL> {
    return this.dataService.get('account', 'loggedInUser')
      .toPromise()
      .then(res => res.json() as UserL);
  }

  public setLoggedInUserInSession() {
    this.getLoggedInUser()
      .then(u => this.sessionService.user = u);
  }
}
