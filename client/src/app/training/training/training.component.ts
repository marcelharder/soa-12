import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/_models/User';
import { AccountService } from 'src/app/_services/account.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-training',
  templateUrl: './training.component.html',
  styleUrls: ['./training.component.css']
})
export class TrainingComponent implements OnInit {

  epa = 0;
  doc = 1;
  courses = 0;
  cme = 0;
  currentUserId = 0;
  currentCountry = "";
  currentUser: User = {
    UserId: 0,
    Id: 0,
    hospital_id: 0,
    password: '',
    UserName: '',
    Token: '',
    roles: [],
    knownAs: '',
    age: 0,
    gender: '',
    created: undefined,
    image: '',
    lastActive: undefined,
    PhotoUrl: '',
    city: '',
    mobile: '',
    email: '',
    country: '',
    worked_in: '',
    active: false,
    ltk: false,
    paidTill: undefined
  };

  constructor(private auth: AccountService, private us: UserService) { }

  ngOnInit() {
    this.auth.currentUser$.subscribe((u)=>{
      
      this.currentUserId = u.UserId;
      this.us.getUser(this.currentUserId).subscribe((next)=>{
       this.currentCountry = next.country;
      })
    }
      
      );
  }

  showEpaButton(){if(this.currentCountry === "NL")return true; else return false;}

  showDocuments(){this.epa = 0;this.cme = 0; this.courses = 0;this.doc = 1;}
  showCourses()  {this.epa = 0;this.cme = 0; this.courses = 1;this.doc = 0;}
  showEpa()      {this.epa = 1;this.cme = 0; this.courses = 0;this.doc = 0;}
  showCME()      {this.epa = 0;this.cme = 1; this.courses = 0;this.doc = 0;}

  showEpaPanel(){if(this.epa === 1){return true;} else {return false;};}
  showDocPanel(){if(this.doc === 1){return true;} else {return false;}}
  showCoursesPanel(){if(this.courses === 1){return true;} else {return false;}}
  showCMEPanel(){if(this.cme === 1){return true;} else {return false;}}

}
