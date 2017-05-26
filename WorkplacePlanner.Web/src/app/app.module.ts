import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';

import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ChartsModule } from 'ng2-charts/ng2-charts';

import { NAV_DROPDOWN_DIRECTIVES } from './layouts/nav-dropdown.directive';
import { SIDEBAR_TOGGLE_DIRECTIVES } from './layouts/sidebar.directive';
import { AsideToggleDirective } from './layouts/aside.directive';
import { BreadcrumbsComponent } from './layouts/breadcrumb.component';

import { SharedModule } from './shared/shared.module';
import { CalendarModule } from './calendar/calendar.module';
import { TeamsModule } from './teams/teams.module';
import { MembersModule } from './members/members.module';
import { ReportsModule } from './reports/reports.module';
import { CoreModule } from './core/core.module';

import { AppComponent } from './app.component';
import { FullLayoutComponent } from './layouts/full-layout.component';

// Routing Module
import { AppRoutingModule } from './app-routing.module';

@NgModule({
  imports: [
    BrowserModule,
    AppRoutingModule,
    BsDropdownModule.forRoot(),
    CalendarModule,
    CoreModule,
    MembersModule,
    ReportsModule,
    SharedModule,
    TabsModule.forRoot(),
    TeamsModule,
    ChartsModule
  ],
  declarations: [
    AppComponent,
    FullLayoutComponent,
    NAV_DROPDOWN_DIRECTIVES,
    BreadcrumbsComponent,
    SIDEBAR_TOGGLE_DIRECTIVES,
    AsideToggleDirective
  ],
  providers: [{
    provide: LocationStrategy,
    useClass: HashLocationStrategy
  }],
  bootstrap: [ AppComponent ]
})
export class AppModule { }
