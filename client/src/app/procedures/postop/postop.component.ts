import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { dropItem } from 'src/app/_models/dropItem';
import { ModelTimes } from 'src/app/_models/modelTimes';
import { PostOp } from 'src/app/_models/PostOp';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { PostOpService } from 'src/app/_services/postop.service';
import { ProcedureService } from 'src/app/_services/procedure.service';

@Component({
  selector: 'app-postop',
  templateUrl: './postop.component.html',
  styleUrls: ['./postop.component.css']
})
export class PostopComponent implements OnInit {
  @ViewChild('postOpDetailsForm') postopForm: NgForm;
  pd: PostOp;


  model: ModelTimes = {
    beginDate: new Date(),
    endDate: new Date(),
    beginHour: 0,
    endHour: 0,
  };

  imageDisplay = true;

  calculatedTime = 0;
  calculatedTimeStay = 0;
  complication = 0;
  calculatedIcuSecondarystay = 0;
  calculatedIcuTotalstay = 0;
  calculatedVentilatedSec = 0;
  calculatedVentilatedTotal = 0;

  hours: Array<string> = [];
  mins: Array<string> = [];
  optionsYN: Array<dropItem> = [];
  optionsTiming: Array<dropItem> = [];
  compOptions01: Array<dropItem> = [];
  compOptions02: Array<dropItem> = [];
  compOptions03: Array<dropItem> = [];
  compOptions04: Array<dropItem> = [];
  compOptions05: Array<dropItem> = [];
  compOptions06: Array<dropItem> = [];
  compOptions07: Array<dropItem> = [];
  compOptions08: Array<dropItem> = [];
  compOptions09: Array<dropItem> = [];
  numberOfItems: Array<string> = [];
  maxCreat: Array<string> = [];

  constructor(
    private route: ActivatedRoute,
    private drops: DropdownService,
    private proc: ProcedureService,
    private postOpservice: PostOpService,
    private alertify: ToastrService
  ) { }

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.pd = data.postop;
      this.proc.getProcedure(this.pd.procedure_id).subscribe((pr) => {
         let surgeryEnd = moment(pr.dateOfSurgery).hour(pr.selectedStopHr).minute(pr.selectedStopMin + 10);
        this.pd.ICU_ARRIVAL_DATE = surgeryEnd.toDate();
        this.pd.ICU_ARRIVAL_DATE_HOURS = moment(surgeryEnd).hour();
        this.pd.ICU_ARRIVAL_DATE_MINUTES = moment(surgeryEnd).minutes();
      })
      this.calculatedTime = this.calculateTime(this.pd.EXTUBATION_DATE);
      this.calculatedTimeStay = this.calculateTime(this.pd.ICU_DISCHARGE_DATE);
    });
    this.loadDrops();
    // check to see if there is any complication, if so then this.complication = '2'
    if (this.checkComplication()) { this.complication = 2; } else { this.complication = 1; }

  }

  loadDrops() {
    let d = JSON.parse(localStorage.getItem('YN'));
    if (d == null || d.length === 0) {
      this.drops.getYNOptions().subscribe((response) => {
        this.optionsYN = response;
        localStorage.setItem('YN', JSON.stringify(response));
      });
    } else {
      this.optionsYN = JSON.parse(localStorage.getItem('YN'));
    }
    d = JSON.parse(localStorage.getItem('autologogous_blood_timing'));
    if (d == null || d.length === 0) {
      this.drops.getAutoBloodTiming().subscribe((response) => {
        this.optionsTiming = response;
        localStorage.setItem(
          'autologogous_blood_timing',
          JSON.stringify(response)
        );
      });
    } else {
      this.optionsTiming = JSON.parse(
        localStorage.getItem('autologogous_blood_timing')
      );
    }

    d = JSON.parse(localStorage.getItem('complicationOption01'));
    if (d == null || d.length === 0) {
      this.drops.getComp_options_01().subscribe((response) => {
        this.compOptions01 = response;
        localStorage.setItem('complicationOption01', JSON.stringify(response));
      });
    } else {
      this.compOptions01 = JSON.parse(
        localStorage.getItem('complicationOption01')
      );
    }

    d = JSON.parse(localStorage.getItem('complicationOption02'));
    if (d == null || d.length === 0) {
      this.drops.getComp_options_02().subscribe((response) => {
        this.compOptions02 = response;
        localStorage.setItem('complicationOption02', JSON.stringify(response));
      });
    } else {
      this.compOptions02 = JSON.parse(
        localStorage.getItem('complicationOption02')
      );
    }
    d = JSON.parse(localStorage.getItem('complicationOption03'));
    if (d == null || d.length === 0) {
      this.drops.getComp_options_03().subscribe((response) => {
        this.compOptions03 = response;
        localStorage.setItem('complicationOption03', JSON.stringify(response));
      });
    } else {
      this.compOptions03 = JSON.parse(
        localStorage.getItem('complicationOption03')
      );
    }
    d = JSON.parse(localStorage.getItem('complicationOption04'));
    if (d == null || d.length === 0) {
      this.drops.getComp_options_04().subscribe((response) => {
        this.compOptions04 = response;
        localStorage.setItem('complicationOption04', JSON.stringify(response));
      });
    } else {
      this.compOptions04 = JSON.parse(
        localStorage.getItem('complicationOption04')
      );
    }
    d = JSON.parse(localStorage.getItem('complicationOption05'));
    if (d == null || d.length === 0) {
      this.drops.getComp_options_05().subscribe((response) => {
        this.compOptions05 = response;
        localStorage.setItem('complicationOption05', JSON.stringify(response));
      });
    } else {
      this.compOptions05 = JSON.parse(
        localStorage.getItem('complicationOption05')
      );
    }
    d = JSON.parse(localStorage.getItem('complicationOption06'));
    if (d == null || d.length === 0) {
      this.drops.getComp_options_06().subscribe((response) => {
        this.compOptions06 = response;
        localStorage.setItem('complicationOption06', JSON.stringify(response));
      });
    } else {
      this.compOptions06 = JSON.parse(
        localStorage.getItem('complicationOption06')
      );
    }
    d = JSON.parse(localStorage.getItem('complicationOption07'));
    if (d == null || d.length === 0) {
      this.drops.getComp_options_07().subscribe((response) => {
        this.compOptions07 = response;
        localStorage.setItem('complicationOption07', JSON.stringify(response));
      });
    } else {
      this.compOptions07 = JSON.parse(
        localStorage.getItem('complicationOption07')
      );
    }
    d = JSON.parse(localStorage.getItem('complicationOption08'));
    if (d == null || d.length === 0) {
      this.drops.getComp_options_08().subscribe((response) => {
        this.compOptions08 = response;
        localStorage.setItem('complicationOption08', JSON.stringify(response));
      });
    } else {
      this.compOptions08 = JSON.parse(
        localStorage.getItem('complicationOption08')
      );
    }
    d = JSON.parse(localStorage.getItem('complicationOption09'));
    if (d == null || d.length === 0) {
      this.drops.getComp_options_09().subscribe((response) => {
        this.compOptions09 = response;
        localStorage.setItem('complicationOption09', JSON.stringify(response));
      });
    } else {
      this.compOptions09 = JSON.parse(
        localStorage.getItem('complicationOption09')
      );
    }

    for (let x = 0; x < 24; x++) {
      this.hours.push(x.toString());
    }
    for (let x = 0; x < 60; x++) {
      this.mins.push(x.toString());
    }
    for (let x = 0; x < 11; x++) {
      this.numberOfItems.push(x.toString());
    }
    for (let x = 50; x < 1000; x++) {
      this.maxCreat.push(x.toString());
    }
  }
  showBloodProductsDetails() {
    if (this.pd.blood_products === '2') {
      return true;
    } else {
      return false;
    }
  }
  showAutologousBloodGiven() {
    if (this.pd.autologous_Blood === '2') {
      return true;
    } else {
      return false;
    }
  }
  showReadmis() {
    if (this.pd.readmitted === '2') {
       return true;
    } else {
      return false;
    }
  }
  showComplication() {
    if (this.complication !== null) {
      if (this.complication.toString() === '2') {
        return true;
      } else {
        return false;
      }
    }
  }
  showReintubation() {
    if (this.pd.reintubated === '2') {
      return true;
    } else {
      return false;
    }
  }
  showImage(){if(!this.showReadmis() && !this.showComplication() && !this.showReintubation()) return true}

  calculateTime(d: Date): number {
    let help = 0;
    help = (new Date(d).getTime() - new Date(this.pd.ICU_ARRIVAL_DATE).getTime()) / 3600000;
    return help;
  }
  setICUDischargeTime() {
    const currentDate1 = new Date();
    this.pd.ICU_DISCHARGE_DATE = new Date(currentDate1.getTime());
    this.pd.ICU_DISCHARGE_DATE_HOURS = currentDate1.getHours();
    this.pd.ICU_DISCHARGE_DATE_MINUTES = currentDate1.getMinutes();

    const n = this.pd.ICU_DISCHARGE_DATE.getTimezoneOffset() * 60 * 1000;
    this.pd.ICU_DISCHARGE_DATE = new Date(this.pd.ICU_DISCHARGE_DATE.getTime() - n);
    this.postOpservice.savePostOp(this.pd).subscribe((next) => { });
  }
  setExtubationTime() {
    const currentDate1 = new Date();
    this.pd.EXTUBATION_DATE = new Date(currentDate1);
    this.pd.EXTUBATION_DATE_HOURS = currentDate1.getHours();
    this.pd.EXTUBATION_DATE_MINUTES = currentDate1.getMinutes();

    const n = this.pd.EXTUBATION_DATE.getTimezoneOffset() * 60 * 1000;
    this.pd.EXTUBATION_DATE = new Date(this.pd.EXTUBATION_DATE.getTime() - n);
    this.postOpservice.savePostOp(this.pd).subscribe((next) => { });
  }
  setICUSecondDischargeTime() {
    const currentDate1 = new Date();
    this.pd.ICU_DISCHARGE_1_DATE = currentDate1;
    // this.pd.ICU_DISCHARGE_1_DATE_HOURS.setHours(currentDate1.getHours());
    // this.pd.ICU_DISCHARGE_1_DATE_MINUTES.setMinutes( currentDate1.getMinutes());
  }
  setReintubationStartTime() {
    const currentDate1 = new Date();
    this.pd.REINTUBATION_DATE = currentDate1;
    // this.pd.REINTUBATION_DATE_HOURS.setHours(currentDate1.getHours());
    // this.pd.REINTUBATION_DATE.setMinutes( currentDate1.getMinutes());
  }
  setReintubationStopTime() {
    const currentDate1 = new Date();
    this.pd.EXTUBATION_1_DATE = currentDate1;
    // this.pd.EXTUBATION_1_DATE.setHours(currentDate1.getHours());
    // this.pd.EXTUBATION_1_DATE.setMinutes( currentDate1.getMinutes());
  }
  setICUSecondArrivalTime() {
    const currentDate1 = new Date();
    this.pd.ICU_ARRIVAL_1_DATE = currentDate1;
    // this.pd.ICU_ARRIVAL_1_DATE_HOURS = currentDate1.getHours();
    // this.pd.ICU_ARRIVAL_1_DATE_MINUTES =  currentDate1.getMinutes();
  }
  savePostOp() {
    const d = new Date();
    const n = d.getTimezoneOffset() * 60 * 1000;

    let test = new Date(this.pd.ICU_DISCHARGE_DATE);
    let testString = test.toDateString();
    test = new Date(testString);
    this.pd.ICU_DISCHARGE_DATE = new Date(test.getTime() - n);

    test = new Date(this.pd.EXTUBATION_DATE);
    testString = test.toDateString();
    test = new Date(testString);
    this.pd.EXTUBATION_DATE = new Date(test.getTime() - n);


    this.postOpservice.savePostOp(this.pd).subscribe((next) => { });
    this.postopForm.reset(this.pd);
  }
  checkComplication(): boolean {
    let help = false;

    if (this.pd.complicatie_1 != null && this.pd.complicatie_1 != "0") { help = true; }
    if (this.pd.complicatie_2 != null && this.pd.complicatie_2 != "0") { help = true; }
    if (this.pd.complicatie_3 != null && this.pd.complicatie_3 != "0") { help = true; }
    if (this.pd.complicatie_4 != null && this.pd.complicatie_4 != "0") { help = true; }
    if (this.pd.complicatie_5 != null && this.pd.complicatie_5 != "0") { help = true; }
    if (this.pd.complicatie_6 != null && this.pd.complicatie_6 != "0") { help = true; }
    if (this.pd.complicatie_7 != null && this.pd.complicatie_7 != "0") { help = true; }
    if (this.pd.complicatie_8 != null && this.pd.complicatie_8 != "0") { help = true; }
    if (this.pd.complicatie_9 != null && this.pd.complicatie_9 != "0") { help = true; }

    return help;
  }
  canDeactivate() {

    this.savePostOp();
    this.alertify.show('saving PostOp');
    return true;
  }



}
