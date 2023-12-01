import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';
import { HospitalService } from 'src/app/_services/hospital.service';
import { OviUpdate } from 'src/app/_models/OviUpdate';
import { User } from 'src/app/_models/User';
import { dropItem } from 'src/app/_models/dropItem';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/_services/account.service';
import { countryItem } from 'src/app/_models/countryItem';
import { take } from 'rxjs/operators';

@Component({
    selector: 'app-worked-in',
    templateUrl: './worked-in.component.html',
    styleUrls: ['./worked-in.component.css']
})

export class WorkedInComponent implements OnInit {
    @Input() user: User;
    @Output() updateUserToParent = new EventEmitter<User>();
    selectedHospital = 0;

    selectedCountry = '';
    currentUserId = 0;
    currentUserName = '';
    currentHospital = 0;
    OptionActiveHospitals: Array<dropItem> = [];
    OptionCountries: Array<countryItem> = [];
    OptionHospitals: Array<dropItem> = [];
    OviUpdate: OviUpdate = { name: '', role: '', gender: '', email: '' }

    constructor(
        private drop: DropdownService,
        private alertify: ToastrService,
        private hos: HospitalService,
        private Auth: AccountService,
        private router: Router) { }

    ngOnInit(): void {
        // get the currentUser from account
        this.Auth.currentUser$.pipe(take(1)).subscribe((u) => {
            this.currentUserId = u.UserId;
            this.currentUserName = u.UserName;
        });

        this.drop.getHospitals(this.currentUserId).subscribe(response => {
            this.OptionActiveHospitals = response;
        }, (error) => { console.log(error); });
        // get all countries
        this.drop.getAllCountries().subscribe((next) => {
            this.OptionCountries = next;
            this.selectedCountry = this.user.country;
            // get all hospitals per country
            this.drop.getAvailableHospitals(this.selectedCountry).subscribe((nex) => {
                this.OptionHospitals = nex;
            });
        });
        this.currentHospital = this.user.hospital_id;
    }
    changeCurrentHospital() {
        if (this.currentHospital !== 0) {
            this.hos.getSpecificHospital(this.currentHospital).subscribe(
                (response) => {
                    this.Auth.changeCurrentHospital(response.HospitalName);
                });
            this.user.hospital_id = this.currentHospital; // change the current hospital of the current user
            // send the changed user up to the parent
            this.updateUserToParent.emit(this.user);
        }
    }
    changeCountry() {
        this.drop.getAvailableHospitals(this.selectedCountry).subscribe(
            (next) => {
                this.OptionHospitals = next;
                this.selectedHospital = this.OptionHospitals[0].value;
            });
    }
    addToListOfHospitals() {
        const a: Array<number> = [];
        this.hos.getSpecificHospital(this.selectedHospital).subscribe((res) => {
            const help: dropItem = { value: +res.HospitalNo, description: res.HospitalName }
            if (!this.OptionActiveHospitals.find(e => e.value === +res.HospitalNo)) {
                this.OptionActiveHospitals.push(help);
                for (const row of this.OptionActiveHospitals) { a.push(row.value); }
                this.user.worked_in = a.join(',');
            }
        });
    }
    removeCurrentHospital() {
        const a: Array<number> = [];
        const index = this.OptionActiveHospitals.findIndex(x => x.value === +this.currentHospital); // find index in your array
        this.OptionActiveHospitals.splice(index, 1);// remove element from array
        for (const row of this.OptionActiveHospitals) { a.push(row.value); }
        this.user.worked_in = a.join(',');
    }
}
