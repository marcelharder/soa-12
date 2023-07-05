import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Course } from '../_models/Course';
import { Epa_model } from '../_models/Epa_model';
import { dropItem } from '../_models/dropItem';

@Injectable({
  providedIn: 'root'
})
export class EpaService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  OnInit() {
   
  }
  public getDescriptions(): Observable<dropItem[]> {
    return this.http.get<dropItem[]>(this.baseUrl + 'training/getEpaDefinition');
  }
  public getEpas(userId: number): Observable<Epa_model[]>{
    return this.http.get<Epa_model[]>(this.baseUrl + 'training/getListEpaas/' + userId);
  }
  public updateEpa(se: Epa_model){
    return this.http.put<number>(this.baseUrl + 'training/updateEpa', se, { responseType: 'text' as 'json'});
  }

}
