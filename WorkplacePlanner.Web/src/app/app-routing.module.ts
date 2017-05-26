import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// Layouts
import { FullLayoutComponent } from './layouts/full-layout.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full',
  },
  {
    path: '',
    component: FullLayoutComponent,
    data: {
      title: 'Home'
    },
    children: [
      {
        path: 'dashboard',
        loadChildren: './calendar/calendar.module#CalendarModule'
        
      },
      {
        path: 'teams',
        loadChildren: './teams/teams.module#TeamsModule',
        data: {
          title: "Teams"
        }
      },
      {
        path: 'members',
        loadChildren: './members/members.module#MembersModule'
      },
      {
        path: 'reports',
        loadChildren: './reports/reports.module#ReportsModule'
      }
    ]
  }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
