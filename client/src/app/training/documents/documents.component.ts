import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Document_model } from 'src/app/_models/Document_model';
import { DocumentService } from 'src/app/_services/document.service';

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
      type: ['', ]
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
 

  }
  updateDocument(){
    this.listOfDocuments = [];
    this.ds.updateDocument(this.detailsForm.value, this.documentId).subscribe(
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
  updatePhoto(photoUrl: string) {
    this.selectedDocument = this.listOfDocuments.find(x => x.documentId == this.documentId);
    this.selectedDocument.document_url = photoUrl;
  }

  showPdf(){
    window.open(this.selectedDocument.document_url);
  }
  deleteModel(id: number){
    this.listOfDocuments = [];
    this.ds.deleteDocument(id).subscribe((next)=>{
      this.ds.getDocuments(this.userId).subscribe((next)=>{this.listOfDocuments = next;})
      this.selectedDocument = this.listOfDocuments.find(x => x.documentId == this.documentId);
    })
  }

  Cancel(){this.details = 0;}

}
