import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { PreViewReportService } from '../_services/pre-view-report.service';
import { previewReport } from '../_models/previewReport';
import { ToastrService } from 'ngx-toastr';

@Injectable()

export class PreviewReportResolver implements Resolve<previewReport> {
    constructor(private previewservice: PreViewReportService,
        private router: Router,
        private alertify: ToastrService) {
    }
    resolve(route: ActivatedRouteSnapshot): Observable<previewReport> {
        return this.previewservice.getPreView(route.params.id).pipe(catchError(error => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/home']);
            return of(null);
        }));
    }
}
