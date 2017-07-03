import { NgModule, Optional, SkipSelf } from '@angular/core';

import { throwIfAlreadyLoaded } from './module-import-guard';
import { AppConfigService } from './services/app-config.service';

@NgModule({
    imports: [
        
    ],
    declarations: [
        
    ],
    providers: [
        AppConfigService
    ]
})

export class CoreModule {
    constructor( @Optional() @SkipSelf() parentModule: CoreModule) {
        throwIfAlreadyLoaded(parentModule, 'CoreModule');
    }
}