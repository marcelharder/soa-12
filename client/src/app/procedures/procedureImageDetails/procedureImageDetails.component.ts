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
 
  @Input() selectedProcedure: ProcedurePhoto = {
    procedureId: 0,
    description: '',
    url: '',
    publicId: '',
    dateAdded: undefined
  };

 

  constructor(private router: Router) { }

  ngOnInit() {
  }

  /* goDetails(id: number) {
     this.router.navigate(['/diaList/'+id]);
      }
  getImageFromServer(id: string) { // get it from the pictures Array
     return this.baseUrl + 'Images/getImageFile/' + id; 
     } */

}
