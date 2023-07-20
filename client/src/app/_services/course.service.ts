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
  getCourses(id: number): Observable<Course[]> { return this.http.get<Course[]>(this.baseUrl + 'training/Courses/' + id); }
  getCourse(id: number): Observable<Course> { return this.http.get<Course>(this.baseUrl + 'training/CourseDetails/' + id); }
  createCourse(id: number){ return this.http.post<Course>(this.baseUrl + 'training/AddCourse/' + id, null);}
  removeCourse(id: number){return this.http.delete<string>(this.baseUrl + 'training/DeleteCourse/' + id,{ responseType: 'text' as 'json' }); }
  updateCourse(item: Course){return this.http.put<string>(this.baseUrl + 'training/UpdateCourse',item);}
}
