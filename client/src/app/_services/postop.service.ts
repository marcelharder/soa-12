import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PostOp } from '../_models/PostOp';
import { environment } from '../../environments/environment';
import { ModelTimes } from '../_models/modelTimes';


@Injectable({
    providedIn: 'root'
})
export class PostOpService {
    baseUrl = environment.apiUrl;
    constructor(private http: HttpClient) { }

    getPostOP(id: string) { return this.http.get<PostOp>(this.baseUrl + 'postop/' + id); }

    savePostOp(c: PostOp) { return this.http.post<PostOp>(this.baseUrl + 'postop', c); }
          
}
