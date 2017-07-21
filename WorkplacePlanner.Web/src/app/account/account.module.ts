import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { AccountRoutingModule } from './account-routing.module';
import { AccountService } from './services/account.service';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [
    AccountRoutingModule,
    CommonModule,
    SharedModule
  ],
  providers: [ AccountService ],

  declarations: [LoginComponent],

  

})
export class AccountModule { }
