import { Component, Input, OnInit } from '@angular/core';
import { Document_model } from 'src/app/_models/Document_model';
import { DocumentService } from 'src/app/_services/document.service';

@Component({
  selector: 'app-documents',
  templateUrl: './documents.component.html',
  styleUrls: ['./documents.component.css']
})
export class DocumentsComponent implements OnInit {
@Input() userId:number;

listOfDocuments: Document_model[] = [];
  
constructor(private ds: DocumentService) { }

  ngOnInit() {
    // get the list of documents
    this.ds.getDocuments(this.userId).subscribe((next)=>{
      
      this.listOfDocuments = next;
      
    
    
    
    
    
    })
  }

}
