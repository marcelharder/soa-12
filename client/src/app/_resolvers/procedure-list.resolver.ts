import {Injectable} from '@angular/core';
import { Procedure } from '../_models/Procedure';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ProcedureService } from '../_services/procedure.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()

export class ProcedureListResolver implements Resolve<Procedure[]> {
    pageNumber = 1;
    pageSize = 10;
    constructor(
        private procedureservice: ProcedureService,
        private alertify: ToastrService,
        private router: Router) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Procedure[]> {
        return this.procedureservice.getProcedures(this.pageNumber, this.pageSize)
        .pipe(catchError(error => {
            this.alertify.error(error);
            this.router.navigate(['/home']);
            return of(null);
        }));
    }


}
