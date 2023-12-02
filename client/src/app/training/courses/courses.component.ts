import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Course } from 'src/app/_models/Course';
import { AccountService } from 'src/app/_services/account.service';
import { CourseService } from 'src/app/_services/course.service';

@Component({
  selector: 'app-courses',
  templateUrl: './courses.component.html',
  styleUrls: ['./courses.component.css']
})
export class CoursesComponent implements OnInit {
  @Input() userId: number;
  detailsForm: FormGroup | undefined;
  courseId = 0;
  courseP = 0;
  scm: Course = {
    courseId: 0,
    active: '',
    level: 0,
    description: '',
    title: '',
    diploma: '',
    venue_location: '',
    courseDate: undefined,
    endDate: undefined,
    price: 0,
    userId: 0
  };
  listOfCourses: Course[] = [];

  constructor(
    private cs: CourseService,
    private account: AccountService,
    private alertify: ToastrService,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.cs.getCourses(this.userId).subscribe((next) => {
      this.listOfCourses = next;
      this.initializeForm();
    })

  }

  initializeForm() {
    this.detailsForm = this.fb.group({
      CourseId: ['',],
      active: [false,],
      level: ['',],
      description: ['',],
      title: ['',],
      diploma: ['',],
      venue_location: ['',],
      courseDate: ['',],
      endDate: ['',],
      price: ['',],
      userId: ['',],
    });
  }

  addCourse() {
    this.cs.createCourse(this.userId).subscribe(
      (next) => { this.courseId = next.courseId; },
      (error) => { this.alertify.error(error) },
      () => {
        this.cs.getCourses(this.userId).subscribe((next) => { this.listOfCourses = next; });
        this.scm = this.listOfCourses.find(x => x.courseId == this.courseId);
      });
  }

  showDetailsPanel() { if (this.courseP == 1) { return true } else { return false } }

  showDetails(id: number) {
    this.courseP = 1;
    this.courseId = id;
    this.scm = this.listOfCourses.find(x => x.courseId == id);
    this.detailsForm.controls.CourseId.setValue(this.scm.courseId);
    this.detailsForm.controls.userId.setValue(this.userId);
    this.detailsForm.controls.active.setValue(this.scm.active);
    this.detailsForm.controls.description.setValue(this.scm.description);
    this.detailsForm.controls.courseDate.setValue(this.scm.courseDate);
    this.detailsForm.controls.endDate.setValue(this.scm.endDate);
    this.detailsForm.controls.title.setValue(this.scm.title);
    this.detailsForm.controls.level.setValue(this.scm.level);
    this.detailsForm.controls.diploma.setValue(this.scm.diploma);
    this.detailsForm.controls.venue_location.setValue(this.scm.venue_location);
    this.detailsForm.controls.price.setValue(this.scm.price);
    //this.cs.getCourse(id).subscribe((next)=>{this.course_model = next;})
  }

  deleteModel(id: number) {

    this.cs.removeCourse(id).subscribe((next) => { this.alertify.show("Record deleted ..") });
    this.cs.getCourses(this.userId).subscribe((next) => { this.listOfCourses = next; })
  }

  updateCourse() {
    this.alertify.show("updating record ..");
    this.listOfCourses = [];
    this.cs.updateCourse(this.detailsForm.value).subscribe(
      (next) => {
        this.cs.getCourses(this.userId).subscribe((next) => { this.listOfCourses = next; })
        this.scm = this.listOfCourses.find(x => x.courseId == this.courseId);
      },
      (error) => { this.alertify.error(error) },
      () => { this.courseP = 0; });
  }

  Cancel() { this.courseP = 0; }

}

