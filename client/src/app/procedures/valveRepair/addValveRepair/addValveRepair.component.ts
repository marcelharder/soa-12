import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { dropItem } from 'src/app/_models/dropItem';
import { hospitalValve } from 'src/app/_models/hospitalValve';
import { OVIvalve } from 'src/app/_models/OVIvalve';
import { Valve } from 'src/app/_models/Valve';
import { AccountService } from 'src/app/_services/account.service';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { HospitalService } from 'src/app/_services/hospital.service';
import { UserService } from 'src/app/_services/user.service';
import { ValveService } from 'src/app/_services/valve.service';


@Component({
  selector: 'app-addValveRepair',
  templateUrl: './addValveRepair.component.html',
  styleUrls: ['./addValveRepair.component.css']
})
export class AddValveRepairComponent implements OnInit {
 @Input() Implant_Location = "";
 @Input() procedureValve:Valve;
 @Output() sendNewProcedureValveUp = new EventEmitter<Valve>();

 
 ringCheck = false;
 existingRepair = false;
 existingRing = false;
 valveDescription ="";
 oviHospital = false;
 currentUserId: number;
 currentHospitalNo: number;

 optionsAvailableMitralRingsFromOVI: Array<OVIvalve> = [];
 optionsAvailableTricuspidRingsFromOVI: Array<OVIvalve> = [];
 optionsAvailableMitralRings: Array<hospitalValve> = [];
 optionsAvailableTricuspidRings: Array<hospitalValve> = [];

 optionsMitralRepairType: Array<dropItem> = [];
 optionsTricuspidRepairType: Array<dropItem> = [];
 

  constructor(
    private auth: AccountService,
    private hos: HospitalService,
    private vs: ValveService,
    private user: UserService,
    private drops: DropdownService,
    private alertify: ToastrService) { }

  ngOnInit() { 
    this.auth.currentUser$.pipe(take(1)).subscribe((u) => { this.currentUserId = u.UserId; });
    this.user.getUser(this.currentUserId).subscribe((next) => {
      this.currentHospitalNo = next.hospital_id;
      this.hos.IsThisHospitalUsingOVI(next.hospital_id).subscribe((r) => { this.oviHospital = r })
    });
    this.loadDrops();

  }
  loadDrops() {
    let d = JSON.parse(localStorage.getItem('MitralRepairType'));
    if (d == null || d.length === 0) {
      this.drops.getMitralValveRepair().subscribe((response) => {
        this.optionsMitralRepairType = response;
        localStorage.setItem('MitralRepairType', JSON.stringify(response));
      });
    } else {
      this.optionsMitralRepairType = JSON.parse(localStorage.getItem('MitralRepairType')
      );
    }
    d = JSON.parse(localStorage.getItem('TricuspidRepairType'));
    if (d == null || d.length === 0) {
      this.drops.getTricuspidValveRepair().subscribe((response) => {
        this.optionsTricuspidRepairType = response;
        localStorage.setItem('TricuspidRepairType', JSON.stringify(response));
      });
    } else {
      this.optionsTricuspidRepairType = JSON.parse(localStorage.getItem('TricuspidRepairType')
      );
    }

  }
  displayTVP(){if(this.Implant_Location === 'tvp'){return true;}}
  displayMVP(){if(this.Implant_Location === 'mvp'){return true;}}
  displayRingUsed(){if(this.ringCheck){return true;}}
  displayOVIStatus(){return this.oviHospital;}


  getThisOne(valveId: number) { // this comes from OVI
   
    if (this.Implant_Location === "mvp") {
      let selectedOVIRing = this.optionsAvailableMitralRingsFromOVI.find(x => x.valveId === valveId);
      this.procedureValve = this.transferValues(this.procedureValve, selectedOVIRing);
      this.procedureValve.Implant_Position = "Mitral";
    }
    if (this.Implant_Location === "tvp") {
      let selectedOVIRing = this.optionsAvailableTricuspidRingsFromOVI.find(x => x.valveId === valveId);
      this.procedureValve = this.transferValues(this.procedureValve, selectedOVIRing);
      this.procedureValve.Implant_Position = "Tricuspid";
    }
    if (this.displayRingUsed()) { 
      // mark the valve as implanted in the OVI
      this.vs.markValve(this.procedureValve.SERIAL_IMP, 1, this.procedureValve.procedure_id).subscribe(
        (next)=>{this.alertify.show(next)})
      this.procedureValve.RING_USED = 'true'; } 
      else { this.procedureValve.RING_USED = 'false'; }
    //this.vs.updateValve(this.procedureValve).subscribe((next) => { this.alertify.info(next) })
    this.sendNewProcedureValveUp.emit(this.procedureValve);
  } 
  receiveFullProcedure(x: Valve){ // now this comes from the valveFromHospital 
    x.RING_USED = 'true';
    if (this.Implant_Location === "mvp"){x.Implant_Position = "Mitral";}
    if (this.Implant_Location === "tvp"){x.Implant_Position = "Tricuspid";}
    this.sendNewProcedureValveUp.emit(x);
  }
  saveRepairNoRing(){
    this.procedureValve.RING_USED = 'false';
    if (this.Implant_Location === "mvp"){this.procedureValve.Implant_Position = "Mitral";}
    if (this.Implant_Location === "tvp"){this.procedureValve.Implant_Position = "Tricuspid";}
    this.sendNewProcedureValveUp.emit(this.procedureValve);
  }

  
  
  onCheckBoxChange(test: any) {

    if (this.displayMVP()) {
      if (test.target.id === 'customCheckDetails') { if (!test.target.checked) { this.procedureValve.Memo = ''; } }

      if (test.target.id === 'customCheck') {
          let help: Partial<hospitalValve> = {};

          if (this.displayOVIStatus()) {// get the rings from OVI
            help.hospitalId = this.currentHospitalNo.toString();
            help.soort = 4;
            help.Implant_position = "2";
            this.alertify.info("Getting rings from the OVI")
            this.vs.getValvesFromOVI(help).subscribe((next) => {
              this.optionsAvailableMitralRingsFromOVI = next;
            })
          } else {// get the rings from the current hospital
            this.alertify.info("Getting rings from the hospital");
            help.Type = "Annuloplasty_Ring"
            help.Implant_position = "Mitral";
            this.vs.getHospitalValves(help.Type, help.Implant_position).subscribe(
              (next) => { this.optionsAvailableMitralRings = next; },
              (error) => { this.alertify.warning(error) });
          }
        
      }
    }
    if (this.displayTVP()) {
      

      if (test.target.id === 'customCheckDetails') { if (!test.target.checked) { this.procedureValve.Memo = ''; } }

      if (test.target.id === 'customCheck') {
               
        let help: Partial<hospitalValve> = {};
        if (this.displayOVIStatus()) {// get the rings from OVI
          help.hospitalId = this.currentHospitalNo.toString();
          help.soort = 4;
          help.Implant_position = "4";
          this.alertify.info("Getting rings from the OVI")
          this.vs.getValvesFromOVI(help).subscribe((next) => {
            this.optionsAvailableTricuspidRingsFromOVI = next;
          })
        } else {// get the rings from the current hospital
          this.alertify.info("Getting rings from the hospital");
          help.hospitalId = this.currentHospitalNo.toString();
          help.soort = 4;
          help.Type = "Annuloplasty_Ring"
          help.Implant_position = "Tricuspid";
          this.vs.getHospitalValves(help.Type, help.Implant_position).subscribe(
            (next) => {this.optionsAvailableTricuspidRings = next; },
            (error) => {this.alertify.warning(error) });
        }
      }

    }
  }
  private transferValues(proc: Valve, sel: OVIvalve): Valve {
    proc.valveDescription = sel.description;
    this.valveDescription = sel.description;
    proc.RING_USED = "true";
    proc.TYPE = "Annuloplasty_Ring";
    proc.MODEL = sel.product_code;
    proc.SERIAL_IMP = sel.serial_no;
    proc.SIZE = sel.size.toString();
    return proc;
  }
  
}
