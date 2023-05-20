import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { hospitalValve } from 'src/app/_models/hospitalValve';
import { Valve } from 'src/app/_models/Valve';

@Component({
  selector: 'app-selectValve',
  templateUrl: './selectValve.component.html',
  styleUrls: ['./selectValve.component.css']
})
export class SelectValveComponent implements OnInit {
  @Input() ovi: boolean;
  @Input() valveDescription: string;
  @Input() hospitalId: number;
  @Input() implant: string;
  @Input() hv: hospitalValve;
  @Input() pd: Valve;
  @Output() pushValve = new EventEmitter<Valve>();

  constructor() { }

  ngOnInit() {
  }

  getOviStatus(): boolean { return this.ovi }

  receiveNewValve(nv: Valve) {
    this.pushValve.emit(nv);
  }
 
}
