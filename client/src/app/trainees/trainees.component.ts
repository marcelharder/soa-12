import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Pagination } from '../_models/pagination';
import { User } from '../_models/User';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-trainees',
  templateUrl: './trainees.component.html',
  styleUrls: ['./trainees.component.css']
})
export class TraineesComponent implements OnInit {
  currentHospital = "";
  listOfTrainees: User[] = [];
  pagination: Pagination;
  constructor(
    private auth: AccountService,
    private router: Router, 
    private route: ActivatedRoute, 
    private alertify: ToastrService) { }

  ngOnInit() {
    this.auth.currentHospitalName.subscribe((next)=>{
      this.currentHospital = next;
    });

    // get the trainees from the trainee resolver
    this.route.data.subscribe((data) => {
      this.listOfTrainees = data['users'].result;
    }, error => {this.alertify.error(error);});
 
  }

  //show details van de employee, gets the id from the @output
  toEmployeeDetails(id: number){this.router.navigate(['/editTrainee/' + id]);


  }

}
