import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ForgotPassword } from '../_models/ForgotPassword';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  forgotPasswordForm: FormGroup;
  successMessage: string;
  errorMessage: string;
  showSuccess: boolean;
  showError: boolean;
  resetUrl = environment.resetUrl;
  quest = 0;

  constructor(private auth: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.forgotPasswordForm = new FormGroup({
      email: new FormControl('',[Validators.required])
    })
  }

  public validateControl = (controlName: string) => {
    return this.forgotPasswordForm.get(controlName).invalid && this.forgotPasswordForm.get(controlName).touched
  }

  tryAgain(){
    this.router.navigate(['/']);
    this.quest = 0;
  }
  reset(){this.quest = 1;}

  showQuestion(){if(this.quest === 0){return true}}

  cancel(){this.router.navigate(['/'])};

  public hasError = (controlName: string, errorName: string) => {
    return this.forgotPasswordForm.get(controlName).hasError(errorName)
  }

    
  public forgotPassword = (forgotPasswordFormValue) => {
    this.showError = this.showSuccess = false;
    const forgotPassDto: ForgotPassword = {
      email: forgotPasswordFormValue.email,
      clientURI: this.resetUrl
    };
    this.auth.isThisEmailInDatabase(forgotPassDto.email).subscribe({
      next: (isAvailable) => {
        if (isAvailable) {
          this.auth.forgotPassword(forgotPassDto).subscribe({
            next: () => {
              this.showSuccess = true;
              this.successMessage = 'The link has been sent, please check your email to reset your password.';
            },
            error: (err) => {
              this.showError = true;
              this.errorMessage = err.message;
            }
          });
        } else {
          this.showError = true;
          this.errorMessage = 'This email does not exist in our database.';
        }
      },
      error: (err) => {
        this.showError = true;
        this.errorMessage = err.message;
      }
    });
  }
  



}