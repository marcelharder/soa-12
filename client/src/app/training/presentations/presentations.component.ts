import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-presentations',
  templateUrl: './presentations.component.html',
  styleUrls: ['./presentations.component.css']
})
export class PresentationsComponent implements OnInit {
  @Input() userId: number;
  details = 0;
  constructor() { }

  ngOnInit() {
  }
  showDetailsPanel(){if(this.details == 1){return true;}else{return false;}}

}
