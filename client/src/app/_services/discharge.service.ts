import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { discharge } from '../_models/Discharge';

@Injectable()
export class DischargeService {

    baseUrl = environment.apiUrl;
    constructor(private http: HttpClient) { }

    getDischarge(id: string) { return this.http.get<discharge>(this.baseUrl + 'discharge/' + id); }
    saveDischarge(c: discharge) { return this.http.post<discharge>(this.baseUrl + 'discharge', c); }
}
