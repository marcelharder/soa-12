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
    publicationId: 0,
    userId: 0,
    author: '',
    title: '',
    volume: '',
    issue: '',
    placeOfPublication: '',
    publisher: '',
    editor: '',
    dateOfPublication: undefined,
    url: '',
    doi: '',
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

  Cancel(){this.details = 0;}

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
    this.currentPublication =  this.listOfPublications.find(x => x.publicationId == id);

    this.detailsForm.controls.PublicationId.setValue(this.currentPublication.publicationId);
    this.detailsForm.controls.Title.setValue(this.currentPublication.title); 
    this.detailsForm.controls.Author.setValue(this.currentPublication.author); 
    this.detailsForm.controls.Issue.setValue(this.currentPublication.issue); 
    this.detailsForm.controls.PlaceOfPublication.setValue(this.currentPublication.placeOfPublication); 
    this.detailsForm.controls.Publisher.setValue(this.currentPublication.publisher); 
    this.detailsForm.controls.Editor.setValue(this.currentPublication.editor); 
    this.detailsForm.controls.DateOfPublication.setValue(this.currentPublication.dateOfPublication); 
    this.detailsForm.controls.URL.setValue(this.currentPublication.url); 
    this.detailsForm.controls.DOI.setValue(this.currentPublication.doi); 

 
    this.detailsForm.controls.PublicationId.setValue(this.currentPublication.publicationId);
  }
  deleteCurrentPublication(id: number){
    this.pre.deletePublication(id).subscribe((next)=>{
      this.pre.getListOfPrublications(this.userId).subscribe((next)=>{this.listOfPublications = next;}); 
      this.currentPublication = this.listOfPublications.find(x => x.publicationId == this.currentPublication.publicationId);
      this.alertify.info("Presentation removed");
    })

  }
  
  addPublication() {

    this.pre.createPublication(this.userId).subscribe((next)=>{
      this.currentPublication = next;

      this.detailsForm.controls.PublicationId.setValue(this.currentPublication.publicationId);
      this.detailsForm.controls.Title.setValue(this.currentPublication.title); 
      this.detailsForm.controls.Author.setValue(this.currentPublication.author); 
      this.detailsForm.controls.Issue.setValue(this.currentPublication.issue); 
      this.detailsForm.controls.PlaceOfPublication.setValue(this.currentPublication.placeOfPublication); 
      this.detailsForm.controls.Publisher.setValue(this.currentPublication.publisher); 
      this.detailsForm.controls.Editor.setValue(this.currentPublication.editor); 
      this.detailsForm.controls.DateOfPublication.setValue(this.currentPublication.dateOfPublication); 
      this.detailsForm.controls.URL.setValue(this.currentPublication.url); 
      this.detailsForm.controls.DOI.setValue(this.currentPublication.doi); 
     

    this.alertify.info('Adding publication');
    this.details = 1;
    });
  }

 
  updatePublication(){
    this.pre.updatePublication(this.detailsForm.value).subscribe((next)=>{
      this.pre.getListOfPrublications(this.userId).subscribe((next)=>{this.listOfPublications = next;}); 
      this.currentPublication = this.listOfPublications.find(x => x.publicationId == this.currentPublication.publicationId);




   

      this.details = 0;
      this.alertify.info('Updating publication');


    })
  }
}
