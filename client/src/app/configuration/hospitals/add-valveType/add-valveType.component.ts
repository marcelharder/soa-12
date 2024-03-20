import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { AbstractControl, Form, FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { dropItem } from 'src/app/_models/dropItem';
import { hospitalValve } from 'src/app/_models/hospitalValve';
import { AccountService } from 'src/app/_services/account.service';
import { UserService } from 'src/app/_services/user.service';
import { ValveService } from 'src/app/_services/valve.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-add-valveType',
  templateUrl: './add-valveType.component.html',
  styleUrls: ['./add-valveType.component.css']
})
export class AddValveTypeComponent implements OnInit {
  @ViewChild("addValveForm") addValveForm: NgForm;
  optionsVendors: Array<dropItem> = [];
  optionsType: Array<dropItem> = [];
  optionsPosition: Array<dropItem> = [];

  @Input() hv: hospitalValve;
  @Output() newHospitalValve: EventEmitter<hospitalValve> = new EventEmitter();
  @Output() ct: EventEmitter<number> = new EventEmitter();
  selectedVendor = 2;
  targetUrl = "";
  baseUrl = environment.apiUrl;



  constructor(
    private vs: ValveService,
    private user: UserService,
    private auth: AccountService,
    private alertify: ToastrService) { }

  ngOnInit() {
    this.loadDrops();


    this.optionsVendors.sort(function (a, b) { return ('' + a.description).localeCompare(b.description); })
    // need to adjust the endpoint on the inventory container so that it will be anonymous
    // this.vs.getVendors().subscribe((next) => { this.optionsVendors = next; });


  }

  loadDrops() {
    this.optionsVendors =
      [
        {
          "value": 9,
          "description": "CryoLife"
        },
        {
          "value": 7,
          "description": "Atrium"
        },
        {
          "value": 3,
          "description": "LivaNova"
        },
        {
          "value": 8,
          "description": "Gore"
        },
        {
          "value": 2,
          "description": "Abbott"
        },
        {
          "value": 6,
          "description": "KFSRC"
        },
        {
          "value": 5,
          "description": "Medtronic"
        },
        {
          "value": 4,
          "description": "Edwards"
        }
      ];


  }



  updatePhoto(url: string) { this.hv.image = url; }

  cancel() { this.ct.emit(1); }

  IsLoaded() {
    if (+this.hv.ValveTypeId !== 0) {
      this.targetUrl = this.baseUrl + 'Valve/addValveTypePhoto/' + this.hv.ValveTypeId;
      return true;
    } else { return false; }
  }

  SaveNewValveType() {
    // get vendor description from vendor_code
    if (this.selectedVendor !== 0) {
      var hep = this.optionsVendors.find(x => x.value == this.selectedVendor);
      this.hv.Vendor_description = hep.description;
      this.hv.Vendor_code = hep.value;
      this.hv.Implant_position = "Aortic";
      this.hv.Type = "Biological";
      this.hv.countries = this.getCountry();

      debugger;
      this.newHospitalValve.emit(this.hv);
    }


  }

  getCountry(): string {
    var ret = "";
    var userId = 0;

    this.auth.currentUser$.subscribe((next) => { userId = next.UserId; }
    , ()=> {}
    , ()=>{
      this.user.getUser(userId).subscribe((response) => 
      {     
        ret = response.country; 
      }
      );
    })

   
    
    
    
      return ret;
  }




}
