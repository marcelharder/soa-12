import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { dropItem } from 'src/app/_models/dropItem';
import { hospitalValve } from 'src/app/_models/hospitalValve';
import { ValveService } from 'src/app/_services/valve.service';

@Component({
  selector: 'app-add-valveType',
  templateUrl: './add-valveType.component.html',
  styleUrls: ['./add-valveType.component.css']
})
export class AddValveTypeComponent implements OnInit {
  addValveTypeForm: FormGroup | undefined;
  optionsVendors: Array<dropItem> = [];
  @Input() hv: hospitalValve;
  @Output() sendupdate = new EventEmitter();



  constructor(
    private fb: FormBuilder,
    private vs: ValveService,
    private alertify: ToastrService) { }

  ngOnInit() {

    this.addValveTypeForm = new FormGroup({
      hospitalId: new FormControl(),
      No: new FormControl(),
      Vendor_description: new FormControl(),
      Vendor_code: new FormControl(),
      Valve_size: new FormControl(),
      Model_code: new FormControl(),
      Implant_position: new FormControl(),
      uk_code: new FormControl(),
      us_code: new FormControl(),
      image: new FormControl(),
      Description: new FormControl(),
      Type: new FormControl(),
      countries: new FormControl()
    })
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
    // need to adjust the endpoint on the inventory container so that it will be anonymous
    // this.vs.getVendors().subscribe((next) => { this.optionsVendors = next; });
    this.initializeForm;

  }

  cancel() { this.sendupdate.emit(10); }
  SaveNewValveType() { this.sendupdate.emit(1); }

  initializeForm() {
    this.addValveTypeForm = this.fb.group({
      hospitalId: ['', []],
      No: ['', []],
      Vendor_description: ['', []],
      Vendor_code: ['', []],
      Valve_size: ['', []],
      Model_code: ['', []],
      Implant_position: ['', []],
      uk_code: ['', []],
      us_code: ['', []],
      image: ['', []],
      Description: ['', []],
      Type: ['', []],
      countries: ['', []]
    });
  }

  hideRestPage(h: any) { }
}
