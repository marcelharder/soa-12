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
  procedures_found = 0;
  user: User;
  procedures: Array<Partial<Procedure>> = [];

  constructor(private route: ActivatedRoute, private train: TrainingService, private alertify: ToastrService) { }

  ngOnInit() {
    this.route.data.subscribe((data: { user: User }) => { this.user = data.user; });
    // get the list of procedures done by this resident
    this.train.getProcedures(this.user.Id).subscribe((next) => {
      if (next === 'no procedure found') { this.procedures_found = 0; } else {
        this.procedures_found = 1;
        this.train.getProcedureArray(this.user.Id).subscribe((res) => { this.procedures = res; })
      }
    }
    )
  }

  showProcedures() { if (this.procedures_found != 0) { return true; } }

}
