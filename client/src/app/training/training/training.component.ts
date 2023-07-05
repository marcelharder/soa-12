import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-training',
  templateUrl: './training.component.html',
  styleUrls: ['./training.component.css']
})
export class TrainingComponent implements OnInit {

  epa = 0;

  constructor(private alertify: ToastrService) { }

  ngOnInit() {
  }

  showDocuments(){this.alertify.info("Documents are showing now")}
  showCourses(){this.alertify.info("Courses are showing now")}
  showEpa(){this.alertify.info("Epa are showing now"); this.epa = 1;}
  showCME(){this.alertify.info("CME are showing now")}

  showEpaPanel(){if(this.epa === 1){return true;} else {return false;}}

}
