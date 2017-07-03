import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { AppConfigService } from '../../core/services/app-config.service';
import { QueryStringParam } from '../models/query-string-param';

@Injectable()
export class DataService {

    private apiUrl: string;
    private headers = new Headers({
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    });

    constructor(private http: Http, private appConfigService: AppConfigService) {
        this.apiUrl = this.appConfigService.getApiUrl();
    }

    get(controler: string, action: string = '', urlParams?: string[], param?: QueryStringParam[]): Observable<Response> {
        let url = this.createUrl(controler, action, urlParams, param);
        console.log('Get URL: ' + url);
        return this.http.get(url);
    }

    getById(controler: string, action: string = '', id: number): Observable<Response> {
        return this.get(controler, action, [id.toString()]);
    }

    update(controler: string, action: string, data?: any): Observable<Response> {
        let url = this.createUrl(controler, action);
        return this.http
            .put(url, JSON.stringify(data), { headers: this.headers })
    }

    updateById(controler: string, action: string, id: number, data?: any): Observable<Response> {
        let url = this.createUrl(controler, action, [id.toString()]);
        console.log('Update url: ' + url);
        return this.http
            .put(url, JSON.stringify(data), { headers: this.headers })
    }

    create(controler: string, action: string, data: any): Observable<Response> {
        let url = this.createUrl(controler, action);
        return this.http
            .post(url, JSON.stringify(data), { headers: this.headers })
    }

    private createUrl(controler: string, action: string, urlParams?: string[], queryStrParams?: QueryStringParam[]): string {
        let url = this.pathCombine(this.apiUrl, this.pathCombine(controler, action));

        if (urlParams != null) {
            let urlParamsStr = '';
            urlParams.forEach(param => {
                urlParamsStr += param + "/"

            });
            urlParamsStr = urlParamsStr.slice(0, -1);
            url += '/' + urlParamsStr;
        }

        if (queryStrParams != null) {
            let queryString: string = '';
            queryStrParams.forEach(param => {
                queryString += param.name + '=' + param.value + '&'
            });
            queryString = queryString.slice(0, -1);
            url = url + '?' + queryString;
        }

        return url;
    }

    private pathCombine(part1: string, part2: string): string {
        return part1 + (part2 != '' ? '/' + part2 : '');
    }
}