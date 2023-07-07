import { Component, Input, OnInit } from '@angular/core';
import { previewReport } from 'src/app/_models/previewReport';

@Component({
  selector: 'app-blok2',
  templateUrl: './blok2.component.html',
  styleUrls: ['./blok2.component.css']
})
export class Blok2Component {
  @Input() pre: previewReport;

  constructor() {

  }
}
