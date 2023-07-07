import {Injectable} from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Course } from '../_models/Course';
import { CourseService } from '../_services/course.service';
import { ToastrService } from 'ngx-toastr';

@Injectable()

export class AioCourseResolver implements Resolve<Course[]> {
    pageNumber = 1;
    pageSize = 12;
    
    constructor(private cservice: CourseService,
        private router: Router,
        private alertify: ToastrService) {

    }
    resolve(route: ActivatedRouteSnapshot): Observable<Course[]> {
         return this.cservice.getCourses(route.params.id).pipe(catchError(error => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/home']);
            return of(null);
        }));







    }
}

