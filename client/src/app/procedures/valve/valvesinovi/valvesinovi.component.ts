import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { dropItem } from 'src/app/_models/dropItem';
import { hospitalValve } from 'src/app/_models/hospitalValve';
import { OVIvalve } from 'src/app/_models/OVIvalve';
import { Valve } from 'src/app/_models/Valve';
import { AccountService } from 'src/app/_services/account.service';
import { PatientService } from 'src/app/_services/patient.service';
import { ProcedureService } from 'src/app/_services/procedure.service';
import { ValveService } from 'src/app/_services/valve.service';

@Component({
  selector: 'app-valvesinovi',
  templateUrl: './valvesinovi.component.html',
  styleUrls: ['./valvesinovi.component.css']
})
export class ValvesinoviComponent implements OnInit  {
  @Input() hospitalId: number;
  @Input() implant: string;
  @Input() pd: Valve;
  @Output() pushValve = new EventEmitter<Valve>();
 
  _addValveIndicator: boolean = false;
  currentProcedureId = 0;
  adviceText = "";
  valveImageUrl: string = "";
  
  noPPM:boolean = false;
  moderatePPM:boolean = false;
  severePPM:boolean = false;

  new_valve: Valve = {
    Id: 0,Hospitalno:0,
    Implant_Position: '', IMPLANT: '', EXPLANT: '', SIZE: '', TYPE: '', SIZE_EXP: '',
    TYPE_EXP: 0, ProcedureType: 0, ProcedureAetiology: 0, MODEL: '', MODEL_EXP: '', SERIAL_IMP: '',
    SERIAL_EXP: '', RING_USED: '', valveDescription:'', REPAIR_TYPE: '', Memo: '', Combined: 0, procedure_id: 0
  };
  model: any = {};
  selectedSoort = 0;
  optionsAvailableValves:Array<Partial<Valve>> = [];
  
  optionsSoort:Array<dropItem> = [
  {value:1,description:"Mechanical"},
  {value:2,description:"Biological"}];

  optionsPosition:Array<dropItem> = [ 
  {value:0,description:"Choose"},
  {value:1,description:"Aortic"},
  {value:2,description:"Mitral"}];
  description: string;

 

  constructor(
    private vs:ValveService, 
    private alertify:ToastrService, 
    private router: Router,
    private proc: ProcedureService,
    private patient: PatientService,
    private auth: AccountService) { }

  ngOnInit() {
  this.auth.currentProcedure$.pipe(take(1)).subscribe((u) => { this.currentProcedureId = u; });
 

  this.selectedSoort = 1;

  this.model.Type = 1;
  this.model.Implant_position = this.getImplantValue(this.implant);
  this.model.hospitalId = this.hospitalId;
  
  this.vs.getValvesFromOVI(this.model).subscribe((next)=>{
    this.optionsAvailableValves = next;
 
  })
}
getThisOne(v: OVIvalve){
  this._addValveIndicator = true;

  this.givePPM_Advice(v.product_code, v.size.toString());

  
  this.valveImageUrl = v.image;
  this.new_valve.Hospitalno = this.hospitalId;
  this.new_valve.procedure_id = this.currentProcedureId;
  this.new_valve.Implant_Position = v.implant_position;
  this.new_valve.SERIAL_IMP = v.serial_no;
  this.new_valve.TYPE = v.type;
  this.new_valve.SIZE = v.size.toString();
  this.new_valve.MODEL = v.product_code;
  this.description = v.description;

  this.vs.markValve(this.new_valve.SERIAL_IMP, 1, this.new_valve.procedure_id)
  .subscribe((next)=>{this.alertify.show(next)})

}



findSelectedValves(){
  this.model.Type = this.selectedSoort;
  this.model.Implant_position = this.getImplantValue(this.implant);
  this.model.hospitalId = this.hospitalId;
  debugger;
  this.vs.getValvesFromOVI(this.model).subscribe((next)=>{this.optionsAvailableValves = next;})
}

cancel(){this._addValveIndicator = false;}

save(){this.pushValve.emit(this.new_valve);}

getImplantValue(implant: string): number {
   let help = 0;
  // implant is eg. Aortic comes from the input
  let d = this.optionsPosition.findIndex(x => x.description === implant);
  help = this.optionsPosition[d].value;
  return help;
}


  
 

showAddedValve(){return this._addValveIndicator;}


givePPM_Advice(productCode: string, size: string) {
  if (this.implant === 'Aortic') { // give only advice about aortic valves
    let procedureId = 0;
    let patientId = 0;
    let height = 0;
    let weight = 0;
    let eoa = 0;
    let eoai = 0;


    this.proc.getProcedure(this.currentProcedureId).subscribe((next) => {
      let patient_id = next.patientId;
      this.patient.getPatientFromId(patient_id).subscribe((next) => {
        let weight = next.weight;
        let height = next.height;
        this.vs.getPPM(productCode, size, weight.toString(), height.toString()).subscribe((next) => {
          
          if (next.body === 'no') { 
            this.noPPM = true; this.moderatePPM = false; this.severePPM = false;
            this.adviceText = "This valve will give no Patient Prosthesis Mismatch";
          }
          if (next.body === 'moderate') { 
            this.noPPM = false; this.moderatePPM = true; this.severePPM = false;
            this.adviceText = "This valve will give moderate Patient Prosthesis Mismatch";
          }
          if (next.body === 'severe') { 
            this.noPPM = false; this.moderatePPM = false; this.severePPM = true;
            this.adviceText = "This valve will give severe Patient Prosthesis Mismatch";
          }
        }, (error) => { this.adviceText = error; })
      });
    });
  }
}
 
}



