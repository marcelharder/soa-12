import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-fullProcedureImage',
  templateUrl: './fullProcedureImage.component.html',
  styleUrls: ['./fullProcedureImage.component.css']
})
export class FullProcedureImageComponent implements OnInit {
  photoUrl="https://res.cloudinary.com/marcelcloud/image/upload/v1744220630/pnggidtcw9a1bzmfjgnc.jpg";
  constructor() { }

  ngOnInit() {
  }

  getFullImage(){return this.photoUrl;}

}
