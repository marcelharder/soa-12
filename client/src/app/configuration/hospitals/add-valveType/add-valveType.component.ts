import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { AbstractControl, Form, FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { dropItem } from 'src/app/_models/dropItem';
import { hospitalValve } from 'src/app/_models/hospitalValve';
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
  @Input() hv: hospitalValve;
  @Output() sendupdate = new EventEmitter();
  selectedVendor = 2;
  targetUrl = "";
  baseUrl = environment.apiUrl;



  constructor(
    private vs: ValveService,
    private alertify: ToastrService) { }

  ngOnInit() {

    
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

    
   // this.optionsVendors.sort((a, b) => a.value - b.value);
    this.optionsVendors.sort(function (a, b) {
      return ('' + a.description).localeCompare(b.description);
  })
    // need to adjust the endpoint on the inventory container so that it will be anonymous
    // this.vs.getVendors().subscribe((next) => { this.optionsVendors = next; });
    

  }

 

  updatePhoto(url: string){this.hv.image = url;}

  cancel() { this.sendupdate.emit(10); }

   IsLoaded() {
    if (+this.hv.hospitalId !== 0) {
      this.targetUrl = this.baseUrl + 'Valve/addValveTypePhoto/' + this.hv.hospitalId;
      return true;
    } else { return false; }
  } 

  SaveNewValveType() {
    // get vendor description from vendor_code
    if (this.selectedVendor !== 0) {
      var hep = this.optionsVendors.find(x => x.value == this.selectedVendor);
      this.hv.Vendor_description = hep.description;
      this.hv.Vendor_code = hep.value;
      this.sendupdate.emit(1);
    }


  }

  

  

  
}
