import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { dropItem } from 'src/app/_models/dropItem';
import { hospitalValve } from 'src/app/_models/hospitalValve';
import { Valve } from 'src/app/_models/Valve';
import { AccountService } from 'src/app/_services/account.service';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { HospitalService } from 'src/app/_services/hospital.service';
import { UserService } from 'src/app/_services/user.service';
import { ValveService } from 'src/app/_services/valve.service';

@Component({
  selector: 'app-valve',
  templateUrl: './valve.component.html',
  styleUrls: ['./valve.component.css']
})
export class ValveComponent implements OnInit {
  @ViewChild('valveDetailsForm') valveForm: NgForm;
  pd: Array<Valve> = [];

  procedureValve: Valve = {
    Id: 0, Hospitalno:0,Implant_Position: '', IMPLANT: '', EXPLANT: '', SIZE: '', TYPE: '', SIZE_EXP: '',
    TYPE_EXP: 0, ProcedureType: 0, ProcedureAetiology: 0, MODEL: '', MODEL_EXP: '', SERIAL_IMP: '',
    SERIAL_EXP: '', RING_USED: '', valveDescription:'',REPAIR_TYPE: '', Memo: '', Combined: 0, procedure_id: 0
  };
  hv: hospitalValve = {
    Implant_position: "Aortic",
    ValveTypeId: 0,
    hospitalId: '',
    No: 0,
    Vendor_description: '',
    Vendor_code: 0,
    Valve_size: undefined,
    Model_code: '',
    uk_code: '',
    soort: 1,
    image: '',
    Description: '',
    Type: '',
    countries: ''
  };

  currentHospitalNo = '';
  currentUserId = 0;

  avr = 0;
  mvr = 0;
  tvr = 0;
  explanted = 1;
  markImplanted = 0;

  saveAlways = true;

  YN: Array<dropItem> = [];
  optionsAetiology: Array<dropItem> = [];
  optionsAorticProcedure: Array<dropItem> = [];
  optionsMitralProcedure: Array<dropItem> = [];
  optionsAorticTypes: Array<dropItem> = [];
  optionsValveCodes: Array<dropItem> = [];
  optionsAorticSizes: Array<string> = [];
  optionsMitralSizes: Array<string> = [];
  optionsModelsInHospital: Array<dropItem> = [];
  private _showValvePresent: boolean = false;
  private _showAddValve: boolean = false;
  private _oviHospital: boolean = false;
  private _showSelectValve: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private hos: HospitalService,
    private drops: DropdownService,
    private auth: AccountService,
    private valveService: ValveService,
    private alertify: ToastrService,
    private userService: UserService
  ) { }

  ngOnInit() {
    this.auth.currentUser$.pipe(take(1)).subscribe((u) => { this.currentUserId = u.UserId; })
    let h = '';
    this.userService
      .getUser(this.currentUserId)
      .subscribe((next) => {
        this.currentHospitalNo = next.hospital_id.toString();
        var index = next.hospital_id;
        // find out if I can show the available valves in the participating hospital
        this.hos.IsThisHospitalUsingOVI(index).subscribe((next) => { if (next) {this._oviHospital = true; } })
      });

    this.route.data.subscribe((data) => {
          if (data.valve.length > 0) {
            
            this.pd = data.valve;
            this._showValvePresent = true;
            this.auth.currentSoortProcedure.subscribe((res) => {
              h = res;
              switch (res) {
                case "avr": this.hv.Implant_position = 'Aortic'; this.avr = 1;

                  const index = this.pd.findIndex(a => a.Implant_Position === 'Aortic');
                  if (index === -1) { }
                  else {
                    let procedure_id = this.pd[index].procedure_id;
                    this.valveService.getValveFromSerial(this.pd[index].SERIAL_IMP, procedure_id).subscribe((next) => {
                      this.procedureValve = next;
                   });

                  }; break;
                case "mvr": this.hv.Implant_position = 'Mitral'; this.mvr = 1;
                  const index_2 = this.pd.findIndex(a => a.Implant_Position === 'Mitral');
                  if (index_2 === -1) { }
                  else {
                    let procedure_id = this.pd[index_2].procedure_id;
                    this.valveService.getValveFromSerial(this.pd[index_2].SERIAL_IMP, procedure_id).subscribe((next) => {
                      this.procedureValve = next;
                     });

                  }; break;
                case "tvr": this.hv.Implant_position = 'Tricuspid'; this.tvr = 1;
                  const index_3 = this.pd.findIndex(a => a.Implant_Position === 'Tricuspid');
                  if (index_3 === -1) { }
                  else {
                    let procedure_id = this.pd[index_3].procedure_id;
                    this.valveService.getValveFromSerial(this.pd[index_3].SERIAL_IMP, procedure_id).subscribe((next) => {
                      this.procedureValve = next;
                    });
                  }; break;


              };
             
            });
          } else {
            // er is geen valve gevonden voor deze patient
           
            this.auth.currentSoortProcedure.subscribe((next) => {
              h = next;
              if (h === 'avr') { this.hv.Implant_position = 'Aortic'; this.avr = 1; }
              if (h === 'mvr') { this.hv.Implant_position = 'Mitral'; this.mvr = 1; }
              if (h === 'tvr') { this.hv.Implant_position = 'Tricuspid'; this.tvr = 1; }
            });
          }
        });

      
    this.loadDrops();
  }
  // this is used to show the different procedureTypes and aetiology
  showAVR() { if (this.avr === 1) { return true; } }
  showMVR() { if (this.mvr === 1) { return true; } }
  showTVR() { if (this.tvr === 1) { return true; } }

  showAddValve(){return this._showAddValve;}
  showValvePresent(){return this._showValvePresent;}
  IsOVIHospital() { return this._oviHospital;}
  selectValve(){if(this.readyToImplant(this.procedureValve)){this._showSelectValve = true;}}
  

  loadDrops() {
    for (let x = 16; x < 31; x++) { this.optionsAorticSizes.push(x.toString()); }
    for (let x = 21; x < 37; x++) { this.optionsMitralSizes.push(x.toString()); }

    let d = JSON.parse(localStorage.getItem('YN'));
    // tslint:disable-next-line: max-line-length
    if (d == null || d.length === 0) {
      this.drops.getYNOptions().subscribe((response) => { this.YN = response; localStorage.setItem('YN', JSON.stringify(response)); });
    } else {
      this.YN = JSON.parse(localStorage.getItem('YN'));
    }
    d = JSON.parse(localStorage.getItem('optionsAorticProcedure'));
    // tslint:disable-next-line: max-line-length
    if (d == null || d.length === 0) {
      this.drops.getAorticProcedure().subscribe((response) => { this.optionsAorticProcedure = response; localStorage.setItem('optionsAorticProcedure', JSON.stringify(response)); });
    } else {
      this.optionsAorticProcedure = JSON.parse(localStorage.getItem('optionsAorticProcedure'));
    }
    d = JSON.parse(localStorage.getItem('options_AorticTypes'));
    // tslint:disable-next-line: max-line-length
    if (d == null || d.length === 0) {
      this.drops.getValveType().subscribe((response) => { this.optionsAorticTypes = response; localStorage.setItem('options_AorticTypes', JSON.stringify(response)); });
    } else {
      this.optionsAorticTypes = JSON.parse(localStorage.getItem('options_AorticTypes'));
    }
    d = JSON.parse(localStorage.getItem('options_Aetiology'));
    // tslint:disable-next-line: max-line-length
    if (d == null || d.length === 0) {
      this.drops.getAetiology().subscribe((response) => { this.optionsAetiology = response; localStorage.setItem('options_Aetiology', JSON.stringify(response)); });
    } else {
      this.optionsAetiology = JSON.parse(localStorage.getItem('options_Aetiology'));
    }
    d = JSON.parse(localStorage.getItem('options_MitralProcedure'));
    // tslint:disable-next-line: max-line-length
    if (d == null || d.length === 0) {
      this.drops.getMitralProcedure().subscribe((response) => { this.optionsMitralProcedure = response; localStorage.setItem('options_MitralProcedure', JSON.stringify(response)); });
    } else { this.optionsMitralProcedure = JSON.parse(localStorage.getItem('options_MitralProcedure')); }
  }

  readyToSelectValve(){return this._showSelectValve;}

  readyToImplant(v: Valve): boolean {

    if (v.Combined === null || v.Combined === 0) {
      this.alertify.error("Please indicate if this is a combined procedure");
      return false;
    }

    if (v.ProcedureAetiology === null || v.ProcedureAetiology === 0) {
      this.alertify.error("Please indicate the aetiology");
      return false;
    }

    if (v.ProcedureType === null || v.ProcedureType === 0) {
      this.alertify.error("Please indicate the type of procedure");
      return false;
    }

    return true;
  }



  deleteFromDatabase(s: string){
    // if the program comes here the valve is already removed in child
    this.procedureValve = {
      Id: 0, Hospitalno:0,Implant_Position: '', IMPLANT: '', EXPLANT: '', SIZE: '', TYPE: '', SIZE_EXP: '',
      TYPE_EXP: 0, ProcedureType: 0, ProcedureAetiology: 0, MODEL: '', MODEL_EXP: '', SERIAL_IMP: '',
      SERIAL_EXP: '', RING_USED: '', valveDescription:'',REPAIR_TYPE: '', Memo: '', Combined: 0, procedure_id: 0
    };
    this.alertify.info(s);
    this._showAddValve = false;
    this._showValvePresent = false;
    this._showSelectValve = false;
  
  }


  receiveFromSelect(v: Valve){
    this.valveService.addValveInProcedure(v.SERIAL_IMP, v.procedure_id).subscribe((next)=>{
      next.Combined = this.procedureValve.Combined;
      next.ProcedureAetiology = this.procedureValve.ProcedureAetiology;
      next.ProcedureType = this.procedureValve.ProcedureType;

      next.Hospitalno = v.Hospitalno;
      next.MODEL = v.MODEL;
      next.TYPE = v.TYPE;
      next.Implant_Position = v.Implant_Position;
      next.SIZE = v.SIZE;
      this.valveService.updateValve(next).subscribe(
        (response)=>{this.alertify.info(response)},
        error => {this.alertify.warning(error)},
        () => {
          this.valveService.getValveFromSerial(v.SERIAL_IMP, v.procedure_id)
          .subscribe((next)=>{this.procedureValve = next});
          this._showValvePresent = true})


    })
    
    
    
   
    }



  canDeactivate() {
    // this.saveValve();
    this.alertify.show('saving Valve');
    return true;
  }
}
