import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ProcedurePhoto } from 'src/app/_models/ProcedurePhoto';
import { ProcedureService } from 'src/app/_services/procedure.service';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-procedureImageDetails',
  templateUrl: './procedureImageDetails.component.html',
  styleUrls: ['./procedureImageDetails.component.css']
})
export class ProcedureImageDetailsComponent implements OnInit {
  @Input() ProcedurePhotos: Array<ProcedurePhoto> = [];
  @Input() selectedProcedure: ProcedurePhoto = {
    ProcedureId: 0,
    Description: '',
    Url: '',
    PublicId: '',
    DateAdded: undefined,
    Id: 0
  };
  



  constructor(
    private router: Router,
    private proc: ProcedureService,
    private alertify: ToastrService) { }

  ngOnInit() {
  }

  getImageUrlFromArray(f: number) {
    // select the correctItem
    var selected = this.ProcedurePhotos.filter(x => x.Id == f);
    return selected[0].Url;
  }

  deleteImage() {
    this.proc.deleteProcedurePhoto(this.selectedProcedure.Id).subscribe((next) => {
      if (next == 1) {
        this.router.navigateByUrl('/procedures');
      }
    },
      (error) => { this.alertify.error(error); }
    )
  }

  getImageFull(sid: number){
    this.alertify.error("test");
  }
}
