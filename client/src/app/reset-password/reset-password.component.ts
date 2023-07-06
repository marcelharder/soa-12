
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, Validators, ValidatorFn, AbstractControl } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { resetPasswordDto } from '../_models/resetPasswordDto';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm: FormGroup;
  showSuccess: boolean;
  showError: boolean;
  errorMessage: string;
  model:resetPasswordDto = {
    password: '',
    confirmPassword: '',
    email: '',
    token: ''
  };
 
  constructor(
    private auth: AccountService,
    private route: ActivatedRoute, 
    private alertify: ToastrService) { }
  
    ngOnInit(): void {
      this.resetPasswordForm = new FormGroup({
        password: new FormControl('', [Validators.required,Validators.minLength(6), this.requiresOneDigit(), this.hasUpperCase()]),
        confirmPassword: new FormControl('',[Validators.required, this.matchValues('password')])
    });
    this.resetPasswordForm.controls.password.valueChanges.subscribe(() => { this.resetPasswordForm.controls.confirmPassword.updateValueAndValidity(); })

    this.route.queryParamMap.subscribe(params => {
      this.model.email = params.get('email');
      this.model.token = params.get('token');
    })
    
  }

  Save(){
    this.model.password = this.resetPasswordForm.value.password;    
    this.model.confirmPassword = this.resetPasswordForm.value.confirmPassword;  
      
    this.auth.resetPassword(this.model).subscribe({
    next:(_) => this.showSuccess = true,
    error: (err: HttpErrorResponse) => {
    this.showError = true;
    this.errorMessage = err.message;
  }})
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






}

