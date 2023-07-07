import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Course } from '../_models/Course';

@Injectable({
  providedIn: 'root'
})
export class CourseService {
  
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  OnInit() {
   
  }
  //  get the courses that this aio attended
  getCourses(id: number): Observable<Course[]> { return this.http.get<Course[]>(this.baseUrl + 'aio/Courses/' + id); }
    // get the details of this course
  getCourse(id: number): Observable<Course> { return this.http.get<Course>(this.baseUrl + 'aio/CourseDetails/' + id); }
  createCourse(item: Course){ return this.http.post<Course>(this.baseUrl + 'aio/AddCourse',item);}
  removeCourse(id: number){return this.http.delete<string>(this.baseUrl + 'aio/DeleteCourse/' + id,{ responseType: 'text' as 'json' }); }
  updateCourse(item: Course){return this.http.put<string>(this.baseUrl + 'aio/UpdateCourse',item);}
}
