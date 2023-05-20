import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { countryItem } from 'src/app/_models/countryItem';
import { dropItem } from 'src/app/_models/dropItem';
import { RefPhysModel } from 'src/app/_models/RefPhysModel';
import { User } from 'src/app/_models/User';
import { AccountService } from 'src/app/_services/account.service';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { RefPhysService } from 'src/app/_services/refPhys.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-refphys',
  templateUrl: './refphys.component.html',
  styleUrls: ['./refphys.component.css']
})
export class RefphysComponent implements OnInit {
  currentUserId = 0;
  refphysicians: Array<dropItem> = [];
  states: Array<dropItem> = [];
  countries: Array<countryItem> = [];
  optionsYN: Array<dropItem> = [];
  currentUser: User;
  selectedRef = 0;
  modalRef: BsModalRef;
  edit = '0';
  mail = false;
  active = false;
  pd: RefPhysModel = {
    Id: 0,
    hospital_id: 0,
    name: '',
    image:
      'https://res.cloudinary.com/marcelcloud/image/upload/v1559818775/user.png.jpg',
    address: '',
    street: '',
    postcode: '',
    city: '',
    state: '',
    country: '31', // the default setting for now
    tel: '',
    fax: '',
    email: '',
    send_email: false,
    active: false,
  };

  constructor(
    private modalService: BsModalService,
    private drops: DropdownService,
    private alertify: ToastrService,
    private auth: AccountService,
    private router: Router,
    private userservice: UserService,
    private refService: RefPhysService
  ) {}

  ngOnInit(): void {
    this.loadDrops();
    this.auth.currentUser$.pipe(take(1)).subscribe((u) => {this.currentUserId = u.UserId;})
    this.userservice
      .getUser(this.currentUserId)
      .subscribe((next) => {
        this.currentUser = next;
        this.refService
          .getRefPhys(this.currentUser.hospital_id)
          .subscribe((nex) => {
            this.refphysicians = nex;
            this.selectedRef = this.refphysicians[0].value;
          });
      });
  }
  loadDrops() {
    let d = JSON.parse(localStorage.getItem('YN'));
    if (d == null || d.length === 0) {
      this.drops.getYNOptions().subscribe((response) => {
        this.optionsYN = response;
        localStorage.setItem('YN', JSON.stringify(response));
      });
    } else {
      this.optionsYN = JSON.parse(localStorage.getItem('YN'));
    }
    this.states = [
      {
        value: 0,
        description: 'Choose',
      },
      { value: 0, description: 'CA' },
      { value: 0, description: 'NH' },
      { value: 0, description: 'OT' },
    ];

    d = JSON.parse(localStorage.getItem('countries'));
    if (d == null || d.length === 0) {
      this.drops.getAllCountries().subscribe((response) => {
        this.countries = response;
        localStorage.setItem('countries', JSON.stringify(response));
      });
    } else {
      this.countries = JSON.parse(localStorage.getItem('countries'));
    }
  }
  editCardiologist() {
    this.refService.getSpecificRefPhys(+this.selectedRef).subscribe((next) => {
      this.pd = next;
      this.edit = '1';
    });
  }
  addCardiologist() {
    // this.alertify.confirm("You want to add a refering physician", function (e) {
    //    if (e) {
    this.edit = '1';
    this.refService.addRefPhys().subscribe((next) => {
      this.pd = next;
    });
    //    } else {
    //    }
    // });
  }
  updatePhoto(photoUrl: string) {
    this.pd.image = photoUrl;
  }

  showEdit() {
    if (this.edit === '1') {
      return true;
    } else {
      return false;
    }
  }
  DeleteRefPhys(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }
  confirm(): void {
    this.refService.deleteRefPhys(this.pd.Id).subscribe((next) => {
      if (next === 1) {
        this.alertify.success('deleted');
      } else {
        this.alertify.error('ref phys could not be deleted');
      }
      this.edit = '0';
      this.router.navigate(['/config']);
    });
    this.modalRef?.hide();
  }
  
  decline(): void {
    this.modalRef?.hide();
  }
  cancelChanges() {
    this.edit = '0';
  }
  SaveRefPhys() {
    this.refService.updateRefPhys(this.pd).subscribe((next) => {
      this.alertify.success('saved');
      this.edit = '0';
    }, error => this.alertify.error(error));
  }
  showState() {
    if (this.pd.country === '1') {
      return true;
    }
  }


}
