import {Injectable} from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Epa_model } from '../_models/Epa_model';
import { EpaService } from '../_services/epa.service';

@Injectable()

export class AioEpaResolver implements Resolve<Epa_model[]> {
    pageNumber = 1;
    pageSize = 12;
    
    constructor(private epaservice: EpaService,
        private router: Router,
        private alertify: ToastrService) {

    }
    resolve(route: ActivatedRouteSnapshot): Observable<Epa_model[]> {
         return this.epaservice.getEpas(route.params.id).pipe(catchError(error => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/home']);
            return of(null);
        }));







    }
}

