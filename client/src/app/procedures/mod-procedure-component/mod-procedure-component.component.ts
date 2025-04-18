import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { Procedure } from 'src/app/_models/Procedure';
import { AccountService } from 'src/app/_services/account.service';
import { ProcedureService } from 'src/app/_services/procedure.service';

@Component({
  selector: 'app-mod-procedure-component',
  templateUrl: './mod-procedure-component.component.html',
  styleUrls: ['./mod-procedure-component.component.css']
})
export class ModProcedureComponent implements OnInit {
  procedures: Array<Procedure> = [];
  pagination: Pagination;
  selectedHospital = "";

  constructor(private route: ActivatedRoute,
    private auth: AccountService,
    private router: Router,
    private procedureService: ProcedureService) { }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      debugger;
      this.procedures = data.procedure.result;
      this.pagination = data.procedure.pagination;
    });
    this.auth.currentHospitalName.subscribe((next) => {
      this.selectedHospital = next;
      if (this.selectedHospital === '0') {
        localStorage.removeItem("user");
        this.router.navigate(['/']);
      }

    });
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


}
