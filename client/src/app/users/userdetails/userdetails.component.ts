import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { countryItem } from 'src/app/_models/countryItem';
import { dropItem } from 'src/app/_models/dropItem';
import { User } from 'src/app/_models/User';
import { DropdownService } from 'src/app/_services/dropdown.service';

@Component({
  selector: 'app-userdetails',
  templateUrl: './userdetails.component.html',
  styleUrls: ['./userdetails.component.css']
})
export class UserdetailsComponent implements OnInit {
  @Output() fromUserEdit = new EventEmitter<Partial<User>>();
  @Output() cancelThis = new EventEmitter<number>();
  @Input() user: Partial<User>;
  optionsGender:Array<dropItem> = [];
  optionCountries:Array<countryItem> = [];
  public image = 0;
  hospitals: Array<dropItem> = [];
  selectedCountry = "";
  
  constructor(
    private route: ActivatedRoute, 
    private router: Router, 
    private alertify: ToastrService,
    private drops: DropdownService) { }

  ngOnInit(): void {
    this.selectedCountry = this.user.country;

    this.drops.getAllHospitals().subscribe(response => {
      this.hospitals = response;
      this.hospitals.unshift({value:0,description:"Choose"});
    }, (error) => { console.log(error); });
  
  this.loadDrops();
  this.selectedCountry = this.user.country;

  }

  updateUserDetails(){ 
    this.user.country = this.selectedCountry;
    if(this.user.country !== ""){
      if(this.user.hospital_id === 0){
        this.alertify.error("Please select your current hospital");
      } else {
      this.user.worked_in = this.user.hospital_id.toString();
      this.fromUserEdit.emit(this.user);}
    }
   
  }

  Cancel(){this.cancelThis.emit(1)};

  loadDrops(){
    this.drops.getGenderOptions().subscribe((next) => { this.optionsGender = next; });

    const d = JSON.parse(localStorage.getItem('optionCountries'));
        if (d == null || d.length === 0) {
            this.drops.getAllCountries().subscribe((response) => {
                this.optionCountries = response;
                this.optionCountries.unshift({Id:"",Description:"Choose",TelCode:"",IsoCode:"",Cities:""}); 
                localStorage.setItem('optionCountries', JSON.stringify(response));
            });
        } else {

            this.optionCountries = JSON.parse(localStorage.getItem('optionCountries'));
        }
  }

  changeCountry() {

    this.drops.getAvailableHospitals(this.selectedCountry).subscribe(
        (next) => {
            this.hospitals = next;
            this.user.hospital_id = this.hospitals[0].value;
        });
}

 



}
