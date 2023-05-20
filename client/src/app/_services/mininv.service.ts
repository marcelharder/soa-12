import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { MinInv } from '../_models/MinInv';

@Injectable({
    providedIn: 'root'
})
export class MinInvService {
    baseUrl = environment.apiUrl;

    constructor(private http: HttpClient) { }

    OnInit() { }

    getSpecificMinInv(id: number): Observable<MinInv> { return this.http.get<MinInv>(this.baseUrl + 'MinInv/'+ id); }
    saveMinInv(min: MinInv): Observable<MinInv> { return this.http.post<MinInv>(this.baseUrl + 'MinInv', min); }


}
