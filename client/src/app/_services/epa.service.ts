import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Course } from '../_models/Course';
import { Epa } from '../_models/Epa';

@Injectable({
  providedIn: 'root'
})
export class EpaService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  OnInit() {
   
  }
  //  get the courses that this aio attended
  getEpas(id: number): Observable<Epa[]> { return this.http.get<Epa[]>(this.baseUrl + 'Aio/Epas/' + id); }
  
  // get the details of this course
  getSpecificEpa(id: number): Observable<Epa> { return this.http.get<Epa>(this.baseUrl + 'Aio/EpaDetails/' + id); }


}
