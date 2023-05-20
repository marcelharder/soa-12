import {Injectable} from '@angular/core';
import { User } from '../_models/User';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from '../_services/user.service';
import { Observable, of } from 'rxjs';
import { catchError, take } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Injectable()

export class ProfileResolver implements Resolve<User> {
    private _currentUserId = 0;
    constructor(
        private userservice: UserService,
        private auth: AccountService,
        private router: Router,
        private alertify: ToastrService) {

    }
    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        
        this.auth.currentUser$.pipe(take(1)).subscribe((u) => {
             this._currentUserId = u.UserId;});
        return this.userservice.getUser(this._currentUserId).pipe(catchError(error => {
            this.alertify.error('Problem retrieving your data');
            this.router.navigate(['/home']);
            return of(null);
        }));
    }
}

