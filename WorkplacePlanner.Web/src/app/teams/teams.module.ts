import { NgModule } from '@angular/core';

import { SharedModule } from '../shared/shared.module';
import { SpinnerModule, DropdownModule } from 'primeng/primeng';

import { TeamEditorComponent } from './team-editor.component';
import { TeamEditComponent } from './team-edit/team-edit.component';
import { TeamService } from './services/team.service';
import { TeamsRoutingModule } from './teams-routing.module';
import { IndentTreeTableDirective } from './shared/indent-tree-table'; 
import { TeamListComponent } from './team-list/team-list.component';
import { TeamPickerComponent } from './team-picker/team-picker.component';

@NgModule({
    imports: [
        DropdownModule,
        SharedModule,
        TeamsRoutingModule,
        SpinnerModule,
    ],
    declarations: [ 
        TeamEditComponent,
        TeamEditorComponent,
        TeamListComponent,
        IndentTreeTableDirective,
        TeamPickerComponent
    ],
    providers: [
        TeamService
    ],
    exports: [
        TeamPickerComponent
    ]
})

export class TeamsModule {
    
}

