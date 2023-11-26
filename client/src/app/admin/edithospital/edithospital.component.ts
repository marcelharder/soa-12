import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Hospital } from 'src/app/_models/Hospital';
import { previewReport } from 'src/app/_models/previewReport';
import { AccountService } from 'src/app/_services/account.service';
import { HospitalService } from 'src/app/_services/hospital.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-edithospital',
  templateUrl: './edithospital.component.html',
  styleUrls: ['./edithospital.component.css']
})
export class EdithospitalComponent implements OnInit {
  @Input() pd: Hospital;
  @Output() cancelThis = new EventEmitter<number>();
  targetUrl="";
  baseUrl = environment.apiUrl;

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
  editInstitutionalReport(){this.showInstitutionalReport = true;}

  Cancel() { this.cancelThis.emit(1); }
  Save(){this.hospitalservice.saveHospital(this.pd).subscribe((next)=>{this.cancelThis.emit(1); })}
  
   IsLoaded() {
    if (+this.pd.hospitalNo !== 0) {
        this.targetUrl = this.baseUrl + 'hospital/addHospitalPhoto/' + this.pd.hospitalNo;
        return true;
    } else { return false; }
} 
  updatePhoto(photoUrl: string) { this.pd.imageUrl = photoUrl;}

  receiveDone(no: number){
    this.cancelThis.emit(1);
  }

 
  
  

}

  
