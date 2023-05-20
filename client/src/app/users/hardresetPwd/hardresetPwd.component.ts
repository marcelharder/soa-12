import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-hardresetPwd',
  templateUrl: './hardresetPwd.component.html',
  styleUrls: ['./hardresetPwd.component.css']
})
export class HardresetPwdComponent implements OnInit {
  current_user_id = 0;
  user_email = "";
  user_password = "";
  constructor(private route: ActivatedRoute,
    private us: UserService,
    private router: Router,
    private alertify: ToastrService) { }

  ngOnInit() {
    this.route.params.subscribe(next => { this.current_user_id = next.id; })
    this.us.getUser(this.current_user_id).subscribe(next => { this.user_email = next.UserName; })
  }

  cancel() { this.router.navigate(['users']) }
  reset() {
    if (this.user_password !== null || this.user_password !== undefined) {
      this.us.hardResetPWD(this.user_email, this.user_password).subscribe(
        (next) => {
          this.alertify.info(next);
        }, (error) => { this.alertify.error(error) })
    }
  }
}
