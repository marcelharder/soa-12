import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { loginModel } from 'src/app/_models/loginModel';
import { AccountService } from 'src/app/_services/account.service';
import { HospitalService } from 'src/app/_services/hospital.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {
  addForm: FormGroup |undefined;
  @Input() selectedHospital: number;
  @Input() selectedCountry: number;
  @Output() cancelThis = new EventEmitter<number>();
 
  
  model: loginModel = { username: "", password: '', KnownAs: '' }
  newUserId = 0;
  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router,
    private alertify: ToastrService) { }

  ngOnInit(): void {
    
   
    this.initializeForm(); 
  
  }

  initializeForm() {
    this.addForm = this.fb.group({
      UserName: ['', [Validators.required, Validators.email]],
      currentHospital: [this.selectedHospital.toString(), [Validators.required]],
      country: [this.selectedCountry, [Validators.required]],
      city: ['', [Validators.required]],
      knownAs: ['', [Validators.required]],
      mobile: ['', [Validators.required]],
      active: [false, [Validators.required]],
      ltk: [false, [Validators.required]],
      password: ['', [Validators.required, 
        Validators.minLength(4), 
        Validators.maxLength(8),
        this.requiresOneDigit(),
        this.hasUpperCase()]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]],
    });
    this.addForm.controls.password.valueChanges.subscribe(() => {
      this.addForm.controls.confirmPassword.updateValueAndValidity();
    })
  }

  requiresOneDigit(): ValidatorFn {
    return (control: AbstractControl) => {
    return control?.value.indexOf(control?.value.split('').filter(x => !isNaN(parseInt(x, 10)))[0]) !== -1 ? null : {containsNoDigit: true}
    }
   }
  
   hasUpperCase(): ValidatorFn{
    return (control: AbstractControl) => {
      const hasUpperCase = /[A-Z]+/.test(control!.value);
      return hasUpperCase ? null : {containsNoUpperCase: true}
      }
   }
  

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value ? null : { isMatching: true }
    }
  }

  cancel() { this.cancelThis.emit(1); }

  registerNewUser() {
   // if (this.addForm.status === "VALID") {
    this.userService.addUser(this.addForm.value).subscribe((next) => {
      this.router.navigateByUrl[('/users')];
      this.alertify.success("New user added ...");
    }, error => { this.alertify.error(error) })
  //} else {this.alertify.error("Please enter all fields")}
  }
}
