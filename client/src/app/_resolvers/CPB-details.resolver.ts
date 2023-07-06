import {Injectable} from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { CPB } from '../_models/CPB';
import { CPBService } from '../_services/cpb.service';
@Injectable()

export class CPBDetailsResolver implements Resolve<CPB> {
    constructor(private cpbservice: CPBService,
        private router: Router,
        private alertify: ToastrService) {
  }
    resolve(route: ActivatedRouteSnapshot): Observable<CPB> {
        return this.cpbservice.getCPB(route.params.id).pipe(catchError(error => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/home']);
            return of(null);
        }));
    }
}
