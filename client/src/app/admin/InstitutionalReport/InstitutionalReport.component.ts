import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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
  additionalReportItems: dropItem[];
  
  constructor(private hos:HospitalService) { }

  ngOnInit() {
    
    this.hos.getInstitutionalReport(this.hospitalNo, 1).subscribe((next)=>{
      this.pre = next;
    });

    
  }

  changeOperCode(soort: number){
    this.hos.getInstitutionalReport(this.hospitalNo, soort).subscribe((next)=>{
      this.pre = next;
    })
  }

  Save(){this.done.emit(1);}
  Cancel(){this.done.emit(1);}

}
