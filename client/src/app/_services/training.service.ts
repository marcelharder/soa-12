import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Procedure } from '../_models/Procedure';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TrainingService {

  baseUrl = environment.apiUrl;
   
  constructor(private http: HttpClient) { }


  getProcedures(id: number): Observable<any> {
     return this.http.get<Partial<any>>(this.baseUrl + 'Training/getProcedures/'+ id, { responseType: 'text' as 'json'}); 
  }
  getProcedureArray(id: number): Observable<any> {
    return this.http.get<Partial<any>>(this.baseUrl + 'Training/getProcedures/'+ id); 
 }

  getProcedureDetails(id: number): Observable<Partial<Procedure>> { 
    return this.http.get<Partial<Procedure>>(this.baseUrl + 'Training/getProcedureDetails/' + id); }



}
