import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { OVIvalve } from 'src/app/_models/OVIvalve';
import { Valve } from 'src/app/_models/Valve';


@Component({
  selector: 'app-valvesFromOVI',
  templateUrl: './valvesFromOVI.component.html',
  styleUrls: ['./valvesFromOVI.component.css']
})
export class ValvesFromOVIComponent implements OnInit {
  @Input() optionsAvailableRingsFromOVI: Array<OVIvalve> = [];
  @Input() procedureValve: Valve;
  @Output() sendIdUp = new EventEmitter<number>();

  noRecordsFound = false;

  constructor() { }

  ngOnInit() {
    if(this.optionsAvailableRingsFromOVI.length === 0){this.noRecordsFound = true;} else {this.noRecordsFound = false;}
  }

  displayNoRecordsFound(){return this.noRecordsFound;}

  getThisOne(id: number) {
    this.sendIdUp.emit(id);
  }

}
