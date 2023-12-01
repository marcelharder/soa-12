import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { RefPhysModel } from '../_models/RefPhysModel';
import { RefPhysService } from '../_services/refPhys.service';

@Injectable()

export class RefPyhsResolver implements Resolve<RefPhysModel> {
    constructor(private refservice: RefPhysService,
        private router: Router,
        private alertify: ToastrService) {
    }
    resolve(route: ActivatedRouteSnapshot): Observable<RefPhysModel> {
        return this.refservice.getSpecificRefPhys(route.params['id']).pipe(catchError(error => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/home']);
            return of(null);
        }));
    }
}
