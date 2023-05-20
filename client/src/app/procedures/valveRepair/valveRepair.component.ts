import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { dropItem } from 'src/app/_models/dropItem';
import { hospitalValve } from 'src/app/_models/hospitalValve';
import { OVIvalve } from 'src/app/_models/OVIvalve';
import { Valve } from 'src/app/_models/Valve';
import { valveSize } from 'src/app/_models/valveSize';
import { AccountService } from 'src/app/_services/account.service';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { HospitalService } from 'src/app/_services/hospital.service';
import { ProcedureService } from 'src/app/_services/procedure.service';
import { UserService } from 'src/app/_services/user.service';
import { ValveService } from 'src/app/_services/valve.service';

@Component({
  selector: 'app-valveRepair',
  templateUrl: './valveRepair.component.html',
  styleUrls: ['./valveRepair.component.css']
})
export class ValveRepairComponent implements OnInit {
  @ViewChild('valveRepairForm') valveRepairForm: NgForm;
  currentUserId = 0;
  currentHospitalNo = 0;
  currentProcedureId = 0;
  oviHospital = false;
  existingRepair = false;

  editCard = false;
  ringCheck = false;
  tricuspidRingUsed = false;
  pd: Array<Valve> = [];
  h = "";
  procedureValve: Valve = {
    Id: 0,
    Implant_Position: '', IMPLANT: '', EXPLANT: '', SIZE: '', TYPE: '', SIZE_EXP: '',
    TYPE_EXP: 0, ProcedureType: 0, ProcedureAetiology: 0, MODEL: '', MODEL_EXP: '', SERIAL_IMP: '',
    SERIAL_EXP: '', RING_USED: '', REPAIR_TYPE: '', Memo: '', Combined: 0, procedure_id: 0, Hospitalno: 0, valveDescription: ''
  };
  title = '';
  valveDescription = '';
 
  optionsAvailableMitralRings: Array<hospitalValve> = [];
  optionsAvailableMitralRingsFromOVI: Array<OVIvalve> = [];
  optionsAvailableTricuspidRings: Array<hospitalValve> = [];
  optionsAvailableTricuspidRingsFromOVI: Array<OVIvalve> = [];
  optionMitralRingSizes: Array<valveSize> = [];
  optionTricuspidRingSizes: Array<valveSize> = [];

  constructor(
    private alertify: ToastrService,
    private auth: AccountService,
    private userService: UserService,
    private drops: DropdownService,
    private procedureService: ProcedureService,
    private valveService: ValveService,
    private hos: HospitalService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.auth.currentUser$.pipe(take(1)).subscribe((u) => { this.currentUserId = u.UserId; });
    this.auth.currentProcedure$.pipe(take(1)).subscribe((u) => { this.currentProcedureId = u; });
    this.auth.currentSoortProcedure.subscribe((next) => { this.h = next; });

    this.userService.getUser(this.currentUserId).subscribe((next) => {
      this.currentHospitalNo = next.hospital_id;
      this.hos.IsThisHospitalUsingOVI(next.hospital_id).subscribe((r) => { this.oviHospital = r })
    })
    this.route.data.subscribe((data) => {
      
      if (data.valve.length > 0) { // there are repairs recorded for this procedure_id, because the resolver only finds repairs
        this.existingRepair = true;
        this.pd = data.valve;

        //get the index of this list, which are all valve repairs
        if (this.h === 'mvp') {
          const index = this.pd.findIndex(a => a.Implant_Position === 'Mitral'); this.procedureValve = this.pd[index];
        } else {
          if (this.h === 'tvp') {
            const index = this.pd.findIndex(a => a.Implant_Position === 'Tricuspid'); this.procedureValve = this.pd[index];

          }
        }
      } else { // there is no repair recorded for this procedure_id
        this.existingRepair = false;
        if (this.h === 'mvp') {// create a new Annuloplasty Ring
          this.valveService.addValveRepairInProcedure('Mitral', this.currentProcedureId).subscribe(
            (response) => { this.procedureValve = response; });
        }
        else {
          this.valveService.addValveRepairInProcedure('Tricuspid', this.currentProcedureId).subscribe(
            (response) => { this.procedureValve = response; });
        }
      }
    })
    

  }


  saveRepair() {
   
    this.valveService.updateValve(this.procedureValve).subscribe((next) => { this.alertify.info(next) });
    
    this.editCard = false;
    this.existingRepair = true;
  }
  cancelEditRepair() { this.editCard = false; }

  receiveCompletedValve(x: Valve){
     this.procedureValve = x;
    this.saveRepair();
  }

  
 
  

  displayExistingRepair() { return this.existingRepair; }
  displayOVIStatus() { return this.oviHospital; }
  displayTVP() { if (this.h === "tvp") { return true; } }
  displayMVP() { if (this.h === "mvp") { return true; } }
  



  canDeactivate() {
    this.saveRepair();
    this.alertify.show('saving ValveRepair');
    return true;
  }
  receiveCompletedValveId(gift: Valve) {
    this.procedureValve = gift;
    this.existingRepair = true;
  }

  


}
