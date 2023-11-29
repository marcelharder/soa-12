import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { loginModel } from '../_models/loginModel';
import { AccountService } from '../_services/account.service';
import { HospitalService } from '../_services/hospital.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: loginModel = { username: '', password: '' , KnownAs: ''};
  currentRole = '';
  currentUserId = 0;
  currentRoles: Array<string> = [];

  constructor(
    public accountService: AccountService,
    private router: Router,
    private alertify: ToastrService,
    private hospitalService: HospitalService,
    private userService: UserService) { }

  ngOnInit(): void {

   /*  if (this.model.username == '') {
      this.accountService.currentUser$.pipe(take(1)).subscribe((u) => {
        this.model.username = u.UserName;
      })
    } */
    
  }



  RegisterNewClient() { this.router.navigate(['/register']); }

  login() {
    // check if the username is a valid email
    localStorage.removeItem("user");// make sure there is no residual user in browser localstorage
    localStorage.setItem("user-email", this.model.username);//save the email in localstorage in case we need to reset the password
   
    this.accountService.isThisEmailInDatabase(this.model.username).subscribe(
      (next) => {
        if (next === 1) {
          this.accountService.login(this.model).subscribe((next) => {
            this.accountService.currentUser$.pipe(take(1)).subscribe((u) => {
              this.currentUserId = u.UserId;
              this.model.username = u.UserName;
              
              this.currentRoles = u.roles;
            })
            // push the hospitalname to the behavior subject, if the loggedin person is not admin, want hospital_id of the admin  = 0
            if (!this.currentRoles.includes('Admin')) {
              this.userService.getUser(this.currentUserId).subscribe((next) => {
                this.model.KnownAs = next.knownAs;
                this.hospitalService.getSpecificHospital(next.hospital_id).subscribe((d) => {
                  this.accountService.changeCurrentHospital(d.HospitalName); // save the name of this hospital
                });
              })

            }
            else {this.model.KnownAs = "Admin";}
            this.router.navigate(['/procedures']);
          }
          )
        } else {
          this.alertify.show("You are not subscribed, please register");
        }
      }
    )


  }

  logout() {
    this.model.username = "";
    this.model.password = "";
    this.accountService.logout();
    this.router.navigate([''])
  }



}

function IsValidEmail(arg0: string) {
  let emailRegex = /^(([^<>()[]\.,;:\s@"]+(.[^<>()[]\.,;:\s@"]+)*)|(".+"))@(([[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}])|(([a-zA-Z-0-9]+.)+[a-zA-Z]{2,}))$/;
  return emailRegex.test(arg0);
}


