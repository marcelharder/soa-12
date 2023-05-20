import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { discharge } from '../_models/Discharge';
import { DischargeService } from '../_services/discharge.service';

@Injectable()


export class dischargeDetailsResolver implements Resolve<discharge> {
    constructor(private dischargeservice: DischargeService,
        private router: Router,
        private alertify: ToastrService) {
    }
    resolve(route: ActivatedRouteSnapshot): Observable<discharge> {
        return this.dischargeservice.getDischarge(route.params['id']).pipe(catchError(error => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/home']);
            return of(null);
        }));
    }
}

