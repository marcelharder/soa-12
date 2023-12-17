import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { countryItem } from 'src/app/_models/countryItem';
import { dropItem } from 'src/app/_models/dropItem';
import { Hospital } from 'src/app/_models/Hospital';
import { hospitalValve } from 'src/app/_models/hospitalValve';
import { OVIvalve } from 'src/app/_models/OVIvalve';
import { AccountService } from 'src/app/_services/account.service';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { HospitalService } from 'src/app/_services/hospital.service';
import { ValveService } from 'src/app/_services/valve.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-hospitals',
  templateUrl: './hospitals.component.html',
  styleUrls: ['./hospitals.component.css']
})
export class HospitalsComponent implements OnInit {
  @ViewChild("hospitalForm") hospitalForm: NgForm;
  pd: Hospital;
  targetUrl = "";
  baseUrl = environment.apiUrl;
  hv: hospitalValve = {
    ValveTypeId: 0,
    Description: "",
    Implant_position: "Aortic",
    Type: "",
    hospitalId: "0",
    Vendor_code: 0,
    Vendor_description: "",
    Valve_size: null,
    No: 0,
    Model_code: '',
    uk_code: '',
    soort: 1,
    image: '',
    countries: ''
  };
  

  listCities: Array<dropItem> = [];
  listCountries: Array<countryItem> = [];
  valveTypes: Array<dropItem> = [];
  valvePositions: Array<dropItem> = [];
  valveOVITypes: Array<dropItem> = [];
  valveOVIPositions: Array<dropItem> = [];
  onlineValves: Array<dropItem> = [];
  hospitalValves: Array<hospitalValve> = [];
  OVIValves: Array<OVIvalve> = [];

  TitleDetailsForm = "Details";
  currentHospital = "";
  selectedOnlineValve = "";
  searchPosition = "";
  searchType = "";
  showOVITab = false;

  don = 0;
  displayList = 1;
  displayHospitalImage = 1;
  addbutton = 0;
  updatebutton = 0;
  savebutton = 0;

  constructor(
    private route: ActivatedRoute,
    private drops: DropdownService,
    private auth: AccountService,
    private router: Router,
    private vs: ValveService,
    private hospitalService: HospitalService,
    private alertify: ToastrService
  ) { }

  ngOnInit(): void {
    this.loadDrops();
    this.auth.currentHospitalName.subscribe((next) => {
      this.currentHospital = next;
    });
    this.route.data.subscribe((data) => {
      this.pd = data.hos;
      this.hospitalService.IsThisHospitalUsingOVI(+this.pd.HospitalNo).subscribe((next) => {
        if (next) { this.showOVITab = true } else { this.showOVITab = false }
      })
      //this.getCorrectCities();
    });
    // find out if this hospital uses the online valve inventory, if so then show the ovi tab




  }

  getCorrectCities() {
    this.hospitalService.getListOfCities(this.pd.Country).subscribe((next) => {
      this.listCities = next;
    });
  }



  SearchValve() {
    this.addbutton = 1;
    this.vs
      .getHospitalValves(this.hv.Type, this.hv.Implant_position)
      .subscribe((next) => {
        this.hospitalValves = next;
      });
    this.alertify.show("Searching");
  }


  showList() { if (this.displayList == 1) { return true; } }
  showAddButton() { if (this.addbutton == 1) { return true; } }
  showUpdateButton() { if (this.updatebutton == 1) { return true; } }
  showSaveButton() { if (this.savebutton == 1) { return true; } }
  showHospitalImage() { if (this.displayHospitalImage == 1) { return true; } }
  doneWithOvi() {
    this.router.navigate(['/config']);
    this.alertify.show("Done with this");
  }

  AddValve() {
    this.displayList = 0;
    this.updatebutton = 0;
    this.savebutton = 1;
    this.don = 1;
    this.SearchHospitalValve();
  }

  SearchHospitalValve() {
    this.onlineValves = [];
    // go out to online valve app and find the valveTypes[]
    this.vs.searchHospitalValveOnline(this.hv.Type, this.hv.Implant_position).subscribe((next) => { this.onlineValves = next; });
    this.alertify.show("find product now ...");
  }
  deleteDetails(code: string) {

    this.alertify.show("Needs to be implemented");



    /* this.vs.removeSpecificValveType(+code).subscribe(
      (next) => {
        this.alertify.show("Valve deleted ...");
      },
      (error) => {
        this.alertify.error(error);
      }, () => {
        this.SearchValve();
        this.displayList = 1;

      }
    ); */




    this.alertify.show("delete now ..." + code);
  }
  changeToHospitalValve() { // write the hospitalId to the valvetype
    this.alertify.info("hello");
    this.vs.getSpecificValveType(+this.selectedOnlineValve).subscribe((next) => {
      this.alertify.show("Valve added ...");
    }, (error) => {
      this.alertify.error(error);
    }, () => {
      this.SearchValve();
      this.displayList = 1;
      this.don = 0;
    })
  }

  findValveInOVI() {
    this.alertify.show("Getting the valves in the online valve inventory");

    let help: Partial<hospitalValve> = {};
    help.Type = this.searchType;
    help.hospitalId = this.pd.HospitalNo;
    help.Implant_position = this.searchPosition;

    this.vs.getValvesFromOVI(help).subscribe((next) => {
      this.OVIValves = next;
      this.OVIValves.sort((x, y) => x.size > y.size ? 1 : x.size < y.size ? -1 : 0)
    })

  }


  displayOnlineValves() { if (this.don === 1) { return true; } }

  loadDrops() {
    this.valveTypes.push({ value: 0, description: "Biological" });
    this.valveTypes.push({ value: 1, description: "Mechanical" });
    this.valveTypes.push({ value: 2, description: "Annuloplasty_Ring" });
    this.valveTypes.push({ value: 3, description: "Valved_Conduit" });
    this.valveTypes.push({ value: 4, description: "Homograft" });

    this.valvePositions.push({ value: 0, description: "Aortic" });
    this.valvePositions.push({ value: 1, description: "Mitral" });
    this.valvePositions.push({ value: 2, description: "Tricuspid" });

    this.valveOVITypes.push({ value: 2, description: "Biological" });
    this.valveOVITypes.push({ value: 1, description: "Mechanical" });
    this.valveOVITypes.push({ value: 4, description: "Annuloplasty_Ring" });
    this.valveOVITypes.push({ value: 3, description: "Valved_Conduit" });
    this.valveOVITypes.push({ value: 5, description: "Homograft" });

    this.valveOVIPositions.push({ value: 1, description: "Aortic" });
    this.valveOVIPositions.push({ value: 2, description: "Mitral" });
    this.valveOVIPositions.push({ value: 4, description: "Tricuspid" });

    const d = JSON.parse(localStorage.getItem("optionCountries"));
    if (d == null || d.length === 0) {
      this.drops.getAllCountries().subscribe((response) => {
        this.listCountries = response;
        localStorage.setItem("optionCountries", JSON.stringify(response));
      });
    } else {
      this.listCountries = JSON.parse(localStorage.getItem("optionCountries"));
    }
  }
  updatePhoto(photoUrl: string) { this.pd.ImageUrl = photoUrl; }

  IsLoaded() {
    if (+this.pd.HospitalNo !== 0) {
      this.targetUrl = this.baseUrl + 'hospital/addHospitalPhoto/' + this.pd.HospitalNo;
      return true;
    } else { return false; }
  }
  saveHospital() {
    this.hospitalService.saveHospital(this.pd).subscribe(() => {
      this.router.navigate(["/config"]);
    });
  }
  cancel() { this.router.navigate(["/config"]); }

  AddOnlineValve() {
    // hide the hospital image and show the dataentry form for the new hospitalValve
    this.displayHospitalImage = 0;

    this.alertify.info("Adding new ValveType now");
  }

  receiveAddValveType(result: number) {
    
    if(result == 10)
    {
      this.displayHospitalImage = 1;
    }
    if (result == 1)
     {
      this.displayHospitalImage = 1;
      this.alertify.info("Valve added ...");
    //  this.vs.createSpecificHospitalValve(this.hv).subscribe(() => { }, (error) => { })
    }

  }

  canDeactivate() {
    this.saveHospital();
    this.alertify.show("saving Hospital");
    return true;
  }
}
