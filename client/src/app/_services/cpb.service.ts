import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { CPB } from '../_models/CPB';

@Injectable({
  providedIn: 'root'
})
export class CPBService {
baseUrl = environment.apiUrl;
constructor(private http: HttpClient) { }

    getCPB(id: string) { return this.http.get<CPB>(this.baseUrl + 'cpb/' + id); }
    saveCPB(c: CPB) { return this.http.post<CPB>(this.baseUrl + 'cpb', c); }


}
