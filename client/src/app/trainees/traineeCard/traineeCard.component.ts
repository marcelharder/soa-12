import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { User } from 'src/app/_models/User';

@Component({
  selector: 'app-traineeCard',
  templateUrl: './traineeCard.component.html',
  styleUrls: ['./traineeCard.component.css']
})
export class TraineeCardComponent implements OnInit {
@Input() u: User;
@Output() eId = new EventEmitter<number>();

  constructor() { }

  ngOnInit() {
    
  }
  ejectId(){
     this.eId.emit(this.u.Id);
  }

}
