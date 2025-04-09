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
    description: '',
    url: '',
    publicId: '',
    dateAdded: undefined
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
    if(this.soort == '1'){this.loadPictures(this.id);}
    
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
    this.proc.getPhotosAvailable(id).subscribe((next)=>{
      if(next){
        this.proc.getProcedurePhotos(id).subscribe((next)=>{
          this.pictures = next;
        })
      }
      else{this.alertify.info("no pictures found ...");}
    })
  }

  updateProcedurePhoto(t: ProcedurePhoto) {
    this.Photo.url = t.url;
    this.Photo.dateAdded = t.dateAdded;
    this.Photo.publicId = t.publicId;
  }

  cancel() { this.router.navigateByUrl('/procedures'); }

  saveProcedurePhoto(){
   this.Photo.procedureId = this.id;
   // save this to the database in PsSOA
    this.proc.saveToPfSoa(this.Photo).subscribe(
      (next)=>{
        this.alertify.info(next);
        this.pictures.push(this.Photo);
        this.soort = "1";
      },
      (error)=>{this.alertify.error(error)}
    )
  }

}



