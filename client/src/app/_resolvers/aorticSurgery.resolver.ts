import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AorticSurgery } from '../_models/AorticSurgery';
import { AorticSurgeryService } from '../_services/aorticsurgery.service';
@Injectable()

export class AorticSurgeryResolver implements Resolve<AorticSurgery> {
    constructor(private aosservice: AorticSurgeryService,
        private router: Router,
        private alertify: ToastrService) {
    }
    resolve(route: ActivatedRouteSnapshot): Observable<AorticSurgery> {
        return this.aosservice.getAOS(route.params.id).pipe(catchError(error => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/home']);
            return of(null);
        }));
    }
}
