import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TabDirective } from 'ngx-bootstrap/tabs';
import { ToastrService } from 'ngx-toastr';
import { dropItem } from 'src/app/_models/dropItem';
import { additionalReportModel } from 'src/app/_models/InstitutionalReportModels/additionalReportModel';
import { mainTextModel } from 'src/app/_models/InstitutionalReportModels/mainTextModel';
import { HospitalService } from 'src/app/_services/hospital.service';
import { PreViewReportService } from 'src/app/_services/pre-view-report.service';

@Component({
  selector: 'app-InstitutionalReport',
  templateUrl: './InstitutionalReport.component.html',
  styleUrls: ['./InstitutionalReport.component.css']
})
export class InstitutionalReportComponent implements OnInit {
  @Input() hospitalNo: number;
  @Output() done = new EventEmitter<number>(); 
  pre: Partial<mainTextModel> = { };
  additional:Partial<additionalReportModel> = {};
  selectedProcedure = 0;
  text_insert:string[] = [];
  addRepToBeSaved = 0;
  procedureChoices:dropItem[] = [
    {value:0,description:"Choose"},
    {value:1,description:"CABG on pump"},
    {value:2,description:"CABG off pump"},
    {value:3,description:"AVR"},
    {value:30,description:"Minimally Invasive AVR"},
    {value:4,description:"MVR"},
    {value:41,description:"MVP"},
    {value:5,description:"AVR/MVR"},
    {value:51,description:"AVR/MVP"}];
  
  constructor(private hos:HospitalService, private alertify: ToastrService) { }

  ngOnInit() {
  // get the first additionalReport for circulatorysupport
  this.hos.getAdditionalInstitutionalReport(this.hospitalNo, 1).subscribe((next)=>{this.additional = next;})
  this.addRepToBeSaved = 1;
  }

  findnewreport(){
    this.hos.getInstitutionalReport(this.hospitalNo, this.selectedProcedure).subscribe((next)=>{
      this.pre = next;
      debugger;
      this.activateTextInserts(this.selectedProcedure.toString());
    })
  }
  

  changeOperCode(soort: number){
    this.hos.getInstitutionalReport(this.hospitalNo, soort).subscribe((next)=>{
      this.pre = next;
    })
  }

  Save(){
    // update the changed report to the api
    this.hos.updateInstitutionalReport(this.hospitalNo, this.selectedProcedure, this.pre )
    .subscribe(()=>{this.alertify.success("report changed")}, (error)=>{this.alertify.error(error)});
    this.done.emit(1);}
    
  Cancel(){this.done.emit(1);}

  onSelect(data: TabDirective): void {
    this.saveAdditionalReport(this.addRepToBeSaved);
    let value  = data.heading;
    if(value === 'Main text'){
      this.alertify.info("Main selected");
    }
    if(value === 'Circulation Support'){
      this.alertify.info("Support selected");
      this.addRepToBeSaved = 1;
      this.getAdditionalReport(1);
    }
    if(value === 'IABP'){
      this.alertify.info("IABP selected");
      this.addRepToBeSaved = 2;
      this.getAdditionalReport(2);
    }
    if(value === 'PMWires'){
      this.alertify.info("PMWires selected");
      this.addRepToBeSaved = 3;
      this.getAdditionalReport(3);
    }
  }

  activateTextInserts(soort: string){
    // clear the array first
    this.text_insert.length = 0;
    switch(soort){
      case "1": 
      this.text_insert.push("");
      this.text_insert.push("harvest-location");
      
      break;
      case "2":
      this.text_insert.push("");
      this.text_insert.push("harvest-location");
       break;
      case "3": break;
      case "4": break;
      case "41": break;
      case "5": break;
      case "51": break;
    }
  }

  saveAdditionalReport(id: number){
    if(this.additional.line_1 !== null){
    this.hos.updateAdditionalReports(this.hospitalNo, id, this.additional).subscribe(
      ()=>{}, (error)=>{this.alertify.error(error)})
  }}
  getAdditionalReport(id: number){
    this.hos.getAdditionalInstitutionalReport(this.hospitalNo, id).subscribe(
      (next)=>{this.additional = next}, (error)=>{this.alertify.error(error)})
  }

}
