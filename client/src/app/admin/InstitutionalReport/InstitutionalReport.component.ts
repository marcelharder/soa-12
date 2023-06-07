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
  additionalReportItems: dropItem[] = [];
  selectedProcedure = 0;
  text_insert:string[] = [];
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
    
   /*  this.hos.getInstitutionalReport(this.hospitalNo, 1).subscribe((next)=>{
      this.pre = next;
    }); */



    
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
    let value  = data.heading;
    if(value === 'Main text'){this.alertify.info("Hallo");
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

}
