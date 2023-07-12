import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Document_model } from '../_models/Document_model';

@Injectable({
  providedIn: 'root'
})
export class DocumentService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public getDocuments(userId: number): Observable<Document_model[]> {
    return this.http.get<Document_model[]>(this.baseUrl + 'training/getListDocuments/' + userId);
  }

  public getSpecificDocument(docId: number): Observable<Document_model> {
    return this.http.get<Document_model>(this.baseUrl + 'training/getSpecificFile/' + docId);
  }

  public deleteDocument(docId: number): Observable<string> {
    return this.http.delete<string>(this.baseUrl + 'training/deleteDocument/' + docId, { responseType: 'text' as 'json'});
  }

  public updateDocument(doc: Document_model, docId: number): Observable<Document_model> {
    return this.http.put<Document_model>(this.baseUrl + 'training/updateDocument/' + docId,doc, { responseType: 'text' as 'json'});
  }

  public createDocument(userId: number): Observable<Document_model> {
    return this.http.post<Document_model>(this.baseUrl + 'training/createDocument/' + userId, null);
  }

}
