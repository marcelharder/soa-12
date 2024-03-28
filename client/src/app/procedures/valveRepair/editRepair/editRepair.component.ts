import { ThrowStmt } from '@angular/compiler';
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
  selector: 'app-editRepair',
  templateUrl: './editRepair.component.html',
  styleUrls: ['./editRepair.component.css']
})
export class EditRepairComponent implements OnInit {
  @Input() procedureValve: Valve;
  @Output() sendNewProcedureValveUp = new EventEmitter<Valve>();


  optionsAvailableMitralRingsFromOVI: Array<OVIvalve> = [];
  optionsAvailableTricuspidRingsFromOVI: Array<OVIvalve> = [];
  optionsAvailableMitralRings: Array<hospitalValve> = [];
  optionsAvailableTricuspidRings: Array<hospitalValve> = [];

  optionsMitralRepairType: Array<dropItem> = [];
  optionsTricuspidRepairType: Array<dropItem> = [];
  valveDescription = "";
  optionMitralRingSizes: Array<any> = [];
  currentUserId = 0;
  currentHospitalNo = 0;
  oviHospital = false;

  constructor(private drops: DropdownService,
    private valveService: ValveService,
    private user: UserService,
    private auth: AccountService,
    private hos: HospitalService,
    private alertify: ToastrService) { }

  ngOnInit() {
    this.auth.currentUser$.pipe(take(1)).subscribe((u) => { this.currentUserId = u.UserId; });
    this.user.getUser(this.currentUserId).subscribe((next) => {
      this.currentHospitalNo = next.hospital_id;
      this.hos.IsThisHospitalUsingOVI(next.hospital_id).subscribe((r) => { this.oviHospital = r })
    });
    this.loadDrops();
    this.getRingSizes();
    if (this.procedureValve.RING_USED === 'true') {
      // show the ring entry area, done by showExistingRing()

      //this.getThisOne(this.procedureValve.Id);
      this.ringChanged();



    }


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

  getRingSizes() {
    // get the correct hospitalvalve from the model
    this.valveService.getSpecificHospitalValveByModelCode(this.procedureValve.MODEL).subscribe(
      (next) => {

        if (this.procedureValve.MODEL !== null) {
          this.valveService.getValveCodeSizes(next.ValveTypeId).subscribe(
            (next) => {
              this.optionMitralRingSizes = next;
            },
            (error) => { this.alertify.error(error) })
        }
      }
    )


  }



  ringChanged() {
    if (this.displayMVP()) {
      let help: Partial<hospitalValve> = {};
      if (this.displayOVIStatus()) {
        this.getListOfProducts(true, 1, help);
      } else {
        this.getListOfProducts(false, 1, help);
      }
    }
    if (this.displayTVP()) {
      let help: Partial<hospitalValve> = {};
      if (this.displayOVIStatus()) {
        this.getListOfProducts(true, 2, help);
      } else {
        this.getListOfProducts(false, 2, help);
      }
    }

    this.alertify.show("changed")
  }

  cancelEditRepair() { }
  saveRepair() {// save stuff if there is no ring
    this.sendNewProcedureValveUp.emit(this.procedureValve);
  }

  getThisOne(valveId: number) {
    if (this.procedureValve.Implant_Position === "Mitral") {
      let selectedOVIRing = this.optionsAvailableMitralRingsFromOVI.find(x => x.valveId === valveId);
      this.procedureValve = this.transferValues(this.procedureValve, selectedOVIRing);
      this.procedureValve.Implant_Position = "Mitral";
    }
    if (this.procedureValve.Implant_Position === "Tricuspid") {
      let selectedOVIRing = this.optionsAvailableTricuspidRingsFromOVI.find(x => x.valveId === valveId);
      this.procedureValve = this.transferValues(this.procedureValve, selectedOVIRing);
      this.procedureValve.Implant_Position = "Tricuspid";
    }
    this.sendNewProcedureValveUp.emit(this.procedureValve);

  }
  receiveCompletedValve(x: Valve) {
    x.RING_USED = 'true';
    if (this.procedureValve.Implant_Position === "mvp") { x.Implant_Position = "Mitral"; }
    if (this.procedureValve.Implant_Position === "tvp") { x.Implant_Position = "Tricuspid"; }
    this.sendNewProcedureValveUp.emit(x);
  }

  showExistingRing() { if (this.procedureValve.RING_USED === "true") { return true } }
  displayOVIStatus() { return this.oviHospital; }
  displayMVP() { if (this.procedureValve.Implant_Position === 'Mitral') { return true; } }
  displayTVP() { if (this.procedureValve.Implant_Position === 'Tricuspid') { return true; } }

  private transferValues(proc: Valve, sel: OVIvalve): Valve {
    proc.valveDescription = sel.description;
    this.valveDescription = sel.description;
    proc.RING_USED = "true";
    proc.TYPE = "Annuloplasty_Ring";
    proc.MODEL = sel.product_code;
    proc.SERIAL_IMP = sel.serial_no;
    proc.SIZE = sel.size.toString();
    return proc
  }

  private getListOfProducts(ovi: boolean, location: number, help: Partial<hospitalValve>) {
    if (ovi) { // look in the online registry
      if (location === 1) { // mitral position
        help.hospitalId = this.currentHospitalNo.toString();
        help.Type = "4";
        help.Implant_position = "2";
        this.alertify.info("Getting rings from the OVI")
        this.valveService.getValvesFromOVI(help).subscribe((next) => {
          this.optionsAvailableMitralRingsFromOVI = next;
        })
      } else { // tricuspid position
        help.hospitalId = this.currentHospitalNo.toString();
        help.Type = "4";
        help.Implant_position = "4";
        this.alertify.info("Getting rings from the OVI")
        this.valveService.getValvesFromOVI(help).subscribe((next) => {
          this.optionsAvailableTricuspidRingsFromOVI = next;
        })
      }

    } else { // look in the current hospital
      if (location === 1) { // mitral position
        this.alertify.info("Getting rings your hospital");
        help.Type = "Annuloplasty_Ring"
        help.Implant_position = "Mitral";
        this.valveService.getHospitalValves(help.Type, help.Implant_position).subscribe(
          (next) => { this.optionsAvailableMitralRings = next; },
          (error) => { this.alertify.warning(error) });
      } else { // tricuspid position
        this.alertify.info("Getting rings from your hospital");
        help.Type = "Annuloplasty_Ring"
        help.Implant_position = "Tricuspid";
        this.valveService.getHospitalValves(help.Type, help.Implant_position).subscribe(
          (next) => { this.optionsAvailableTricuspidRings = next; },
          (error) => { this.alertify.warning(error) });
      }
    }
  }

}
