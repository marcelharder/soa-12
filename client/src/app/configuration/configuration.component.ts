import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-configuration',
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.css']
})
export class ConfigurationComponent implements OnInit {
  currentUserId = 0;
  hospitalPhoto = '';
  employeePhoto = '';
  refPhysPhoto = '';
  usersPhoto = '';
  refOperativeReport = '';
  hospital = 0;
  employee = 0;
  ref = 0;

  private sub: any;
  id: number;

  constructor(
    private router: Router,
    private alertify: ToastrService,
    private auth: AccountService,
    private user: UserService) { }

  ngOnInit(): void {

    this.auth.currentServiceLevel$.pipe(take(1)).subscribe((s) => {
      if (s === 1) {
        this.auth.currentUser$.pipe(take(1)).subscribe((u) => { this.currentUserId = u.UserId; });
        this.hospitalPhoto = 'https://res.cloudinary.com/marcelcloud/image/upload/v1567770782/Hospitals/kfafh.jpg';
        this.usersPhoto = 'https://res.cloudinary.com/marcelcloud/image/upload/v1560755000/rf9mgoftqqsdyndoaxqv.jpg';
        this.employeePhoto = 'https://res.cloudinary.com/marcelcloud/image/upload/v1569914898/employees/nurse-employee.jpg';
        this.refPhysPhoto = 'https://res.cloudinary.com/marcelcloud/image/upload/v1568664430/mdszyrzqtjpeiq9svokh.jpg';
        this.refOperativeReport = 'https://res.cloudinary.com/marcelcloud/image/upload/v1569919553/general/report.jpg';
      } else {
        this.router.navigate(['/']);
        this.alertify.error("You need a premium subscription ...")
      }
    })

  }
  editHospital() {
    this.user.getUser(this.currentUserId).subscribe((next) => {
      const currentUser = next;
      this.router.navigate(['/editHospital', currentUser.hospital_id]);
    });
  }

}



