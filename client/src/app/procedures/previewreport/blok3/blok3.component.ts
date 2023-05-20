import { Component, Input, OnInit } from '@angular/core';
import { previewReport } from 'src/app/_models/previewReport';

@Component({
  selector: 'app-blok3',
  templateUrl: './blok3.component.html',
  styleUrls: ['./blok3.component.css']
})
export class Blok3Component implements OnInit {
  @Input() pre: previewReport;
  constructor() { }

  ngOnInit() {
  }

}
