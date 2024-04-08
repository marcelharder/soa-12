import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Publication } from 'src/app/_models/CME/Publication';
import { PublicationService } from 'src/app/_services/publication.service';

@Component({
  selector: 'app-publications',
  templateUrl: './publications.component.html',
  styleUrls: ['./publications.component.css'],
})
export class PublicationsComponent implements OnInit {
  @Input() userId: number;
  details = 0;
  detailsForm: FormGroup | undefined;
  listOfPublications: Array<Publication> = [];
  currentPublication: Publication = {
    PublicationId: 0,
    userId: 0,
    Author: '',
    Title: '',
    Volume: '',
    Issue: '',
    PlaceOfPublication: '',
    Publisher: '',
    Editor: '',
    DateOfPublication: undefined,
    URL: '',
    DOI: '',
  };

  constructor(
    private alertify: ToastrService,
    private pre: PublicationService,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    this.pre.getListOfPrublications(this.userId).subscribe((next) => {
      this.listOfPublications = next;
    });
    this.initializeForm();
  }

  initializeForm() {
    this.detailsForm = this.fb.group({
      PublicationId: [''],
      userId: 0,
      Author: [''],
      Title: [''],
      Volume: [''],
      Issue: [''],
      PlaceOfPublication: [''],
      Publisher: [''],
      Editor: [''],
      DateOfPublication: [''],
      URL: [''],
      DOI: [''],
    });
  }

  showDetailsPanel() { if (this.details == 1) { return true; } else { return false; }
  }

  showPublicationDetails(id: number){
    this.details = 1;
    this.currentPublication =  this.listOfPublications.find(x => x.PublicationId == id);
 
    this.detailsForm.controls.PublicationId.setValue(this.currentPublication.PublicationId);
  }
  deleteCurrentPublication(id: number){}
  
  addPublication() {
    this.alertify.info('Adding publication');
    this.details = 1;
  }

  updatePublication() {
    this.alertify.info('Updating publication');
  }
}
