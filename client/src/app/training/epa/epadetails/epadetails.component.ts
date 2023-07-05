import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Epa_model } from 'src/app/_models/Epa_model';

@Component({
  selector: 'app-epadetails',
  templateUrl: './epadetails.component.html',
  styleUrls: ['./epadetails.component.css']
})
export class EpadetailsComponent implements OnInit {

  @Input() selectedEpa: Epa_model = {
    EpaId: 0,
    name: '',
    category: 0,
    year: 0,
    created: new Date,
    image: '',
    Id: 0,
    userId: 0,
    started: new Date,
    finished: new Boolean,
    grade: 0,
    KBP: new Boolean,
    OSATS: new Boolean,
    Beoordeling_360: new Boolean,
    CAT_CAL: new Boolean,
    Examen: new Boolean,
    option_6: new Boolean,
    option_7: new Boolean
  };
  @Output() up = new EventEmitter<Epa_model>();
  
 
   constructor() { }
 
   ngOnInit() {
     
   }
 
  saveEpa(){this.up.emit(this.selectedEpa);}
 

}
