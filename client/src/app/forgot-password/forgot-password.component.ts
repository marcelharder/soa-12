import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { ForgotPassword } from '../_models/ForgotPassword';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css'],
})
export class ForgotPasswordComponent implements OnInit {
  successMessage: string;
  errorMessage: string;
  showSuccess: boolean;
  showInitialComment: boolean = true;
  showError: boolean;
  resetUrl = environment.resetUrl;
  

  constructor(
    private auth: AccountService,
    private router: Router,
    private alertify: ToastrService
  ) {}

  ngOnInit(): void { }
 
  reset() {
    this.showInitialComment = false;
    var help: ForgotPassword = { email: '', clientURI: '' };
    help.email = localStorage.getItem('user-email');
    help.clientURI = this.resetUrl;
    
    this.auth.forgotPassword(help).subscribe({
      next: () => {
        this.showSuccess = true;
        this.successMessage = 'The link has been sent, please check your email to reset your password.';
        setTimeout(()=>{this.tryAgain()},1500);
      },
      error: (err) => {
        this.showError = true;
        this.errorMessage = err.message;
      },
    });
  }

  
  tryAgain(){this.router.navigate(['/']);}

  

 
  
}
