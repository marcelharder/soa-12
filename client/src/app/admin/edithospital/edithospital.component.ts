import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Hospital } from 'src/app/_models/Hospital';
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

  constructor(private hospitalservice: HospitalService, 
    private alertify: ToastrService, 
    private account: AccountService,
    private router: Router) { }

  ngOnInit(): void {
  }

  manageUsers(){
    
    this.account.setCurrentHospitalId(+this.pd.hospitalNo);//post the currenthospitalId to the accountservice
     // go to the employee edit page
    this.router.navigate(['/editEmployee']);
    
    
  }

  Cancel() { this.cancelThis.emit(1); }
  Save(){
  this.hospitalservice.saveHospital(this.pd).subscribe((next)=>{
    this.cancelThis.emit(1);
  })
  }
  updatePhoto(photoUrl: string) { this.pd.imageUrl = photoUrl;}

}
