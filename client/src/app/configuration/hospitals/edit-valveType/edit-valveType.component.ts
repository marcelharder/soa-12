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
  valvesize: valveSize = { SizeId: 0,Size: 0, VTValveTypeId: 0, EOA: 0.0, ValveTypeId: 0};
  listOfSizes: Array<valveSize> = [];

  constructor(
    private vs: ValveService,
    private alertify: ToastrService,
    private router: Router,) { }

  ngOnInit() {

    this.listOfSizes = this.getSizes(this.valve.ValveTypeId);

  }

  back() { this.result.emit(1); }

  getPatchType() { if (this.valve.Type === "Pericardial Patch") { return true }; }

  saveBacktoToDB() {
    this.result.emit(1);
    if (this.canIGo()) {
      this.vs.updateSpecificHospitalValve(this.valve).subscribe(() => { }, (error) => { })
    }
  }

  changeSize() { }


  getSizes(mc: number): Array<valveSize> {
      this.vs.getValveCodeSizes(mc).subscribe(res => {
      this.listOfSizes = res;
    })
    return this.listOfSizes;
  }

  displayChangeSize() { if (this.ch === 1) { return true; } }
  Cancel() { }


  canIGo(): boolean {
    let help = false;
    if (this.valve.Valve_size === null || this.valve.Valve_size === "") { this.alertify.error('Please enter valve size ...'); } else { help = true; }


    /*  const currentDate = new Date();
     if (moment(currentDate).isAfter(this.valve.expiry_date)) {
         this.alertify.error('This valve is already expired ...');
         help = false;
     } */

    if (!this.getPatchType()) {

      if (this.valve.Valve_size === null || this.valve.Valve_size === "") {
        this.alertify.error('Please enter valve size ...');
        help = false;
      }
    } else {
      if (this.valve.Patch_size === null || this.valve.Patch_size === "") {
        this.alertify.error('Please enter patch size ...');
        help = false;
      }
    }



    return help;
  }


}
