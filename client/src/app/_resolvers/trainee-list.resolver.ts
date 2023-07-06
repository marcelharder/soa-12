import {Injectable} from '@angular/core';
import { User } from '../_models/User';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from '../_services/user.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()

export class TraineeListResolver implements Resolve<User[]> {
    pageNumber = 1;
    pageSize = 5;
    constructor(private userservice: UserService,
        private router: Router,
        private alertify: ToastrService) {

    }
    resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
        return this.userservice.getTrainees(this.pageNumber, this.pageSize).pipe(catchError(error => {
            this.alertify.error('No trainees found ...');
            this.router.navigate(['/']);
            return of(null);
        }));
    }
}

