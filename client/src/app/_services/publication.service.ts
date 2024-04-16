import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Publication } from '../_models/CME/Publication';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PublicationService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

getListOfPrublications(userId: number): Observable<Publication[]> {return this.http.get<Publication[]>(this.baseUrl + 'Training/getListPublications/' + userId);}

getSpecificPrublication(publicationId: number): Observable<Publication> {return this.http.get<Publication>(this.baseUrl + 'Training/getSpecificPublication/' + publicationId);}

createPublication(userId: number): Observable<Publication> {return this.http.post<Publication>(this.baseUrl + 'Training/createPublication/' + userId,null);}

updatePublication(up: Publication): Observable<string> {return this.http.put<string>(this.baseUrl + 'Training/updatePublication',up, { responseType: 'text' as 'json'});}

deletePublication(sel: number): Observable<string> {return this.http.delete<string>(this.baseUrl + 'Training/deletePublication/' + sel);}

}
