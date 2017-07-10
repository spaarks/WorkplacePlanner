import { NgModule } from '@angular/core';
import { DndModule } from 'ng2-dnd';

import { SharedModule } from '../shared/shared.module';
import { UsersModule } from '../users/users.module';
import { TeamsModule } from '../teams/teams.module';

import { MembersComponent } from './members.component';
import { MemberService } from './services/member.service';
import { MembersRoutingModule } from './members-routing.module';

@NgModule({
    imports: [
        DndModule.forRoot(),
        MembersRoutingModule,
        UsersModule,
        SharedModule,
        TeamsModule
    ],
    declarations: [
        MembersComponent
    ],
    providers: [
        MemberService
    ]
})

export class MembersModule {

}
