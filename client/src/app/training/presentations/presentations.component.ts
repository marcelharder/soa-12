import { Component, Input, OnInit } from '@angular/core';
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
  listOfPresentations:Array<Presentation> = [];
  currentPresentation: Presentation = {
    PresentationId: 0,
    Title: '',
    Media: '',
    Venue: '',
    DatePresented: undefined
  };
  constructor(private pre: PresentationService) { }

  ngOnInit() {
    this.pre.getListOfPresentations(this.userId).subscribe((next)=>{
      this.listOfPresentations = next;
    })

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
