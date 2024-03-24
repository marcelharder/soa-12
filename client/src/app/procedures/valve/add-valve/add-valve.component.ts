import { Component, EventEmitter, Input, OnInit, Output, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { hospitalValve } from 'src/app/_models/hospitalValve';
import { Valve } from 'src/app/_models/Valve';
import { valveSize } from 'src/app/_models/valveSize';
import { valveType } from 'src/app/_models/valveType';
import { AccountService } from 'src/app/_services/account.service';
import { HospitalService } from 'src/app/_services/hospital.service';
import { PatientService } from 'src/app/_services/patient.service';
import { ProcedureService } from 'src/app/_services/procedure.service';
import { UserService } from 'src/app/_services/user.service';
import { ValveService } from 'src/app/_services/valve.service';

@Component({
  selector: 'app-add-valve',
  templateUrl: './add-valve.component.html',
  styleUrls: ['./add-valve.component.css']
})
export class AddValveComponent implements OnInit {
  modalRef: BsModalRef;
  @Input() svtid: number;
  @Input() implant_position: string;
  @Output() pushValve = new EventEmitter<Valve>();
  
  noPPM = false;
  moderatePPM = false;
  severePPM = false;

  ppmAdvice = 0;
  valveSize = "Choose";

  adviceText = "Hier komt de advice text";

  optionSizes: Array<valveSize> = [];

  currentProcedureId = 0;
  currentUserId = 0;
  currentHospitalNo = "";
  valveDescription = "";
  
  new_valve: Valve = {
    Id: 0,Hospitalno:0,
    Implant_Position: '', IMPLANT: '', EXPLANT: '', SIZE: '', TYPE: '', SIZE_EXP: '',
    TYPE_EXP: 0, ProcedureType: 0, ProcedureAetiology: 0, MODEL: '', MODEL_EXP: '', SERIAL_IMP: '',
    SERIAL_EXP: '', RING_USED: '', valveDescription:'',REPAIR_TYPE: '', Memo: '', Combined: 0, procedure_id: 0
  };
  pd: Valve = {
    Id: 0,Hospitalno:0,
    Implant_Position: '', IMPLANT: '', EXPLANT: '', SIZE: '', TYPE: '', SIZE_EXP: '',
    TYPE_EXP: 0, ProcedureType: 0, ProcedureAetiology: 0, MODEL: '', MODEL_EXP: '', SERIAL_IMP: '',
    SERIAL_EXP: '', RING_USED: '',  valveDescription:'',REPAIR_TYPE: '', Memo: '', Combined: 0, procedure_id: 0
  };
  new_valve_type: hospitalValve = {
    ValveTypeId: 0,
    hospitalId: '',
    No: 0,
    Vendor_description: '',
    Vendor_code: 0,
    Valve_size: undefined,
    Model_code: '',
    Implant_position: '',
    uk_code: '',
    soort: 0,
    image: '',
    Description: '',
    Type: '',
    countries: ''
  };

  constructor(
    private modalService: BsModalService,
    private auth: AccountService,
    private proc: ProcedureService,
    private alertify: ToastrService,
    private userService: UserService,
    private patient: PatientService,
    private hos: HospitalService,
    private vs: ValveService) { }

  ngOnInit() {
    this.auth.currentProcedure$.pipe(take(1)).subscribe((u) => { this.currentProcedureId = u; });
    this.auth.currentUser$.pipe(take(1)).subscribe((u) => { this.currentUserId = u.UserId; })
    let h = '';
    this.valveDescription = '';
    this.userService
      .getUser(this.currentUserId)
      .subscribe((next) => {
        this.currentHospitalNo = next.hospital_id.toString();
        var index = next.hospital_id;
       });

     // lookup the details of this type of prosthesis
     this.vs.getSpecificHospitalValve(this.svtid.toString()).subscribe((next) => {
     
      this.new_valve_type = next;
      this.valveDescription = this.new_valve_type.Description;
      this.pd.MODEL = this.new_valve_type.uk_code; // needed for EOA measurement
      this.pd.SERIAL_IMP = '';
      
      this.vs.getValveCodeSizes(this.svtid.toString()).subscribe((nex) => {
        this.optionSizes = [];
        nex.forEach((item) => {
          this.optionSizes.push({ SizeId:0, Size: item.Size, EOA: item.EOA,VTValveTypeId: 0,ValveTypeId:0 });
        },(error)=>{
          this.alertify.error(error)});
          });
 
    })
  }

  confirm(): void {
    
    this.vs.addValveInProcedure(this.pd.SERIAL_IMP, this.currentProcedureId).subscribe((nex) => {
      
        this.new_valve = nex;
        this.new_valve.SERIAL_IMP = this.pd.SERIAL_IMP;
        this.new_valve.MODEL = this.new_valve_type.uk_code;
        this.new_valve.TYPE = this.new_valve_type.Type;
        this.new_valve.Implant_Position = this.implant_position;
        this.new_valve.SIZE = this.valveSize;
        this.new_valve.procedure_id = this.currentProcedureId;
        this.new_valve.Combined = this.pd.Combined;
        this.new_valve.ProcedureType = this.pd.ProcedureType;
        this.new_valve.ProcedureAetiology = this.pd.ProcedureAetiology;
        this.new_valve.EXPLANT = this.pd.EXPLANT;
        this.pd.Implant_Position = this.new_valve.Implant_Position; // will hide panel2
        this.pd.TYPE = this.new_valve_type.Type;
        this.pd.SIZE = this.valveSize;
        
        this.pushValve.emit(this.new_valve);

    });
    this.modalRef?.hide();
  }

  decline(): void {
     this.modalRef?.hide();
  }

  implantValve(template: TemplateRef<any>) {
    if (this.pd.SERIAL_IMP == "") {
      this.alertify.warning("Please enter serial number of this valve ...")
    } else {
      this.modalRef = this.modalService.show(template);
    };
  }

  showEoaAdvice() {
    if (this.implant_position === 'Aortic') { if (this.ppmAdvice === 1) { return true; } } else { return false; }
  }

  findEOA() {

   if (this.implant_position === 'Aortic') { // give only advice about aortic valves
      let procedureId = 0;
      let patientId = 0;
      let height = 0;
      let weight = 0;
      let eoa = 0;
      let eoai = 0;
      

      this.proc.getProcedure(this.currentProcedureId).subscribe((next) => {
        patientId = next.patientId;
        this.patient.getPatientFromId(patientId).subscribe((next) => {
          height = next.height;
          weight = next.weight;
          this.vs.getPPM(this.pd.MODEL, this.valveSize, weight.toString(), height.toString()).subscribe((next) => {
            this.adviceText = "You can expect " + next.body + " PPM";
            if (next.body === 'no') { this.noPPM = true; this.moderatePPM = false; this.severePPM = false; }
            if (next.body === 'moderate') { this.noPPM = false; this.moderatePPM = true; this.severePPM = false; }
            if (next.body === 'severe') { this.noPPM = false; this.moderatePPM = false; this.severePPM = true; }
          }, (error) => { this.adviceText = error; })
        })
      })


      this.alertify.show("Finding EOAi of valve " + this.pd.MODEL + "  with size " + this.valveSize);
      this.ppmAdvice = 1;

    }
  }

}
