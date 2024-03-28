import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { dropItem } from 'src/app/_models/dropItem';
import { hospitalValve } from 'src/app/_models/hospitalValve';
import { valveSize } from 'src/app/_models/valveSize';
import * as moment from 'moment';
import { ValveService } from 'src/app/_services/valve.service';

@Component({
  selector: 'app-edit-valveType',
  templateUrl: './edit-valveType.component.html',
  styleUrls: ['./edit-valveType.component.css']
})
export class EditValveTypeComponent implements OnInit {

  @Output() result: EventEmitter<number> = new EventEmitter();
  @Input() valve: hospitalValve;

  PositionList = [
    { value: "Aortic", name: "Aortic" },
    { value: "Mitral", name: "Mitral" },
    { value: "Tricuspid", name: "Tricuspid" },
    { value: "Other", name: "Other" }]
  ch = 0;
  selectedPosition: any;
  valvesize: valveSize = { SizeId: 0, Size: 0, VTValveTypeId: 0, EOA: 0.0, ValveTypeId: 0 };
  listOfSizes: Array<valveSize> = [];
  addSizeRow = 0;
  neweoa = 0;
  newsize = 0;

  constructor(
    private vs: ValveService,
    private alertify: ToastrService,
    private router: Router,) { }

  ngOnInit() {this.listOfSizes = this.getSizes(this.valve.ValveTypeId);}

  back() { this.result.emit(1); }

  getPatchType() { if (this.valve.Type === "Pericardial Patch") { return true }; }

  addSize() { this.addSizeRow = 1; }

  deleteSize(sizeId: number){
    this.vs.deleteValveSize(sizeId).subscribe(res => {
      this.listOfSizes = this.listOfSizes.filter(x => x.SizeId !== sizeId);
      this.listOfSizes.sort(function (a, b) { return a.Size - b.Size; });
    }, error => { this.alertify.error(error); });
  }

  saveSize() {
    this.valvesize.EOA = this.neweoa;
    this.valvesize.Size = this.newsize;
    this.valvesize.VTValveTypeId = this.valve.ValveTypeId;
    this.vs.addValveSize(this.valvesize).subscribe(res => {
      //save this to the listofsizes
      this.listOfSizes.push(this.valvesize);
      this.listOfSizes.sort(function (a, b) { return a.Size - b.Size; });
    }, error => { this.alertify.error(error); }, () => { this.addSizeRow = 0; });
  }

  selectThisValve(id: number) {
    // get the valvetype forn the listofSizes
    this.ch = 1;
  }

  getSizes(mc: number): Array<valveSize> {
    this.vs.getValveCodeSizes(mc).subscribe(res => {
      this.listOfSizes = res;
      // sort this on valveSize
      this.listOfSizes.sort(function (a, b) { return a.Size - b.Size; });
    })
    return this.listOfSizes;
  }
  displayChangeSize() { if (this.ch === 1) { return true; } }
  displayAdd() { if (this.addSizeRow === 1) { return true; } }



 


}
