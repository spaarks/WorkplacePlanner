import { NgModule, Optional, SkipSelf } from '@angular/core';

import { SharedModule } from '../shared/shared.module';
import { throwIfAlreadyLoaded } from './module-import-guard';
import { CommonDataService } from './services/common-data.service';

@NgModule({
    imports: [
        SharedModule
    ],
    declarations: [

    ],
    providers: [
        CommonDataService
    ]
})

export class CoreModule {
    constructor( @Optional() @SkipSelf() parentModule: CoreModule) {
        throwIfAlreadyLoaded(parentModule, 'CoreModule');
    }
}