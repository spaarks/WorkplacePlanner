import { Injectable } from '@angular/core';

@Injectable()
export class AppConfigService {

    private apiUrl = 'http://localhost:3500/api';

    private environment = 'dev';

    public getApiUrl() : string {
         return this.apiUrl;   
    }
}
