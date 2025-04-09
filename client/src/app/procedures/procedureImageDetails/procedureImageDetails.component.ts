import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProcedurePhoto } from 'src/app/_models/ProcedurePhoto';
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
    DateAdded: undefined
  };

 

  constructor(private router: Router) { }

  ngOnInit() {
  }

  getImageUrlFromArray(f: string){
    // select the correctItem
    var selected = this.ProcedurePhotos.filter(x => x.PublicId == f);
    return selected[0].Url;
  }

  /* goDetails(id: number) {
     this.router.navigate(['/diaList/'+id]);
      }
  getImageFromServer(id: string) { // get it from the pictures Array
     return this.baseUrl + 'Images/getImageFile/' + id; 
     } */

}
