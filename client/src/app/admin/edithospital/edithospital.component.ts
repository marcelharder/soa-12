import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Hospital } from 'src/app/_models/Hospital';
import { previewReport } from 'src/app/_models/previewReport';
import { AccountService } from 'src/app/_services/account.service';
import { HospitalService } from 'src/app/_services/hospital.service';

@Component({
  selector: 'app-edithospital',
  templateUrl: './edithospital.component.html',
  styleUrls: ['./edithospital.component.css']
})
export class EdithospitalComponent implements OnInit {
  @Input() pd?: Hospital;
  @Output() cancelThis = new EventEmitter<number>();

  showInstitutionalReport = false;
  pre: previewReport;
 

  constructor(private hospitalservice: HospitalService, 
    private alertify: ToastrService, 
    private account: AccountService,
    private router: Router) { }

  ngOnInit(): void { }

  manageUsers(){
    
    this.account.setCurrentHospitalId(+this.pd.hospitalNo);//post the currenthospitalId to the accountservice
     // go to the employee edit page
    this.router.navigate(['/editEmployee']);
    
    
  }

  displayIR(){if(this.showInstitutionalReport){return true;}}
  backToEdit(){this.showInstitutionalReport = false;}

  Cancel() { this.cancelThis.emit(1); }
  Save(){
  this.hospitalservice.saveHospital(this.pd).subscribe((next)=>{
    this.cancelThis.emit(1);
  })
  }
  SaveInstitutionaOperativeReport(){

    this.hospitalservice.saveHospital(this.pd).subscribe((next)=>{
      // save the institutionalReport
      this.hospitalservice.updateIOReport(this.pre).subscribe((next)=>{
        this.cancelThis.emit(1);
      })
      
    })
  }
  updatePhoto(photoUrl: string) { this.pd.imageUrl = photoUrl;}

  editInstitutionalReport(){
    this.showInstitutionalReport = true;
    this.hospitalservice.saveIOReport(this.pd.hospitalNo).subscribe((next)=>{
      this.hospitalservice.getIOReport(this.pd.hospitalNo).subscribe((rep)=>{
        this.pre = rep;
      })
    })

    
  }
  
  

}

  
