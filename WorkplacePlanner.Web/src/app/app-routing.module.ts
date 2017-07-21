import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// Layouts
import { FullLayoutComponent } from './layouts/full-layout.component';
import { EmptyLayoutComponent } from './layouts/empty-layout.component';
import { AuthGuard } from './guards/auth.guard'

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
    ],
    canActivate: [AuthGuard]
  },
  {
    path: 'account',
    component: EmptyLayoutComponent,
    data: {
      title: 'Account'
    },
    children: [
      {
        path: '',
        loadChildren: './account/account.module#AccountModule'
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [AuthGuard]
})
export class AppRoutingModule { }
