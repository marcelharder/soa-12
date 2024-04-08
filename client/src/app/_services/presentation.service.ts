import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Presentation } from '../_models/CME/Presentation';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PresentationService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getListOfPresentations(userId: number): Observable<Presentation[]> { return this.http.get<Presentation[]>(this.baseUrl + 'Training/getListPresentations/' + userId);  }

  getSpecificPresentation(presentationId: number): Observable<Presentation> { return this.http.get<Presentation>(this.baseUrl + 'Training/getSpecificPresentation/' + presentationId);  }

  createPresentation(userId: number): Observable<Presentation> { return this.http.post<Presentation>(this.baseUrl + 'Training/createPresentation/' + userId,null);  }

  updatePresentation(up: Presentation): Observable<string> { return this.http.put<string>(this.baseUrl + 'Training/updatePresentation',up, { responseType: 'text' as 'json'});  }

  deletePresentation(sel: number): Observable<string> { return this.http.delete<string>(this.baseUrl + 'Training/deletePresentation/' + sel, { responseType: 'text' as 'json'});  }



}
