import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { countryItem } from 'src/app/_models/countryItem';
import { dropItem } from 'src/app/_models/dropItem';
import { Hospital } from 'src/app/_models/Hospital';
import { hospitalValve } from 'src/app/_models/hospitalValve';
import { OVIvalve } from 'src/app/_models/OVIvalve';
import { valveType } from 'src/app/_models/valveType';
import { AccountService } from 'src/app/_services/account.service';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { HospitalService } from 'src/app/_services/hospital.service';
import { ValveService } from 'src/app/_services/valve.service';

@Component({
  selector: 'app-hospitals',
  templateUrl: './hospitals.component.html',
  styleUrls: ['./hospitals.component.css']
})
export class HospitalsComponent implements OnInit  {
  @ViewChild("hospitalForm") hospitalForm: NgForm;
  pd: Hospital;
  hv: hospitalValve = {
    codeId: 0,
    code: "",
    valveTypeId: 0,
    description: "",
    implant_Position: "Aortic",
    soort: 1,
    type: "",
    hospitalNo: 0
  };
  vt: valveType = {
    no: 0,
    valveTypeId:0,
    vendor_description: "",
    vendor_code: "",
    model_code: "",
    implant_position: "",
    uk_code: "",
    us_code: "",
    valve_size:[],
    image: "",
    description: "",
    type: "",
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
  searchType = "";
  searchPosition = "";
  showOVITab = false;

  don = 0;
  displayList = 1;
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
  ) {}

  ngOnInit(): void {
    this.loadDrops();
    this.auth.currentHospitalName.subscribe((next) => {
      this.currentHospital = next;
    });
    this.route.data.subscribe((data) => {
      this.pd = data.hos;
      this.hospitalService.IsThisHospitalUsingOVI(+this.pd.hospitalNo).subscribe((next)=>{
        if(next){this.showOVITab = true} else {this.showOVITab = false}
      })
      //this.getCorrectCities();
    });
    // find out if this hospital uses the online valve inventory, if so then show the ovi tab




  }

  getCorrectCities() {
    this.hospitalService.getListOfCities(this.pd.country).subscribe((next) => {
      this.listCities = next;
    });
  }



  SearchValve() {
    this.addbutton = 1;
    this.vs
      .getHospitalValves(this.hv.type, this.hv.implant_Position)
      .subscribe((next) => {
        this.hospitalValves = next;
      });
    this.alertify.show("Searching");
  }


  showList() { if (this.displayList == 1) { return true; } }
  showAddButton() { if (this.addbutton == 1) {  return true;  }  }
  showUpdateButton() { if (this.updatebutton == 1) { return true; }  }
  showSaveButton() {if (this.savebutton == 1) { return true; }  }
  doneWithOvi(){
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
    this.vs.searchHospitalValveOnline(this.hv.type, this.hv.implant_Position).subscribe((next) => { this.onlineValves = next; });
    this.alertify.show("find product now ...");
  }
  deleteDetails(code:string){
     this.vs.deleteSpecificHospitalValve(+code).subscribe(
      (next) => {
        this.alertify.show("Valve deleted ...");
      },
      (error) => {
        this.alertify.error(error);
      },() => {
        this.SearchValve();
        this.displayList = 1;

      }
    );




    this.alertify.show("delete now ..." + code);
  }
  changeToHospitalValve() {
    // get the valveTypeId and find the details online
    this.vs
      .getSpecificValveType(+this.selectedOnlineValve)
      .subscribe((next) => {
        this.vt = next;
        // then copy some of these details to the hospital valve
        this.hv.code = this.vt.uk_code;
        this.hv.valveTypeId = this.vt.valveTypeId;
        this.hv.description = this.vt.description;
        this.hv.implant_Position = this.vt.implant_position;
        this.hv.type = this.vt.type;
        // upload this valve to the current hospital
        this.vs.createSpecificHospitalValve(this.hv).subscribe(
          (next) => {
            this.alertify.show("Valve added ...");
          },
          (error) => {
            this.alertify.error(error);
          },() => {
            this.SearchValve();
            this.displayList = 1;
            this.don = 0;
          }
        );
        // get the valves from the hospital


      });
  }

  findValveInOVI(){
    this.alertify.show("Getting the valves in the online valve inventory");

    let help: Partial<hospitalValve> = {};
   help.hospitalNo = +this.pd.hospitalNo;
    help.soort = +this.searchType;
    help.implant_Position = this.searchPosition;

    this.vs.getValvesFromOVI(help).subscribe((next) => {
      this.OVIValves = next;
      this.OVIValves.sort((x, y) => x.size > y.size? 1 : x.size < y.size ? -1 : 0)
   })
  
  }
  

  displayOnlineValves() { if (this.don === 1) { return true; }}

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
  updatePhoto(photoUrl: string) { this.pd.imageUrl = photoUrl;}
  saveHospital() {
    this.hospitalService.saveHospital(this.pd).subscribe(() => {
      this.router.navigate(["/config"]);
    });
  }
  cancel() {this.router.navigate(["/config"]);}

  canDeactivate() {
    this.saveHospital();
    this.alertify.show("saving Hospital");
    return true;
  }
}
