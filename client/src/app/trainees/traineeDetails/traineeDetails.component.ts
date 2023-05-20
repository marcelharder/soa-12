import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/User';

@Component({
  selector: 'app-traineeDetails',
  templateUrl: './traineeDetails.component.html',
  styleUrls: ['./traineeDetails.component.css']
})
export class TraineeDetailsComponent implements OnInit {

user: User;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe((data: { user: User }) => {
      this.user = data.user;
     
  });
  }

}
