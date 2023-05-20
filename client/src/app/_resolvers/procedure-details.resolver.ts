import {Injectable} from '@angular/core';;
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ProcedureService } from '../_services/procedure.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ProcedureDetails } from '../_models/procedureDetails';
import { ToastrService } from 'ngx-toastr';

@Injectable()

export class ProcedureDetailsResolver implements Resolve<ProcedureDetails> {
    constructor(private procservice: ProcedureService,
        private router: Router,
        private alertify: ToastrService) {

    }
    resolve(route: ActivatedRouteSnapshot): Observable<ProcedureDetails> {
        return this.procservice.getProcedure(route.params['id']).pipe(catchError(error => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/home']);
            return of(null);
        }));
    }
}


