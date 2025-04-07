import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { ProcedurePhoto } from 'src/app/_models/ProcedurePhoto';
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
    procedureId: 0,
    Description: '0',
    Url: '',
    PublicId: '',
    DateAdded: undefined
  }
  pictures: Array<ProcedurePhoto> = [];


  constructor(
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
    this.loadPictures(this.id);
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
    if (id != 1) { }
    else {
      this.alertify.info("Loading pictures");
    }
  }

  updatePhoto(photoUrl: string) {
    this.Photo.procedureId = this.id;
    this.Photo.Url = photoUrl;
    this.Photo.Description = "";
    this.Photo.DateAdded = new Date;
    this.Photo.PublicId = '0';
    this.pictures.push(this.Photo);
    this.soort = "1";
  }

  cancel() { this.router.navigateByUrl('/procedures'); }

}



