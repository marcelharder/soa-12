import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { hospitalValve } from 'src/app/_models/hospitalValve';

@Component({
  selector: 'app-add-valveType',
  templateUrl: './add-valveType.component.html',
  styleUrls: ['./add-valveType.component.css']
})
export class AddValveTypeComponent implements OnInit {
  @Input() hv: hospitalValve;
  @Output() sendupdate = new EventEmitter();
  addValveTypeForm: FormGroup | undefined;


  constructor( 
    private fb: FormBuilder,
    private alertify: ToastrService) { }

  ngOnInit() {this.initializeForm }

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
