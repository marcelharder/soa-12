import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Procedure } from 'src/app/_models/Procedure';
import { User } from 'src/app/_models/User';
import { TrainingService } from 'src/app/_services/training.service';

@Component({
  selector: 'app-traineeDetails',
  templateUrl: './traineeDetails.component.html',
  styleUrls: ['./traineeDetails.component.css']
})
export class TraineeDetailsComponent implements OnInit {

  user: User;
  procedures: Array<Partial<Procedure>> = [];

  constructor(private route: ActivatedRoute, private train: TrainingService, private alertify: ToastrService) { }

  ngOnInit() {
    this.route.data.subscribe((data: { user: User }) => { this.user = data.user; });
    // get the list of procedures done by this resident
    this.train.getProcedures(this.user.Id).subscribe((next)=>
    {
      if(next === 'no procedure found'){ this.alertify.info("No procedure found");} else {this.procedures = next;}
       }
       
       )

  }

}
