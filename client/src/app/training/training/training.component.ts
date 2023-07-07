import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-training',
  templateUrl: './training.component.html',
  styleUrls: ['./training.component.css']
})
export class TrainingComponent implements OnInit {

  epa = 0;
  doc = 0;
  courses = 0;
  cme = 0;
  currentUserId = 0;

  constructor(private alertify: ToastrService, private auth: AccountService) { }

  ngOnInit() {
    this.auth.currentUser$.subscribe((u)=>{this.currentUserId = u.UserId})
  }

  showDocuments(){this.epa = 0;this.cme = 0; this.courses = 0;this.doc = 1;}
  showCourses()  {this.epa = 0;this.cme = 0; this.courses = 1;this.doc = 0;}
  showEpa()      {this.epa = 1;this.cme = 0; this.courses = 0;this.doc = 0;}
  showCME()      {this.epa = 0;this.cme = 1; this.courses = 0;this.doc = 0;}

  showEpaPanel(){if(this.epa === 1){return true;} else {return false;}}
  showDocPanel(){if(this.doc === 1){return true;} else {return false;}}
  showCoursesPanel(){if(this.courses === 1){return true;} else {return false;}}
  showCMEPanel(){if(this.cme === 1){return true;} else {return false;}}

}
