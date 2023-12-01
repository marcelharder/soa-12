import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { PodCast } from '../_models/PodCast';

@Injectable({
  providedIn: 'root'
})
export class PodCastService {

baseUrl = environment.apiUrl;
constructor(private http: HttpClient) { }

getArticles() {
  return this.http.get<PodCast[]>(this.baseUrl + 'podcast');
}

getSpecificArticle(id) {
  return this.http.get<PodCast>(this.baseUrl + 'article/' + id);
}

addNewArticle(id: number, inv: PodCast) {
  return this.http.post<PodCast>(this.baseUrl + 'article/' + id, inv);
}

updateArticle(id: number, inv: PodCast) {
  return this.http.put<PodCast>(this.baseUrl + 'article/' + id, inv);
}




}
