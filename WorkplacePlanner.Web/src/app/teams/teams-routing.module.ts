import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TeamEditorComponent } from './team-editor.component';
import { TeamEditComponent } from './team-edit/team-edit.component';

const routes: Routes = [
    {
        path: '',
        component: TeamEditorComponent,
    },
    {
        path: 'new',
        component: TeamEditComponent,
        data: {
            title: "New Team"
        }
    },
    {
        path: 'edit/:id',
        component: TeamEditComponent,
        data: {
            title: "Edit Team"
        }
    },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TeamsRoutingModule {}