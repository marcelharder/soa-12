import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { AorticSurgery } from 'src/app/_models/AorticSurgery';
import { dropItem } from 'src/app/_models/dropItem';
import { hospitalValve } from 'src/app/_models/hospitalValve';
import { Valve } from 'src/app/_models/Valve';
import { AccountService } from 'src/app/_services/account.service';
import { AorticSurgeryService } from 'src/app/_services/aorticsurgery.service';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { HospitalService } from 'src/app/_services/hospital.service';
import { UserService } from 'src/app/_services/user.service';
import { ValveService } from 'src/app/_services/valve.service';

@Component({
  selector: 'app-aortic',
  templateUrl: './aortic.component.html',
  styleUrls: ['./aortic.component.css']
})
export class AorticComponent implements OnInit {
  @ViewChild('aorticForm') aorticForm: NgForm;
  currentUserId=0;
  currentProcedureId=0;
  pd: AorticSurgery;
  procedureValve: Valve = {
      Id: 0,  Hospitalno:0,Implant_Position: '', IMPLANT: '', EXPLANT: '', SIZE: '', TYPE: '', SIZE_EXP: '',
      TYPE_EXP: 0, ProcedureType: 0, ProcedureAetiology: 0, MODEL: '', MODEL_EXP: '', SERIAL_IMP: '',
      SERIAL_EXP: '', RING_USED: '', valveDescription:'', REPAIR_TYPE: '', Memo: '', Combined: 0, procedure_id: 0
  };
  optionsPathology: Array<dropItem> = [];
  optionsIndication: Array<dropItem> = [];
  optionsTechnique: Array<dropItem> = [];
  optionsRangeReplacement: Array<dropItem> = [];
  optionsAneurysmType: Array<dropItem> = [];
  optionsOnsetDissection: Array<dropItem> = [];
  optionsDissectionType: Array<dropItem> = [];
  optionsAvailableConduits: Array<hospitalValve> = [];
  oviHospital = 0;
  currentHospital = "";
  currentHospitalNo = "";
  AoDetails = 0;
  //OpDetails = 0;
  Cf = 0;
  conduitDescription = "";

  constructor(private alertify: ToastrService,
      private userService: UserService,
      private vs: ValveService,
      private hos: HospitalService,
      private auth: AccountService,
      private route: ActivatedRoute,
      private drops: DropdownService,
      private aorticService: AorticSurgeryService) { }

  ngOnInit() {
    this.auth.currentUser$.pipe(take(1)).subscribe((u) => {this.currentUserId = u.UserId;});
    this.auth.currentProcedure$.pipe(take(1)).subscribe((u) => {this.currentProcedureId = u;})
       
    this.userService
          .getUser(this.currentUserId)
          .subscribe((next) => {
              this.currentHospitalNo = next.hospital_id.toString();
              var index = next.hospital_id;
              // find out if I can show the available valves in the participating hospital
              this.hos.IsThisHospitalUsingOVI(index).subscribe((next) => { if (next) { this.oviHospital = 1; } })
          });

    this.route.data.subscribe(data => {
         this.pd = data.aortic; });

    if (this.pd.operative_technique === '3') {

          this.AoDetails = 1;
          // get optionsAvailableConduits from the hospital
          this.vs.getHospitalValves("Valved_Conduit", "Aortic").subscribe((next) => { this.optionsAvailableConduits = next; });
          // find out if there is already a valved_conduit
          this.vs.getConduits(this.currentProcedureId).subscribe((next) => {
            if (next.length > 0) { this.Cf = 1; this.procedureValve = next[0]; } else { this.Cf = 0; }
            }, (error)=>{this.alertify.error(error)})
          
      };
      this.loadDrops();
  }

  loadDrops() {
      let d = JSON.parse(localStorage.getItem('Pathology'));
      if (d == null || d.length === 0) {
          this.drops.getPathology().subscribe((response) => {
              this.optionsPathology = response; localStorage.setItem('Pathology', JSON.stringify(response));
          });
      } else {
          this.optionsPathology = JSON.parse(localStorage.getItem('Pathology'));
      }
      d = JSON.parse(localStorage.getItem('aorticIndications'));
      if (d == null || d.length === 0) {
          this.drops.getOpIndication().subscribe((response) => {
              this.optionsIndication = response; localStorage.setItem('aorticIndications', JSON.stringify(response));
          });
      } else {
          this.optionsIndication = JSON.parse(localStorage.getItem('aorticIndications'));
      }
      d = JSON.parse(localStorage.getItem('aorticTechnique'));
      if (d == null || d.length === 0) {
          this.drops.getOpTechnique().subscribe((response) => {
              this.optionsTechnique = response; localStorage.setItem('aorticTechnique', JSON.stringify(response));
          });
      } else {
          this.optionsTechnique = JSON.parse(localStorage.getItem('aorticTechnique'));
      }
      d = JSON.parse(localStorage.getItem('range'));
      if (d == null || d.length === 0) {
          this.drops.getRangeReplacement().subscribe((response) => {
              this.optionsRangeReplacement = response; localStorage.setItem('range', JSON.stringify(response));
          });
      } else {
          this.optionsRangeReplacement = JSON.parse(localStorage.getItem('range'));
      }
      d = JSON.parse(localStorage.getItem('aneurysmType'));
      if (d == null || d.length === 0) {
          this.drops.getAneurysmType().subscribe((response) => {
              this.optionsAneurysmType = response; localStorage.setItem('aneurysmType', JSON.stringify(response));
          });
      } else {
          this.optionsAneurysmType = JSON.parse(localStorage.getItem('aneurysmType'));
      }
      d = JSON.parse(localStorage.getItem('onsetDissection'));
      if (d == null || d.length === 0) {
          this.drops.getDissectionOnset().subscribe((response) => {
              this.optionsOnsetDissection = response; localStorage.setItem('onsetDissection', JSON.stringify(response));
          });
      } else {
          this.optionsOnsetDissection = JSON.parse(localStorage.getItem('onsetDissection'));
      }
      d = JSON.parse(localStorage.getItem('typeDissection'));
      if (d == null || d.length === 0) {
          this.drops.getDissectionType().subscribe((response) => {
              this.optionsDissectionType = response; localStorage.setItem('typeDissection', JSON.stringify(response));
          });
      } else {
          this.optionsDissectionType = JSON.parse(localStorage.getItem('typeDissection'));
      }
  }

  showOVI() { if (this.oviHospital === 1) { return true; } }
  showAoDetails() { if (this.AoDetails === 1) { return true; } }
  conduitFound() { if (this.Cf === 1) { return true; } }

  existingValveStatus(x:string){ if(x === "1"){this.Cf = 0} }

  technique_selected(item: any) {
      if (item.target.id === "optech") {
          if (this.pd.operative_technique === '3') {
              this.AoDetails = 1;
              // get optionsAvailableConduits from the hospital if there are no prior records
              if (!this.conduitFound()) {
                  this.vs.getHospitalValves("Valved_Conduit", "Aortic").subscribe((next) => { this.optionsAvailableConduits = next; });
              }
          } else { this.AoDetails = 0; }
      }
  }

  saveAortic() {
      this.aorticService.saveAOS(this.pd).subscribe((next)=>{this.alertify.show('saving Aortic');});
      this.aorticForm.reset(this.pd);
  }

  record_added(v: any) {// the game card added a record
      this.procedureValve = v;
      this.Cf = 1; // show the newly added valve
  }

  canDeactivate() {
      this.saveAortic();
      return true;
  }

  ChkBox1() { if (!this.pd.aneurysm) { this.pd.aneurysm_type = '0'; } }
  ChkBox2() { if (!this.pd.dissection) { this.pd.dissection_onset = '0'; this.pd.dissection_type = '0'; } }

}

