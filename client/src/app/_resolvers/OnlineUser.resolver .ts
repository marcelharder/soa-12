import {Injectable} from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { onlineUsers } from '../_models/onlineUsers';
import { UserService } from '../_services/user.service';

@Injectable()

export class OnlineUserResolver implements Resolve<onlineUsers[]> {
   
    constructor(private userservice: UserService,
        private router: Router,
        private alertify: ToastrService) {

    }
    resolve(route: ActivatedRouteSnapshot): Observable<onlineUsers[]> {
         return this.userservice.getOnlineUsers().pipe(catchError(error => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/home']);
            return of(null);
        }));







    }
}

