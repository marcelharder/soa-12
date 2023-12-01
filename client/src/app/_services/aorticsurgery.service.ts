import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { AorticSurgery } from '../_models/AorticSurgery';

@Injectable({
    providedIn: 'root'
})
export class AorticSurgeryService {
    baseUrl = environment.apiUrl;
    constructor(private http: HttpClient) { }

    getAOS(id: string) { return this.http.get<AorticSurgery>(this.baseUrl + 'AorticSurgery/' + id); }
    saveAOS(c: AorticSurgery) { return this.http.post<AorticSurgery>(this.baseUrl + 'AorticSurgery', c); }

}
