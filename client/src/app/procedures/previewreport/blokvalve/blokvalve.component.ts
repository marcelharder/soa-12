import { Component, Input, OnInit } from '@angular/core';
import { Valve } from 'src/app/_models/Valve';

@Component({
  selector: 'app-blokvalve',
  templateUrl: './blokvalve.component.html',
  styleUrls: ['./blokvalve.component.css']
})
export class BlokvalveComponent implements OnInit {
  @Input() vd: any;

  constructor() {   }
  ngOnInit(): void {    }
}

