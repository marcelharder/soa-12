import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Ltx } from '../_models/Ltx';

@Injectable({
    providedIn: 'root'
})
export class LtxService {
    baseUrl = environment.apiUrl;
    constructor(private http: HttpClient) { }

    getLTX(id: string) { return this.http.get<Ltx>(this.baseUrl + 'ltx/' + id); }
    saveLTX(c: Ltx) { return this.http.post<Ltx>(this.baseUrl + 'ltx', c); }


}
