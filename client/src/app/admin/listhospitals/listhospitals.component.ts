import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { countryItem } from 'src/app/_models/countryItem';
import { Hospital } from 'src/app/_models/Hospital';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { HospitalService } from 'src/app/_services/hospital.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-listhospitals',
  templateUrl: './listhospitals.component.html',
  styleUrls: ['./listhospitals.component.css']
})
export class ListhospitalsComponent implements OnInit {

  hospitals: Array<Partial<Hospital>> = [];
  modalRef: BsModalRef;
  country: countryItem;
  selectedHospital: Hospital;
  selectedCountry = "NL";
  optionCountries: Array<countryItem> = [];
  editFlag = 0;
  addFlag = 0;
  listFlag = 1;
  model: countryItem = {  Id: "", Description: "Choose",TelCode:"", IsoCode:"",Cities:"" };

  constructor(
    private modalService: BsModalService,
    private hospitalService: HospitalService,
    private drops: DropdownService,
    private router: Router,
    private alertify: ToastrService) { }

  ngOnInit(): void {
    this.loadDrops();
    this.filterCountry();
  }

  
  confirm(): void {
    localStorage.setItem("optionCountries",null);
    
    this.model.TelCode = "123"; // save the newly entered country to the xml
    this.hospitalService.addCountry(this.model).subscribe((next) => 
    {
      this.alertify.info("saving country");
      this.loadDrops();// get the updated country list from the api
    },
      error => { this.alertify.error(error); }
    );
    this.modalRef?.hide();
  }
  close(): void { this.modalRef?.hide(); }

  loadDrops() {
    const d = JSON.parse(localStorage.getItem('optionCountries'));
    if (d == null || d.length === 0) {
      this.drops.getAllCountries().subscribe((response) => {
        this.optionCountries = response;
        this.optionCountries.unshift({ Id: "", Description: "Choose",TelCode:"",IsoCode:"", Cities:"" });
        localStorage.setItem('optionCountries', JSON.stringify(response));
      });
    } else {
      this.optionCountries = JSON.parse(localStorage.getItem('optionCountries'));
    }
    /* this.listOfCountries.push({value:"NL",description:"Netherlands"});
    this.listOfCountries.push({value:"US",description:"US"});
    this.listOfCountries.push({value:"GB",description:"UK"});
    this.listOfCountries.push({value:"FR",description:"France"});
    this.listOfCountries.push({value:"DE",description:"Germany"});
    this.listOfCountries.push({value:"IT",description:"Italy"});
    this.listOfCountries.push({value:"SA",description:"KSA"}); */
  }

  filterCountry() {
     this.hospitalService.getHospitalsInCountry(this.selectedCountry).subscribe((next) => { this.hospitals = next; })
  }

  showAdd() { if (this.addFlag === 1) { return true; } }
  showEdit() { if (this.editFlag === 1) { return true; } }
  showList() { if (this.listFlag === 1) { return true; } }

  Cancel() { this.router.navigate(['/']) }

  addHospital() {
    this.addFlag = 1;
    this.editFlag = 0;
    this.listFlag = 0;

  }
  editHospital(ret: string) {

    this.hospitalService.getSpecificHospital(+ret).subscribe((next) => {
      this.selectedHospital = next
    },
      error => { this.alertify.error(error) });
    this.addFlag = 0;
    this.listFlag = 0;
    this.editFlag = 1;
  }
  deleteHospital(ret: string) {
    this.addFlag = 0;
    this.editFlag = 0;
    this.listFlag = 1;
    this.hospitalService.deleteHospital(ret).subscribe((next) => { this.alertify.show("Hospital removed ...") });
    this.filterCountry();
  }

  backFromAdd(ret: any) {
    this.addFlag = 0;
    this.listFlag = 1;
    this.editFlag = 0;
  }

  backFromEdit(ret: any) {
    this.addFlag = 0;
    this.listFlag = 1;
    this.editFlag = 0;
  }

  receiveSelectedCountry(ret: string) {
    this.selectedCountry = ret;
    this.filterCountry();
    this.addFlag = 0;
    this.listFlag = 1;
    this.editFlag = 0;
  }
  receiveHospital(hos: Hospital) {
    this.selectedHospital = hos;
    // push this new hospital to the api

    // don't forget to load the institutional report

    this.addFlag = 0;
    this.listFlag = 0;
    this.editFlag = 1;
  }

}
