import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TabDirective } from 'ngx-bootstrap/tabs';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { countryItem } from 'src/app/_models/countryItem';
import { dropItem } from 'src/app/_models/dropItem';
import { User } from 'src/app/_models/User';
import { AccountService } from 'src/app/_services/account.service';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { HospitalService } from 'src/app/_services/hospital.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-userlist',
  templateUrl: './userlist.component.html',
  styleUrls: ['./userlist.component.css']
})
export class UserlistComponent implements OnInit {
  user: Partial<User> = {
    UserId: 0, 
    PhotoUrl: '', 
    hospital_id:0,
    gender:'',
    country:'',
    city:'',
    active: true,
    ltk: false};
    lookupId = 0;
  users: Array<User> = [];
  allUsers: Array<User> = [];
  position = "";
  currentUserId = 0;
  currentHospital = 0;
  currentCountry = "";
  
  hospitals: Array<dropItem> = [];
  countries: Array<countryItem> = [];
 value?: string = 'User management';
  editFlag = 0;
  addFlag = 0;
  constructor(
    private userService: UserService,
    private hospitalService: HospitalService,
    private alertify: ToastrService,
    private drop: DropdownService,
    private router: Router,
    private auth: AccountService) { }

  ngOnInit(): void {
    this.auth.currentUser$.pipe(take(1)).subscribe((u) => { this.currentUserId = u.UserId; });
    this.drop.getAllCountries().subscribe((next)=>{
      this.countries = next;
    });

   

    this.loadDrops();
    this.getUsers();
  }
  loadDrops() {}

  selectCountry(){
    debugger;
    this.drop.getAvailableHospitals(this.currentCountry).subscribe(response => {
      debugger;
      this.hospitals = response;
      this.currentHospital = response[0].value
    }, (error) => { console.log(error); });
  }

  LookUpUserId(){
    if(this.lookupId !== 0){ 
      // get the user with id === lookupId
      // this.users.push(the newly found user);
      this.users.splice(0); //empties the array
      this.userService.getUser(this.lookupId).subscribe(
        (next)=>{this.users.push(next)}, 
        error => {this.alertify.error(error)}, 
        ()=>{this.lookupId = 0;})
      this.alertify.info("find user with UserId = " + this.lookupId)}
  }
   

 
  getUsers() {
    this.userService.getUsers().subscribe(next => {
       this.allUsers = next.result;
      this.users = this.allUsers.filter(a => a.hospital_id == this.currentHospital);
    }, error=>{this.alertify.error(error)})
  }

  onSelect(data: TabDirective): void {this.value = data.heading; }

  showHospitalDrop() { if (this.value === 'User management') { return true } }
  getPosition(ltk: boolean) { if (ltk) { return "Surgeon" } else { return "Resident" } }

  selectUserPerHospital() {
   
    
    this.users = this.allUsers.filter(a => a.hospital_id == this.currentHospital);}

  editUser(id: number) {
    this.userService.getUser(id).subscribe((next)=>{
      this.user = next;
      this.editFlag = 1; this.addFlag = 0;
    }, (error)=> {this.alertify.error(error)})
    }

  returnFromUserEdit(ret: User){
    this.userService.updateUser(this.currentUserId, ret).subscribe(
      (next)=>{
        this.editFlag = 0; this.addFlag = 0;
        this.getUsers();
      }, 
      (error)=>{
        this.alertify.error(error)})
  }


  AddUser() {
     this.editFlag = 0; this.addFlag = 1; }

  returnFromAddUser(newUserId: number){
    this.userService.getUser(newUserId).subscribe((next)=>{
      this.user = next;
      this.user.hospital_id = this.currentHospital;
      this.editFlag = 1; this.addFlag = 0;
    }, (error)=> {this.alertify.error(error)});
  }

  cancelAdd(){this.editFlag = 0; this.addFlag = 0;};
  cancelEdit(){this.editFlag = 0; this.addFlag = 0;};

  showEdit() { if (this.editFlag === 1) return true; }
  showAdd() { if (this.addFlag === 1) return true; }

  deleteUser(id: number) {
    this.userService.deleteUser(id).subscribe((next)=>{
      this.alertify.show("User removed ..");
      this.getUsers();
    
    })
   }
  Cancel() { this.router.navigate(['users']) }
  changePWDUser(id: number){this.router.navigate(['hardresetpassword/'+ id]);}
}



function foreach(Hospital: any, arg1: boolean) {
  throw new Error('Function not implemented.');
}

function Hospital(Hospital: any, arg1: boolean) {
  throw new Error('Function not implemented.');
}

