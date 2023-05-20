import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CABG } from '../_models/CABG';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class CABGService {
    baseUrl = environment.apiUrl;
    constructor(private http: HttpClient) { }

    getCABG(id: string) { return this.http.get<CABG>(this.baseUrl + 'cabg/' + id); }
    saveCABG(c: CABG) { return this.http.post<CABG>(this.baseUrl + 'cabg', c); }

    getVSMUsed(procedure_id: number) { return this.http.get<boolean>(this.baseUrl + 'General/showVSM/'+ procedure_id); }
    getRadialUsed(procedure_id: number) { return this.http.get<boolean>(this.baseUrl + 'General/showRadial/' + procedure_id); }
    get80Used(procedure_id: number) { return this.http.get<boolean>(this.baseUrl + 'General/showTachtig/' + procedure_id); }
}
