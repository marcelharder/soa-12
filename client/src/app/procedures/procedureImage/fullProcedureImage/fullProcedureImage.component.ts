import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { take } from 'rxjs/operators';
import { ProcedurePhoto } from 'src/app/_models/ProcedurePhoto';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-fullProcedureImage',
  templateUrl: './fullProcedureImage.component.html',
  styleUrls: ['./fullProcedureImage.component.css']
})
export class FullProcedureImageComponent implements OnInit {
  photoUrl = "";
  description = "";
  pictures: Array<ProcedurePhoto> = [];
  publicId = '';
  leftButton = 1;
  rightButton = 1;
  currentIndex = 0;
  numberOfItems = 0;
  constructor(
    private auth: AccountService, private route: ActivatedRoute) { }

  ngOnInit() {
    // get the publiId from route
    this.route.params.pipe(take(1)).subscribe(params => {this.publicId = params['publicId'];})
    // get the procedurePhoto Array
    this.auth.currentPictures.subscribe((next) => { this.pictures = next; });
    
    this.numberOfItems = this.pictures.length;
    var a = this.pictures.filter(x => x.PublicId == this.publicId);
    this.currentIndex = this.pictures.findIndex(x => x.PublicId == this.publicId);

    this.description = this.pictures[0].Description;
    this.photoUrl = a[0].Url;

  }

  getFullImage() { return this.photoUrl; }

  leftButtonClicked() {
    //go to the previous item in the procededurePhotoArray
    var h = this.currentIndex - 1;
    if (h < 0) { this.leftButton = 0; }
    else {
      this.photoUrl = this.pictures[h].Url;
      this.description = this.pictures[h].Description;
      this.currentIndex--;
      this.rightButton = 1;
      this.leftButton = 1;
    }
  }

  showTheLeftButton() { if (this.leftButton == 1) return true; }

  rightButtonClicked() {
    //go to the next item in the procededurePhotoArray
    var h = this.currentIndex + 1;
    if (h < this.numberOfItems) {
      this.photoUrl = this.pictures[h].Url;
      this.description = this.pictures[h].Description;
      this.currentIndex++;
      this.leftButton = 1;
      this.rightButton = 1;
    }
    else {
      this.rightButton = 0;
    }
  }

  showTheRightButton() { if (this.rightButton == 1) return true; }

}
