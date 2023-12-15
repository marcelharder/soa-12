import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
  optionsVendors: Array<dropItem> = [];
  @Input() hv: hospitalValve;
  @Output() sendupdate = new EventEmitter();
  addValveTypeForm: FormGroup | undefined;


  constructor( 
    private fb: FormBuilder,
    private vs: ValveService,
    private alertify: ToastrService) { }

  ngOnInit() {
    
    this.vs.getVendors().subscribe((next) => { this.optionsVendors = next; });
    this.initializeForm;
  
  }

  cancel() { this.sendupdate.emit(10); }
  SaveNewValveType() { this.sendupdate.emit(1); }

  initializeForm() {
    this.addValveTypeForm = this.fb.group({
      UserName: ['', [Validators.required, Validators.email]],
      country: ['US', [Validators.required]],
      knownAs: ['', [Validators.required]],
      currentHospital: ['', [Validators.required]],
      city: ['', [Validators.required]],
      mobile: ['', [Validators.required]],
      active: [false, [Validators.required]],
      ltk: [false, [Validators.required]],
      
    });
  }
}
