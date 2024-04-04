import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Presentation } from 'src/app/_models/CME/Presentation';
import { PresentationService } from 'src/app/_services/presentation.service';

@Component({
  selector: 'app-presentations',
  templateUrl: './presentations.component.html',
  styleUrls: ['./presentations.component.css']
})
export class PresentationsComponent implements OnInit {
  @Input() userId: number;
  details = 0;
  detailsForm: FormGroup | undefined;
  listOfPresentations:Array<Presentation> = [];
  currentPresentation: Presentation = {
    presentationId: 0,
    title: '',
    media: '',
    venue: '',
    datePresented: undefined
  };
  constructor(private pre: PresentationService, private fb: FormBuilder,) { }

  ngOnInit() {
    this.pre.getListOfPresentations(this.userId).subscribe((next)=>{
      debugger;
      this.listOfPresentations = next;
    });
    this.initializeForm();

  }

  showDetails(id:number){
   this.details = 1;
   this.currentPresentation =  this.listOfPresentations.find(x => x.presentationId == id);

   this.detailsForm.controls.PresentationId.setValue(this.currentPresentation.presentationId);
   this.detailsForm.controls.Title.setValue(this.currentPresentation.title); 
   this.detailsForm.controls.Venue.setValue(this.currentPresentation.venue); 
   this.detailsForm.controls.DatePresented.setValue(this.currentPresentation.datePresented); 
   this.detailsForm.controls.Media.setValue(this.currentPresentation.media);
  }

  initializeForm(){
    this.detailsForm = this.fb.group({
      PresentationId: ['', ],
      Title: ['', ],
      Media: ['', ],
      Venue: ['', ],
      DatePresented: ['', ]
    });
  }

  showDetailsPanel(){if(this.details == 1){return true;}else{return false;}}

  addPresentation(){
    debugger;
    this.pre.createPresentation(this.userId).subscribe((next)=>{
      this.currentPresentation = next;
      this.details = 1;
    })
  }

}
