import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { hospitalValve } from 'src/app/_models/hospitalValve';
import { valveSize } from 'src/app/_models/valveSize';

@Component({
  selector: 'app-edit-valveType',
  templateUrl: './edit-valveType.component.html',
  styleUrls: ['./edit-valveType.component.css']
})
export class EditValveTypeComponent implements OnInit {
  @Output() result: EventEmitter<number> = new EventEmitter();
  @Input() hv: hospitalValve;
  valveSizes:Array<valveSize> = [];
  constructor() { }

  ngOnInit() {

    this.valveSizes = this.getSizes(+this.hv.hospitalId);

  }

  back(){this.result.emit(1);}

  saveBacktoToDB(){this.result.emit(1);}


  getSizes(id: number):Array<valveSize> {
    var result:Array<valveSize> = [];


    return result;
  }
  

}
