import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AbstractControl, FormBuilder, FormControl, FormGroup, NgForm, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from '../../_models/User';
import { UserService } from '../../_services/user.service';
import { environment } from '../../../environments/environment';
import { DropdownService } from '../../_services/dropdown.service';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/_services/account.service';
import { countryItem } from 'src/app/_models/countryItem';
import { take } from 'rxjs/operators';
import { changePassword } from 'src/app/_models/changePassword';


@Component({
    templateUrl: './userprofile.component.html',
    styleUrls: ['./userprofile.component.css']
})

export class UserProfileComponent implements OnInit {
    @ViewChild('editForm') editForm: NgForm;
    targetUrl: string;
    changePasswordForm: FormGroup;
    cpwd:changePassword = {
        password: '',
        confirmPassword: '',
        currentPassword: '',
        email: ''
    };
    user: User;
    model: any = {};
    currentUserId = 0;
    currentUserName = '';
    baseUrl = environment.apiUrl;
    countryDescription = '';
    optionCountries: Array<countryItem> = [];
    countryWhereUserLives = '';
    password_01 = '';
    password_02 = '';
    password_03 = '';
    premium = 0;
   

    @HostListener('window:beforeunload', ['$event'])
    unloadNotification($event: any) { if (this.editForm.dirty) { $event.returnValue = true; } }

    constructor(private route: ActivatedRoute,
        private router: Router,private fb: FormBuilder,
        private drops: DropdownService,
        private alertify: ToastrService,
        private userService: UserService,
        private auth: AccountService) { }

    ngOnInit() {
        this.initializeChangePwdForm();
        this.loadDrops();

        this.auth.currentServiceLevel$.pipe(take(1)).subscribe((n) => {this.premium = n})

        this.auth.currentUser$.pipe(take(1)).subscribe((u) => {
            this.currentUserName = u.UserName;
            this.currentUserId = u.UserId;
        });

        this.route.data.subscribe((data: { user: User }) => {
            this.user = data.user;
            // focus on the correct drops
            this.changeCountry();// let the country name follow the change in country
        });
       
       

    }

    initializeChangePwdForm(){
        this.changePasswordForm = this.fb.group({
            currentPassword: ['', [Validators.required]],
            password: ['', [Validators.required,Validators.minLength(4),Validators.maxLength(8),this.requiresOneDigit(),this.hasUpperCase()]],
            confirmPassword: ['', [Validators.required, this.matchValues('password')]],
          });
          this.changePasswordForm.controls.password.valueChanges.subscribe(() => {
            this.changePasswordForm.controls.confirmPassword.updateValueAndValidity();
          }) 
    }

    matchValues(matchTo: string): ValidatorFn {
        return (control: AbstractControl) => {
          return control?.value === control?.parent?.controls[matchTo].value ? null : { isMatching: true }
        }
      }

      IsLoaded() {
        if (this.currentUserId !== 0) {
            this.targetUrl = this.baseUrl + 'users/addUserPhoto/' + this.currentUserId;
            return true;
        } else { return false; }
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

    loadDrops() {
        const d = JSON.parse(localStorage.getItem('optionCountries'));
        if (d == null || d.length === 0) {
            this.drops.getAllCountries().subscribe((response) => {
                this.optionCountries = response; localStorage.setItem('optionCountries', JSON.stringify(response));
            });
        } else {
            this.optionCountries = JSON.parse(localStorage.getItem('optionCountries'));
        }
    }
    updatePhoto(photoUrl: string) { this.user.PhotoUrl = photoUrl; }
   
    updateUser() {
        this.userService.updateUser(this.currentUserId, this.user).subscribe(next => {
            this.editForm.reset(this.user);
            this.router.navigate(['/procedures']);
        }, error => { this.alertify.error(error); });

    }

    changeCountry() {
       let help = this.optionCountries.find(z => z.value === this.user.country);
        this.countryWhereUserLives = help.description;
    }

    updateFromWorkedIn(us: User) {
        this.userService.updateUser(this.currentUserId, us).subscribe(next => {
            this.router.navigate(['/procedures']);
        },
            error => { this.alertify.error(error); });
    }

    requestPremium() {this.router.navigate(['/premium']); }

    showPremium() {if (this.premium === 1) { return true } else { return false }}

   

    cancel() { this.router.navigate(['/procedures']); }

    changePasswordNow(){
       

       this.cpwd.email = this.user.email;
       this.cpwd.currentPassword = this.changePasswordForm.value.currentPassword;
       this.cpwd.password =        this.changePasswordForm.value.password;
       this.cpwd.confirmPassword = this.changePasswordForm.value.confirmPassword;
       this.auth.changePassword(this.cpwd).subscribe((next)=>{this.alertify.info(next);}, error => {this.alertify.error(error)});


    }


   

    canDeactivate() {
        this.updateUser();
        return true;
        // if (confirm("Are you sure you want to navigate away ?")) {
        //    return true;
        // } else {
        //    return false;
        // }
    }


}

