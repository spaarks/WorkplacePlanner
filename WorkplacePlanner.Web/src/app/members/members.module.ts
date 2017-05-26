import { NgModule } from '@angular/core';
import { DndModule } from 'ng2-dnd';

import { SharedModule } from '../shared/shared.module';

import { MembersComponent } from './members.component';
import { MemberService } from './services/member.service';
import { MembersRoutingModule } from './members-routing.module';

@NgModule({
    imports: [
        SharedModule,
        MembersRoutingModule,
        DndModule.forRoot()
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
