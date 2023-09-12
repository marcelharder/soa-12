import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, ReplaySubject } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { loginModel } from '../_models/loginModel';
import { onlineUsers } from '../_models/onlineUsers';
import { Procedure } from '../_models/Procedure';
import { User } from '../_models/User';
import { PresenceService } from './presence.service';
import * as moment from 'moment';
import { stringify } from 'querystring';
import { ForgotPassword } from '../_models/ForgotPassword';
import { resetPasswordDto } from '../_models/resetPasswordDto';
import { changePassword } from '../_models/changePassword';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1);
  private currentProcedureSource = new ReplaySubject<number>(1);
  private currentHospitalId = new ReplaySubject<number>(1);
  private newRegisteredUserSource = new ReplaySubject<User>(1);
  private serviceLevel = new ReplaySubject<number>(1);

  currentUser$ = this.currentUserSource.asObservable();
  newlyRegisteredUser$ = this.newRegisteredUserSource.asObservable();
  currentServiceLevel$ = this.serviceLevel.asObservable();
  currentProcedure$ = this.currentProcedureSource.asObservable();
  currentHospitalId$ = this.currentHospitalId.asObservable();


  soortProcedure = new BehaviorSubject<string>('0');
  currentSoortProcedure = this.soortProcedure.asObservable();


  HospitalName = new BehaviorSubject<string>('0');
  currentHospitalName = this.HospitalName.asObservable();

  dst = new BehaviorSubject<string>('0');
  currentDst = this.dst.asObservable();


  constructor(private http: HttpClient, private presence: PresenceService) { }

  login(model: loginModel) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        this.setCurrentUser(user);
       })
    );

  }


  register(model: any) {
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((response: User) => {
        const user = response;
        this.setCurrentUser(user);
      })
    )
  }

  setCurrentUser(user: User) {
    // find out if this is a premium client
    const currentDate = new Date();
    if (moment(user.paidTill).year() === 1) { this.serviceLevel.next(0) } else {
      if (moment(currentDate).isBefore(user.paidTill)) {
        this.serviceLevel.next(1);
      } else { this.serviceLevel.next(0); }
    }
    user.roles = [];
    const roles = this.getDecodedToken(user.Token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);

    this.presence.createHubConnection(user);
  }

  setCurrentProcedure(procedureId: number) { this.currentProcedureSource.next(procedureId); }
  setCurrentHospitalId(hospitalId: number) { this.currentHospitalId.next(hospitalId); }
  changeSoortOperatie(sh: string) { this.soortProcedure.next(sh); }
  changeCurrentHospital(sh: string) { this.HospitalName.next(sh); }
  changeDst(sh: string) { this.dst.next(sh); }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.presence.stopHubConnection();
  }

  /*  changePassword(u: User, pwd_02: string) {
     return this.http.put(this.baseUrl + 'account/changePassword/' + pwd_02, u).pipe(
       map((response: User) => {
         const user = response;
         if (user) { localStorage.setItem('user', JSON.stringify(user)) };
         this.currentUserSource.next(user);
       })
     );
   } */

  isThisEmailInDatabase(email: string) {
    return this.http.get<number>(this.baseUrl + 'account/checkIfUserExists/' + email)
      .pipe(
        map(data => {
          return data;
        }),
        catchError(err => {
          console.log('Error occurred: ', err);
          return of(false);
        })
      );
  }
  getDecodedToken(token) { return JSON.parse(atob(token.split('.')[1])); }
  forgotPassword(body: ForgotPassword) {return this.http.post(this.baseUrl + 'account/forgotPassword', body); }
  resetPassword(body: resetPasswordDto) { return this.http.post<string>(this.baseUrl + 'account/resetPassword', body, { responseType: 'text' as 'json' }); }
  changePassword(body: changePassword) { return this.http.put<string>(this.baseUrl + 'account/changePassword', body, { responseType: 'text' as 'json' }); }


}
