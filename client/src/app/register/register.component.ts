import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { filter, take } from 'rxjs/operators';
import { countryItem } from '../_models/countryItem';
import { dropItem } from '../_models/dropItem';
import { User } from '../_models/User';
import { AccountService } from '../_services/account.service';
import { DropdownService } from '../_services/dropdown.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup | undefined;
  model: any;
  showPage = 1;
  currentUsername = "";
  isNotInDatabase = true;
  Registermodel: Partial<User> = { country: "NL", active: true, ltk: true, gender: "Male" };
  optionCountries: Array<countryItem> = [];
  selectedCountry = "";
  optionsGender: Array<dropItem> = [];
  hospitals: Array<dropItem> = [];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private auth: AccountService,
    private alertify: ToastrService,
    
    private drops: DropdownService) { }

  ngOnInit(): void {
    this.loadDrops();
    this.selectedCountry = this.Registermodel.country;
    this.changeCountry();
    this.initializeForm();

  }

 

  initializeForm() {
    this.registerForm = this.fb.group({
      UserName: ['', [Validators.required, Validators.email]],
      country: ['', [Validators.required]],
      knownAs: ['', [Validators.required]],
      currentHospital: ['', [Validators.required]],
      city: ['', [Validators.required]],
      mobile: ['', [Validators.required]],
      active: [false, [Validators.required]],
      ltk: [false, [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6), this.requiresOneDigit(), this.hasUpperCase()]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]],
      
    });

    this.registerForm.controls.UserName.valueChanges.subscribe((next) => {
      this.currentUsername = next;
      if(this.currentUsername !== ""){
        // check if this username is in the database
        this.auth.isThisEmailInDatabase(this.currentUsername).subscribe((next)=>{
           if (next === 0) { this.isNotInDatabase = true; } else {this.isNotInDatabase = false}
        })
      }
    
    });

    this.registerForm.controls.password.valueChanges.subscribe(() => { this.registerForm.controls.confirmPassword.updateValueAndValidity(); })

  }
 
 
  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value ? null : { isMatching: true }
    }
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

 show_hidePage(){if(this.showPage === 1){return true;}}

 hideRestPage(result: number){if(result === 1 ){this.showPage = 0;} else {this.showPage = 1;}}


  loadDrops() {
    this.drops.getGenderOptions().subscribe((next) => { this.optionsGender = next; });

    const d = JSON.parse(localStorage.getItem('optionCountries'));
    if (d == null || d.length === 0) {
      this.drops.getAllCountries().subscribe((response) => {
        this.optionCountries = response;
        this.optionCountries.unshift({ Id: "", Description: "Choose",TelCode:"", IsoCode:"",Cities:"" });
        localStorage.setItem('optionCountries', JSON.stringify(response));
      });
    } else {
      this.optionCountries = JSON.parse(localStorage.getItem('optionCountries'));
    }
    
  }

  changeCountry() {
    let country = this.registerForm?.value.country;
    this.drops.getAvailableHospitals(country).subscribe(
      (next) => {
        this.hospitals = next;
        this.hospitals.unshift({value:0,description:'Choose'});
       /*  if (this.hospitals.length > 0) {
          this.registerForm.value.currentHospital = this.hospitals[0].value.toString();
        }
        else { this.alertify.show("Congratulations, you found the error"); } */
      });
  }

  registerNewUser() {
    if (this.registerForm.status === "VALID") {
      if (this.readytobeSentUp()) {
         this.auth.register(this.registerForm.value).pipe(take(1)).subscribe((next) => {
           this.alertify.show("Congratulations, you can now login with your new credentials ...");
          this.router.navigateByUrl('/');
        }, (error) => { this.alertify.error(error.description) });
      } else { this.alertify.error("Please select your country first") }
    } else { this.alertify.error("Please enter all fields") }
  }

  cancel() { this.router.navigate(['/']) }


  readytobeSentUp() {
    var help = false;
    let country = this.registerForm?.value.country;
    if (country !== '') { return true; }

    return help;

  }

  
  

}
