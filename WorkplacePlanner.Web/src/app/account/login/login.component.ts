import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { LoginData } from '../models/login-data';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginData: LoginData = new LoginData();

  errorMessage: string;

  returnUrl: string = '';

  constructor(private accountService: AccountService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params.forEach((params: Params) => {
      let retUrl = params['returnUrl'];
      this.returnUrl = retUrl;
    });
  }

  textChanged() {
    this.errorMessage = null;
  }

  login() {
    this.accountService.login(this.loginData)
      .then(() => {
        let link = [ ''/*this.returnUrl*/];
        this.router.navigate(link);
      })
      .catch(() => this.errorMessage = 'Username or password is invalied');
  }

}
