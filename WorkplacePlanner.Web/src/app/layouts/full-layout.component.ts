import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';


import { DataService } from '../shared/services/data.service';
import { UserService } from '../users/services/user.service';
import { LoginData } from '../users/models/login-data';
import { SessionService } from '../core/services/session.service';
import { AccountService } from '../account/services/account.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './full-layout.component.html'
})
export class FullLayoutComponent implements OnInit {

  public disabled = false;
  public status: { isopen: boolean } = { isopen: false };

  public toggled(open: boolean): void {
    console.log('Dropdown is now: ', open);
  }

  public toggleDropdown($event: MouseEvent): void {
    $event.preventDefault();
    $event.stopPropagation();
    this.status.isopen = !this.status.isopen;
  }

  ngOnInit(): void {
    if(this.sessionService.user == null)
      this.accountService.setLoggedInUserInSession();
  }

  constructor(private accountService: AccountService,
    public sessionService: SessionService,
    private router : Router) {
  }

  public getUserDisplayName(){
    return this.sessionService.user.email;
  }

  public logout() {
    this.accountService.logout();
    let link =  ['account/login'];
    this.router.navigate(link);
  }
}
