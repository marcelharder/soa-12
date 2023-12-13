import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { dropItem } from 'src/app/_models/dropItem';
import { hospitalValve } from 'src/app/_models/hospitalValve';
import { Valve } from 'src/app/_models/Valve';
import { valveSize } from 'src/app/_models/valveSize';
import { valveType } from 'src/app/_models/valveType';
import { AccountService } from 'src/app/_services/account.service';
import { PatientService } from 'src/app/_services/patient.service';
import { ProcedureService } from 'src/app/_services/procedure.service';
import { ValveService } from 'src/app/_services/valve.service';

@Component({
  selector: 'app-valvedetails',
  templateUrl: './valvedetails.component.html',
  styleUrls: ['./valvedetails.component.css']
})
export class ValvedetailsComponent implements OnInit {

  @Input() hv: hospitalValve;
  @Input() pd: Valve;
  @Output() pushValve = new EventEmitter<Valve>();
 
  @Input() hospitalId: string;

  
  currentProcedureId = 0;


  typeDescription = '';
  
  hospitalValves: Array<hospitalValve> = [];
  optionsTypes: Array<dropItem> = [];
  
  serialNo = "";
  new_valve: Valve = {
    Id: 0,Hospitalno:0,
    Implant_Position: '', IMPLANT: '', EXPLANT: '', SIZE: '', TYPE: '', SIZE_EXP: '',
    TYPE_EXP: 0, ProcedureType: 0, ProcedureAetiology: 0, MODEL: '', MODEL_EXP: '', SERIAL_IMP: '',
    SERIAL_EXP: '', RING_USED: '', valveDescription:'',REPAIR_TYPE: '', Memo: '', Combined: 0, procedure_id: 0
  };
  new_valve_type: valveType = {
    no: 0,
    valveTypeId: 0,
    vendor_description: '',
    vendor_code: '',
    model_code: '',
    implant_position: '',
    uk_code: '',
    code: '',
    valve_size:[],
    image: '',
    description: '',
    type: '',
  };

 
  panel2 = 0;
  panel3 = 0;

  selectedValveTypeId=0;

  


  
 
  valveDescription: string;

  constructor(
    
    private auth: AccountService,
    private alertify: ToastrService,
   
    
    private vs: ValveService) { }

  

  ngOnInit() {
    this.auth.currentProcedure$.pipe(take(1)).subscribe((u) => { this.currentProcedureId = u; });
    this.loadDrops();
    this.panel2 = 1;
  }


  loadDrops() {
    this.optionsTypes.push(
      { value: 1, description: 'Biological' },
      { value: 2, description: 'Mechanical' });
  };

  
  showPanel_2() { if (this.panel2 === 1 || this.pd.Implant_Position === '') { return true; } };
  showPanel_3() { if (this.panel3 === 1) { return true; } };

  

  getModelsInHospital() {
    this.vs.getHospitalValves(this.hv.Type, this.hv.Implant_position).subscribe(
      (next) => { this.hospitalValves = next }, (error) => { this.alertify.error(error) })
  }

  receiveNewValve(nv: Valve) {
    this.pushValve.emit(nv);
  } 

 

 

 

 

  selectThisValve(vtid: number) {
    this.selectedValveTypeId = vtid;
    this.panel2 = 0; this.panel3 = 1;
  }

  

}
