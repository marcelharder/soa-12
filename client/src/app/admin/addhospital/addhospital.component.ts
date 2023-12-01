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
  @Input() alreadySelected: Array<Hospital>;

  selectedHospital = '';
  listOfHospitals: Array<dropItem> = [];
  pd: Partial<Hospital> = {};


  constructor(private hospitalService: HospitalService,
    private alertify: ToastrService,
    private drop: DropdownService) { }

  ngOnInit(): void {
    // get the available hospitals from the server with selectedCountry as parameter
    this.drop.getAvailableHospitals(this.selectedCountry).subscribe((next) => {
      this.listOfHospitals = next;
      // now substract all the hospitals that are in the already selected list
      for (let i of this.listOfHospitals) {
        for (let h of this.alreadySelected) {
          if (h.HospitalNo == i.value.toString()) {
            // this means that the hospital is already in my list
            // so remove this hospitalnumber from the list of hospitals array
            this.listOfHospitals = this.listOfHospitals.filter(item => item.value !== i.value);
            if (this.listOfHospitals.length === 0) { this.alertify.error("No additional hospitals found in this country") }
          }
        }
      }
    })
  }

  Select() {
    this.hospitalService.getSpecificHospitalFromInventory(+this.selectedHospital).subscribe((next) => {
      this.pd.HospitalNo = next.HospitalNo;
      this.pd.HospitalName = next.HospitalName;
      this.pd.Address = next.Address;
      this.pd.Country = next.Country;
      this.pd.Telephone = next.Telephone;
      //this.pd.imageUrl = next.imageUrl;
      this.pushHospital.emit(this.pd);
    })
  }
}
