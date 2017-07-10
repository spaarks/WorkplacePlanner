import { NgModule } from "@angular/core";

import { SharedModule } from '../shared/shared.module';
import { UserService } from './services/user.service';
import { AddUserComponent } from './add-user/add-user.component';
import { FilterUsersPipe } from './pipes/pipes';

@NgModule({
    imports: [
        SharedModule
    ],
    providers: [
        UserService
    ],
    declarations: [
        AddUserComponent,
        FilterUsersPipe
    ],
    exports: [
        AddUserComponent,
        FilterUsersPipe
    ]
})
export class UsersModule { }