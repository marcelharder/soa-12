import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Patient } from '../_models/Patient';
import { PatientService } from '../_services/patient.service';

@Injectable()

export class EuroScoreDetailsResolver implements Resolve<Patient> {
    constructor(private patservice: PatientService,
        private router: Router,
        private alertify: ToastrService) {

    }
    resolve(route: ActivatedRouteSnapshot): Observable<Patient> {
        // Nb de id hier is de procedure Id,
         return this.patservice.getPatientFromProcedureId(+route.params.id).pipe(catchError(error => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/home']);
            return of(null);
        }));
    }
}
