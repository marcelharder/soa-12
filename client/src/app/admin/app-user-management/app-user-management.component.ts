import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from 'src/app/_modals/roles-modal/roles-modal.component';
import { User } from 'src/app/_models/User';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-app-user-management',
  templateUrl: './app-user-management.component.html',
  styleUrls: ['./app-user-management.component.css']
})
export class AppUserManagementComponent implements OnInit {
  users: Partial<User[]>;
 
  bsModalRef: BsModalRef;
  constructor(private adminservice: AdminService, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.getUsersWithRoles();
   
  }

  getUsersWithRoles() {

    this.adminservice.getUsersWithRoles().subscribe(next => { this.users = next })
  }

  openRolesModal(user: User) {
    const config = {
      class: 'model-dialog-centered',
      initialState: {
        user,
        roles: this.getRolesArray(user)
      }

    };

    this.bsModalRef = this.modalService.show(RolesModalComponent, config);
    this.bsModalRef.content.updateSelectedRoles.subscribe(values => {
      const rolesToUpdate = {
        roles: [...values.filter(el => el.checked === true).map(el => el.name)]
      };
      if(rolesToUpdate){
        this.adminservice.updateUserRoles(user.UserName, rolesToUpdate.roles).subscribe(()=>{
          user.roles = [...rolesToUpdate.roles]
        })
      }
    })
 
  }
  private getRolesArray(user) {
    const roles = [];
    const userRoles = user.Roles;
    const availableRoles: any[] = [
      { name: 'Admin', value: 'Admin' },
      { name: 'Moderator', value: 'Moderator' },
      { name: 'Refcard', value: 'Refcard' },
      { name: 'Surgery', value: 'Surgery' },
      { name: 'Chef', value: 'Chef' }
    ];
    availableRoles.forEach(role => {
      let isMatch = false;
      for (const userRole of userRoles) {
        if (role.name === userRole) {
          isMatch = true;
          role.checked = true;
          roles.push(role);
          break;
        }
      }
      if (!isMatch) {
        role.checked = false;
        roles.push(role);
      }
    })
    return roles;






  }

}
