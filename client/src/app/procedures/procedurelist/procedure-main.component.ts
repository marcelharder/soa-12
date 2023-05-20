import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ProcedureService } from 'src/app/_services/procedure.service';
import { Router, ActivatedRoute } from '@angular/router';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { Procedure } from 'src/app/_models/Procedure';
import { User } from 'src/app/_models/User';
import { AccountService } from 'src/app/_services/account.service';
import { HospitalService } from 'src/app/_services/hospital.service';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/_services/user.service';
import { map, take } from 'rxjs/operators';

@Component({
  selector: 'app-procedure-main',
  templateUrl: './procedure-main.component.html',
  styleUrls: ['./procedure-main.component.css']
})
export class ProcedureMainComponent implements OnInit {
  currentUserId = 0;
  user: User;
  primarySurgeon = true;
  modalRef?: BsModalRef;

  procedures: Array<Procedure> = [];
  pagination: Pagination;
  selectedHospital = '';

  constructor(
    private modalService: BsModalService,
    private procedureService: ProcedureService,
    private userService: UserService,
    private alertify: ToastrService,
    private auth: AccountService,
    private hos: HospitalService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.procedures = data.procedure.result;
      this.pagination = data.procedure.pagination;
    });
    this.auth.currentUser$.pipe(take(1)).subscribe((u) => {
      this.currentUserId = u.UserId;
    });
    this.auth.currentHospitalName.subscribe((next)=>{
      this.selectedHospital = next;
      if(this.selectedHospital === '0'){
       localStorage.removeItem("user");
       this.router.navigate(['/']);
      }
    
    });
 }

 openModal(template: TemplateRef<any>) {
  this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
}

confirm(): void {
  this.router.navigate(['/addProcedure']);
  this.modalRef?.hide();
}

decline(): void {
  this.modalRef?.hide();
}

  pageChanged(event: any): void { this.pagination.currentPage = event.page; this.loadProcedures(); }

  loadProcedures() {
    this.procedureService.getProcedures(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe(
      (res: PaginatedResult<Procedure[]>) => {
        this.procedures = res.result;
        this.pagination = res.pagination;
      },
      error => {
        console.log('here is my error' + error);
      }
    );
  }
  loadAssistedProcedures() {
    this.procedureService.getAssistedProcedures(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe(
      (res: PaginatedResult<Procedure[]>) => {
        this.procedures = res.result;
        this.pagination = res.pagination;
      },
      error => {
        console.log('here is my error' + error);
      }
    );
  }

 

  isCompleted(pd: string) { if (pd === 'Yes') { return true; } }
  isEligible(pd: string) { if (pd !== 'N/A') { return true; } }

  testNav(pro: Procedure) {
    this.auth.setCurrentProcedure(pro.procedureId);
    this.router.navigate(['/procedureDetails']);
  }

  changeSurgeonType() {
    if (!this.primarySurgeon) { this.loadProcedures(); }
    else { this.loadAssistedProcedures(); }

  }



}
