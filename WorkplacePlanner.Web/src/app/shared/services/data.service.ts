import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class DataService {

    apiRootUrl = 'app'

    constructor(private http: Http) {
        
    }

    private getUrl(controler: string, action: string, id?: number): string {
        return this.pathCombine(this.pathCombine(this.apiRootUrl, this.pathCombine(controler, action)), id == null ? '' : id.toString());
    }

    private pathCombine(part1: string, part2: string): string {
        return part1 + (part2 != '' ? '/' + part2 : '');
    }

    get(controler: string, action: string): Observable<Response> {
        let url = this.getUrl(controler, action);
        return this.http.get(url);
    }

    private headers = new Headers({ 'Content-Type': 'application/json' });

    update(controler: string, action: string, id: number, data?: any) : Observable<Response> {
        let url = this.getUrl(controler, action, id);
        return this.http
                .put(url, JSON.stringify(data), { headers: this.headers })
    }

    create(controler: string, action: string, data: any) : Observable<Response> {
        let url = this.getUrl(controler, action);
         return this.http
                .post(url, JSON.stringify(data), { headers: this.headers })
    }
}