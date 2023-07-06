import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { User } from '../_models/User';
import { environment } from '../../environments/environment';
import { onlineUsers } from '../_models/onlineUsers';
import { EmailModel } from '../_models/EmailModel';
import { SubscriptionExtensionModel } from '../_models/SubscriptionExtensionModel';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getUsers(page?, itemsPerPage?): Observable<PaginatedResult<User[]>> {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    return this.http
      .get<User[]>(this.baseUrl + 'users', { observe: 'response', params })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }
  getAios(page?, itemsPerPage?): Observable<PaginatedResult<User[]>> {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();
    let params = new HttpParams();
       if (page != null && itemsPerPage != null) {
         params = params.append('pageNumber', page);
         params = params.append('pageSize', itemsPerPage);
         }
      

    return this.http
      .get<User[]>(this.baseUrl + 'users/getAios', { observe: 'response', params })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }
  getTrainees(page?, itemsPerPage?): Observable<PaginatedResult<User[]>> {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();
    let params = new HttpParams();
       if (page != null && itemsPerPage != null) {
         params = params.append('pageNumber', page);
         params = params.append('pageSize', itemsPerPage);
         }
    return this.http
      .get<User[]>(this.baseUrl + 'users/getAiosByHospital', { observe: 'response', params })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }
  getUsersByHospital( center_id: string, page?, itemsPerPage?): Observable<PaginatedResult<User[]>>{
  
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

  let params = new HttpParams();

  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page);
    params = params.append('center_id', center_id);
    params = params.append('pageSize', itemsPerPage);
  }

  return this.http
    .get<User[]>(this.baseUrl + 'users/getUsersByHospital', { observe: 'response', params })
    .pipe(
      map((response) => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(
            response.headers.get('Pagination')
          );
        }
        return paginatedResult;
      })
    );
  }
  getSurgeonsByHospital( center_id: string, page?, itemsPerPage?): Observable<PaginatedResult<User[]>>{
  
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

  let params = new HttpParams();

  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page);
    params = params.append('center_id', center_id);
    params = params.append('pageSize', itemsPerPage);
  }

  return this.http
    .get<User[]>(this.baseUrl + 'users/getSurgeonsByHospital', { observe: 'response', params })
    .pipe(
      map((response) => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(
            response.headers.get('Pagination')
          );
        }
        return paginatedResult;
      })
    );
  }
  getChefsByHospital( center_id: string, page?, itemsPerPage?): Observable<PaginatedResult<User[]>>{
  
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

  let params = new HttpParams();

  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page);
    params = params.append('center_id', center_id);
    params = params.append('pageSize', itemsPerPage);
  }

  return this.http
    .get<User[]>(this.baseUrl + 'users/getChefsByHospital', { observe: 'response', params })
    .pipe(
      map((response) => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(
            response.headers.get('Pagination')
          );
        }
        return paginatedResult;
      })
    );
  }
 
  getUser(id: number): Observable<User> {return this.http.get<User>(this.baseUrl + 'users/' + id); }
  getNewUser(id: number): Observable<User> {return this.http.get<User>(this.baseUrl + 'users/newUser/' + id); }
  updateUser(id: number, user: User) {return this.http.put(this.baseUrl + 'users/' + id, user);  }
  getLtk(id: number){return this.http.get<boolean>(this.baseUrl + 'users/ltk/' + id);}
  deleteUser(id: number) {return this.http.delete(this.baseUrl + 'users/' + id,{ responseType: 'text' as 'json' });  }
  addUser(model: any) {return this.http.post<User>(this.baseUrl + 'users/' + 'addUser/', model); }
  getOnlineUsers(){return this.http.get<onlineUsers[]>(this.baseUrl + 'users/users-online');}
  hardResetPWD(email: string, pwd: string){return this.http.get<string>(this.baseUrl + 'account/hardresetPassword/' + email + '/' + pwd, { responseType: 'text' as 'json'})};
  sendExtensionRequest(req: SubscriptionExtensionModel){return this.http.post<string>(this.baseUrl + 'General/extendSubscription/', req); }
}
