import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-publications',
  templateUrl: './publications.component.html',
  styleUrls: ['./publications.component.css']
})
export class PublicationsComponent implements OnInit {
@Input() UserId: number;
  constructor() { }

  ngOnInit() {
  }

}
