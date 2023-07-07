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

  public updateDocument(doc: Document_model): Observable<Document_model> {
    return this.http.put<Document_model>(this.baseUrl + 'training/updateDocument',doc, { responseType: 'text' as 'json'});
  }

  public postDocument(help: Document_model): Observable<boolean> {
    const formData = new FormData();
    for (const prop in help) {
      if (!help.hasOwnProperty(prop)) { continue; }
      formData.append(prop , help[prop]);
    }
    return this.http.post(this.baseUrl + 'training/upload-pdf', formData).pipe(map(x => true));
  }

}
