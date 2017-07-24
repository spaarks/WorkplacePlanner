import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Router } from '@angular/router';

import { Observable } from 'rxjs/Rx';

import { AppConfigService } from '../../core/services/app-config.service';
import { MessageService } from '../../core/services/message.service';
import { QueryStringParam } from '../models/query-string-param';

@Injectable()
export class DataService {

    private apiUrl: string;

    constructor(private http: Http,
        private appConfigService: AppConfigService,
        private messageService: MessageService,
        private router: Router) {
        this.apiUrl = this.appConfigService.getApiUrl();
    }

    get(controler: string, action: string = '', urlParams?: string[], param?: QueryStringParam[]): Observable<Response> {
        let url = this.createUrl(controler, action, urlParams, param);
        return this.http
            .get(url, { headers: this.getHeaders() })
            .catch(res => this.handleException(res));
    }

    getById(controler: string, action: string = '', id: number): Observable<Response> {
        return this.get(controler, action, [id.toString()]);
    }

    put(controler: string, action: string, data?: any): Observable<Response> {
        let url = this.createUrl(controler, action);
        return this.http
            .put(url, JSON.stringify(data), { headers: this.getHeaders() })
            .catch(res => this.handleException(res));
    }

    putById(controler: string, action: string, id: number, data?: any): Observable<Response> {
        let url = this.createUrl(controler, action, [id.toString()]);
        return this.http
            .put(url, JSON.stringify(data), { headers: this.getHeaders() })
            .catch(res => this.handleException(res));
    }

    post(controler: string, action: string, data: any): Observable<Response> {
        let url = this.createUrl(controler, action);
        return this.http
            .post(url, JSON.stringify(data), { headers: this.getHeaders() })
            .catch(res => this.handleException(res));
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

    private handleException(error: Response): Observable<Response> {
        if (error.status == 401) {
            localStorage.removeItem("authToken");
            sessionStorage.removeItem("authToken");

            let link = ['/account/login', { returnUrl: '' } ]; //TODO
            this.router.navigate(link);
        } else {
            this.messageService.showError(error.json());
        }
        return Observable.throw(error);
    }

    private getHeaders(): Headers {
        var authToken = localStorage.getItem("authToken");
        if (authToken == null)
            authToken = sessionStorage.getItem('authToken');

        return new Headers({
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Authorization': 'bearer ' + authToken
        });
    }
}