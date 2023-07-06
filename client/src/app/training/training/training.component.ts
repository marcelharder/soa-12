import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-training',
  templateUrl: './training.component.html',
  styleUrls: ['./training.component.css']
})
export class TrainingComponent implements OnInit {

  epa = 0;
  doc = 0;

  constructor(private alertify: ToastrService) { }

  ngOnInit() {
  }

  showDocuments(){this.epa = 0;this.doc = 1;}
  showCourses(){this.alertify.info("Courses are showing now")}
  showEpa(){this.epa = 1;this.doc = 0;}
  showCME(){this.alertify.info("CME are showing now")}

  showEpaPanel(){if(this.epa === 1){return true;} else {return false;}}
  showDocPanel(){if(this.doc === 1){return true;} else {return false;}}

}
