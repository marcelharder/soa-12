import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  /**
   *
   */
  constructor(
    private authservice: AccountService,
    private router: Router,
    private alertify: ToastrService) {}

  canActivate(): Observable<boolean>{
    return this.authservice.currentUser$.pipe(
      map(user => {
      if (user) return true;
      this.alertify.error('You are not allowed');
      
      })
    )
  }
  
}
