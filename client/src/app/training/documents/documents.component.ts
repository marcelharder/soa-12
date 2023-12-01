import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Document_model } from 'src/app/_models/Document_model';
import { DocumentService } from 'src/app/_services/document.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-documents',
  templateUrl: './documents.component.html',
  styleUrls: ['./documents.component.css']
})
export class DocumentsComponent implements OnInit {
@Input() userId:number;
detailsForm: FormGroup | undefined;
details = 0;

selectedDocument:Partial<Document_model> = {
  documentId: 0,
  description: '',
  dateAdded: new Date,
  type: 0,
  finished: false,
  document_url: '',
  publicId: '',
  userId: 0
}
documentId = 0;


listOfDocuments: Document_model[] = [];
  
constructor(private ds: DocumentService, 
  private alertify: ToastrService, 
  private router: Router,
  private fb: FormBuilder,) { }

  ngOnInit() {
    // get the list of documents
    this.ds.getDocuments(this.userId).subscribe((next)=>{this.listOfDocuments = next;})
    this.initializeForm();
  }
  initializeForm() {
    this.detailsForm = this.fb.group({
      documentId: ['', ],
      userId: ['', ],
      finished: [false, ],
      description: ['', ],
      dateAdded: ['', ],
      type: ['', ],
      document_url: ['', ]
    });
  }

  showDetailsPanel(){if(this.details == 1){return true;}else{return false;}}

  showDetails(id: number){
   this.documentId = id;
   this.selectedDocument = this.listOfDocuments.find(x => x.documentId == id);
   this.details = 1;
 
   this.detailsForm.controls.documentId.setValue(this.selectedDocument.documentId);
   this.detailsForm.controls.userId.setValue(this.userId);
   this.detailsForm.controls.finished.setValue(this.selectedDocument.finished);
   this.detailsForm.controls.description.setValue(this.selectedDocument.description);
   this.detailsForm.controls.dateAdded.setValue(this.selectedDocument.dateAdded);
   this.detailsForm.controls.type.setValue(this.selectedDocument.type);
   this.detailsForm.controls.document_url.setValue(this.selectedDocument.document_url);
 

  }
  updateDocument(){

    this.listOfDocuments = [];
    this.ds.updateDocument(this.detailsForm.value).subscribe(
      (next)=>{
      this.ds.getDocuments(this.userId).subscribe((next)=>{this.listOfDocuments = next;})
      this.selectedDocument = this.listOfDocuments.find(x => x.documentId == this.documentId);
    },
    (error)=>{this.alertify.error(error)},
    ()=>{this.details = 0;});
   
   
  }

 
  
    uploadPhoto(){
    this.alertify.info("uploading photo");
   
  
  
  
  }
  addDocument(){
    this.ds.createDocument(this.userId).subscribe((next)=>{
      this.documentId = next.documentId;
      // push it to the list
    }, 
    (error)=>{this.alertify.error(error)},
    ()=>{
      this.ds.getDocuments(this.userId).subscribe((next)=>{this.listOfDocuments = next;});
      this.selectedDocument = this.listOfDocuments.find(x => x.documentId == this.documentId);
    });
  }

  updatePhoto(photoUrl: string) {
    this.selectedDocument = this.listOfDocuments.find(x => x.documentId == this.documentId);
    this.selectedDocument.document_url = photoUrl;
    this.detailsForm.controls.document_url.setValue(photoUrl);
  }

  showPdf(){
    window.open(this.selectedDocument.document_url);
  }
  deleteModel(id: number){
   
    this.listOfDocuments = [];

    this.ds.deleteDocument(id).subscribe((next)=>{
      this.ds.getDocuments(this.userId).subscribe((next)=>{this.listOfDocuments = next;})
      this.selectedDocument = this.listOfDocuments.find(x => x.documentId == this.documentId);
    }, (error)=>{this.alertify.error(error)},
    ()=>{this.details = 0;})
  }

  Cancel(){this.details = 0;}

}
