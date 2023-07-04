import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-training',
  templateUrl: './training.component.html',
  styleUrls: ['./training.component.css']
})
export class TrainingComponent implements OnInit {

  constructor(private alertify: ToastrService) { }

  ngOnInit() {
  }

  showDocuments(){this.alertify.info("Documents are showing now")}
  showCourses(){this.alertify.info("Courses are showing now")}
  showEpa(){this.alertify.info("Epa are showing now")}
  showCME(){this.alertify.info("CME are showing now")}

}
