import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-presentations',
  templateUrl: './presentations.component.html',
  styleUrls: ['./presentations.component.css']
})
export class PresentationsComponent implements OnInit {
  @Input() UserId: number;
  constructor() { }

  ngOnInit() {
  }

}
