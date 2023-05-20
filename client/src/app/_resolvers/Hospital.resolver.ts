import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Hospital } from '../_models/Hospital';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HospitalService } from '../_services/hospital.service';
import { ToastrService } from 'ngx-toastr';
@Injectable()

export class HospitalResolver implements Resolve<Hospital> {
    constructor(
        private router: Router,
        private hos: HospitalService,
        private alertify: ToastrService) {

    }
    resolve(route: ActivatedRouteSnapshot): Observable<Hospital> {
        return this.hos.getSpecificHospital(route.params.id).pipe(catchError(error => {
            this.alertify.error('Problem retrieving your data');
            this.router.navigate(['/home']);
            return of(null);
        }));
    }
}
