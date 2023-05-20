import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { dropItem } from 'src/app/_models/dropItem';
import { Hospital } from 'src/app/_models/Hospital';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { HospitalService } from 'src/app/_services/hospital.service';

@Component({
  selector: 'app-addhospital',
  templateUrl: './addhospital.component.html',
  styleUrls: ['./addhospital.component.css']
})
export class AddhospitalComponent implements OnInit {
  @Output() pushHospital = new EventEmitter<Partial<Hospital>>();
  @Output() cancelThis = new EventEmitter<number>();
  @Input() selectedCountry: string;

  selectedHospital='';
  listOfHospitals:Array<dropItem> = [];

  pd: Partial<Hospital> = {};

  

  constructor(private hospitalService: HospitalService, 
    private alertify: ToastrService,
    private drop: DropdownService) { }

  ngOnInit(): void {
    // get the available hospitals from the server with selectedCountry as parameter
  this.drop.getAvailableHospitals(this.selectedCountry).subscribe((next)=>{
    this.listOfHospitals = next;
  })
  }

 
  

  Select(){
    
    this.hospitalService.getSpecificHospitalFromInventory(+this.selectedHospital).subscribe((next)=>{
      this.pd.hospitalNo = next.hospitalNo;
      this.pd.hospitalName = next.hospitalName;
      this.pd.address = next.address;
      this.pd.country = next.country;
      this.pd.telephone = next.telephone;
      //this.pd.imageUrl = next.imageUrl;
      this.pushHospital.emit(this.pd);
    })
  }

  

  


}
