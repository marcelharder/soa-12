import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Epa_model } from 'src/app/_models/Epa_model';
import { AccountService } from 'src/app/_services/account.service';
import { EpaService } from 'src/app/_services/epa.service';

@Component({
  selector: 'app-epa',
  templateUrl: './epa.component.html',
  styleUrls: ['./epa.component.css']
})
export class EpaComponent implements OnInit {
  @Input() userId:number;
  @Input() origin:string;
  d = 0;
  epa_no_description = "";
  description: any = [];
  values: Array<Epa_model> = [];
  selected_epa:Epa_model ={
    EpaId: 0,
    name: '',
    category: 0,
    year: 0,
    created: new Date,
    image: '',
    Id: 0,
    userId: 0,
    started: new Date,
    finished: false,
    grade: 0,
    KBP: false,
    OSATS: false,
    Beoordeling_360: false,
    CAT_CAL: false,
    Examen: false,
    option_6: false,
    option_7: false
  }
  
  
    constructor(private epa: EpaService, private account: AccountService, private alertify:ToastrService) { }
  
    ngOnInit() {
      // get the descriptions
      this.epa.getDescriptions().subscribe((response)=>{ this.description = response;});
      
      // if this is opnened by the chef then use that userId else get current user
      if(this.origin === '1'){
        this.epa.getEpas(this.userId).subscribe((next)=>{this.values = next;});}
      else {
      // get the values for this patient
      this.account.currentUser$.subscribe((next)=>{this.userId = next.UserId;});
      if(this.userId !== undefined){this.epa.getEpas(this.userId).subscribe((next)=>{this.values = next;});
      };
      }
      
      
      
     
      
     
  
    }
    showDetailsPage(){if(this.d === 1)return true; else return false;}
  
    showDetails(id: number){
      this.d = 1;
      this.epa_no_description = "Epa # " + this.values[id].Id + " " + this.description[id].Definition;
      this.selected_epa = this.values[id];
    } 
    getBackFromDetails(changed_epa: Epa_model){
      // update de selected epa
      this.selected_epa = changed_epa;
      // save to the database
      this.epa.updateEpa(this.selected_epa).subscribe((next)=>{this.alertify.info("Updated");});
      // show the list again
      this.d = 0;
    }

}
