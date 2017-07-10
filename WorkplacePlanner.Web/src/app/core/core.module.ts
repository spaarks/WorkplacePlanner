import { NgModule, Optional, SkipSelf } from '@angular/core';

import { GrowlModule } from 'primeng/primeng';
import { throwIfAlreadyLoaded } from './module-import-guard';

import { AppConfigService } from './services/app-config.service';
import { MessageService } from './services/message.service';
import { GrowlMessageComponent } from './growl-message/growl-message.component';


@NgModule({
    imports: [
        GrowlModule
    ],
    declarations: [
        GrowlMessageComponent
    ],
    providers: [
        AppConfigService,
        MessageService
    ],
    exports: [
        GrowlMessageComponent
    ]
})

export class CoreModule {
    constructor( @Optional() @SkipSelf() parentModule: CoreModule) {
        throwIfAlreadyLoaded(parentModule, 'CoreModule');
    }
}