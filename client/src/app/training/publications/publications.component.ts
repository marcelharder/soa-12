import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-publications',
  templateUrl: './publications.component.html',
  styleUrls: ['./publications.component.css']
})
export class PublicationsComponent implements OnInit {
@Input() userId: number;
details = 0;
  constructor() { }

  ngOnInit() {
  }
  showDetailsPanel(){if(this.details == 1){return true;}else{return false;}}
}
