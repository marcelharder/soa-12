import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProcedurePhoto } from 'src/app/_models/ProcedurePhoto';
import { ProcedureService } from 'src/app/_services/procedure.service';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-procedureImageDetails',
  templateUrl: './procedureImageDetails.component.html',
  styleUrls: ['./procedureImageDetails.component.css']
})
export class ProcedureImageDetailsComponent implements OnInit {
 
  @Input() selectedProcedure: ProcedurePhoto = {
    ProcedureId: 0,
    Description: '',
    Url: '',
    PublicId: '',
    DateAdded: undefined,
    Id: 0
  };
  pictures: Array<ProcedurePhoto> = [];
 
  constructor(
    private auth: AccountService,
    private router: Router,
    private proc: ProcedureService,
    private alertify: ToastrService) { }

  ngOnInit() {
      // get the procedurePhoto Array
      this.auth.currentPictures.subscribe((next) => { this.pictures = next; });
  }

  getImageUrlFromArray(f: number) {
    // select the correctItem
    var selected = this.pictures.filter(x => x.Id == f);
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

  getImageFull(publicId: string){
    this.router.navigateByUrl('/fullProcedureImage/' + publicId);
  }
}
