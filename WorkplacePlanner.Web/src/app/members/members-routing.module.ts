import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MembersComponent } from './members.component';

const routes: Routes = [
    {
        path: '',
        component: MembersComponent,
        data: { 
            title: "Members"
        }
    }
];

@NgModule({
    imports:[RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class MembersRoutingModule {}
