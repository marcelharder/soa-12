import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { ProcedurePhoto } from 'src/app/_models/ProcedurePhoto';
import { AccountService } from 'src/app/_services/account.service';
import { ProcedureService } from 'src/app/_services/procedure.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-procedurePictures',
  templateUrl: './procedurePictures.component.html',
  styleUrls: ['./procedurePictures.component.css']
})
export class ProcedurePicturesComponent implements OnInit {
  id = 0;
  soort = '0';
  targetUrl = '';
  baseUrl = environment.apiUrl;
  Photo: ProcedurePhoto = {
    ProcedureId: 0,
    Description: '',
    Url: '',
    PublicId: '',
    DateAdded: undefined,
    Id: 0
  }
  pictures: Array<ProcedurePhoto> = [];


  constructor(
    private auth: AccountService,
    private proc: ProcedureService,
    private route: ActivatedRoute,
    private alertify: ToastrService,
    private router: Router

  ) { }

  ngOnInit() {
    this.route.params.pipe(take(1)).subscribe(params => {
      this.id = +params['id'];
      this.soort = params['soort'];
    })
    if (this.soort == '1') { this.loadPictures(this.id); }

  }

  showAdd() {
    if (this.soort === '2') {
      return true;
    } else {
      return false;
    }
  }

  IsLoaded() {
    if (this.id !== 0) {
      this.targetUrl = this.baseUrl + 'procedure/addProcedurePhoto/' + this.id;
      return true;
    } else { return false; }
  }

  loadPictures(id: number) {
    this.proc.getPhotosAvailable(id).subscribe((next) => {
      if (next) {
        this.proc.getProcedurePhotos(id).subscribe((next) => {
          this.pictures = next;
          this.auth.setCurrentPictures(this.pictures);
        })
      }
      else { this.alertify.info("no pictures found ..."); }
    })
  }

  updateProcedurePhoto(t: ProcedurePhoto) {
    this.Photo.Url = t.Url;
    this.Photo.DateAdded = t.DateAdded;
    this.Photo.PublicId = t.PublicId;
  }

  cancel() { this.router.navigateByUrl('/procedures'); }

  saveProcedurePhoto() {
    this.Photo.ProcedureId = this.id;
    // save this to the database in PsSOA
    this.proc.saveToPfSoa(this.Photo).subscribe(
      (next) => {
        if (next == '1') { this.alertify.info('Photo added to procedure'); }
        this.soort = "1";
        this.loadPictures(this.id);
      },
      (error) => { this.alertify.error(error) }
    )
  }


  addProcedure() { this.soort = "2"; }

  receiveFromDetails(Id: number) {
    debugger;
    // remove this image from the database
    this.proc.deleteProcedurePhoto(Id).subscribe((next) => {
      // remove this image from the pictures array
      this.pictures = this.pictures.filter(x => x.Id !== Id);
    }, (error) => { this.alertify.error(error) }
    );
  }





}



