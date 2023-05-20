import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { dropItem } from 'src/app/_models/dropItem';
import { Patient } from 'src/app/_models/Patient';
import { AccountService } from 'src/app/_services/account.service';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { PatientService } from 'src/app/_services/patient.service';

@Component({
  selector: 'app-euroscoredetails',
  templateUrl: './euroscoredetails.component.html',
  styleUrls: ['./euroscoredetails.component.css']
})
export class EuroscoredetailsComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;
  @ViewChild('toggle1') toggle1: ElementRef;
  @ViewChild('toggle2') toggle2: ElementRef;
  @ViewChild('toggle3') toggle3: ElementRef;
  @ViewChild('toggle4') toggle4: ElementRef;
  @ViewChild('toggle5') toggle5: ElementRef;
  @ViewChild('toggle6') toggle6: ElementRef;
  @ViewChild('toggle7') toggle7: ElementRef;
  @ViewChild('toggle8') toggle8: ElementRef;
  @ViewChild('toggle9') toggle9: ElementRef;

  ageInfluence = 0;
  genderInfluence = 0;
  poormobilityInfluence = 0;
  showCriticalState = '0';
  optionsYN: Array<dropItem> = [];
  optionsAge: Array<dropItem> = [];
  optionsGender: Array<dropItem> = [];
  optionsTiming: Array<dropItem> = [];
  optionsPht: Array<dropItem> = [];
  optionsWeight: Array<dropItem> = [];
  optionsWoint: Array<dropItem> = [];
  optionsNyha: Array<dropItem> = [];
  optionsCreat: Array<dropItem> = [];
  optionsLvef: Array<dropItem> = [];
  showRenalStuff = '0';
  extraCardiacAtheropathy = '0';
  recentMI = '0';
  wl: Patient;

  modalRef: BsModalRef;


  constructor(private route: ActivatedRoute,
      private auth: AccountService,
      private modalService: BsModalService,
      private drops: DropdownService,
      private alertify: ToastrService,
      private patient:PatientService) { }

  ngOnInit() {
      this.loadDrops();
      this.route.data.subscribe(data => {
         
          this.wl = data.patient;
          this.wl.dialysis = false;
          if (this.wl.age === 0) { this.alertify.warning('Age is missing');} else {this.creatinine_clearance(); }
      });
      // restore the state with the stickers from the database.
      this.calculate_euroscore();

  }
  getDialysisState() {
      if (this.wl.dialysis) { return 'Patient on dialysis'; } else { return 'Patient not on dialysis'; }
  }

  loadDrops() {
      let d = JSON.parse(localStorage.getItem('timing_options'));
      if (d == null || d.length === 0) {
          this.drops.getTimingOptions().subscribe((response) => {
              this.optionsTiming = response; localStorage.setItem('timing_options', JSON.stringify(response));
          });
      } else {
          this.optionsTiming = JSON.parse(localStorage.getItem('timing_options'));
      }

      d = JSON.parse(localStorage.getItem('age'));
      if (d == null || d.length === 0) {
          this.drops.getAgeOptions().subscribe((response) => {
              this.optionsAge = response;
              localStorage.setItem('age', JSON.stringify(response));
          });
      } else {
          this.optionsAge = JSON.parse(localStorage.getItem('age'));
      }

      d = JSON.parse(localStorage.getItem('creat'));
      if (d == null || d.length === 0) {
          this.drops.getCreatOptions().subscribe((response) => {
              this.optionsCreat = response;
              localStorage.setItem('creat', JSON.stringify(response));
          });
      } else {
          this.optionsCreat = JSON.parse(localStorage.getItem('creat'));
      }

      d = JSON.parse(localStorage.getItem('NYHA'));
      if (d == null || d.length === 0) {
          this.drops.getNYHA().subscribe((response) => {
              this.optionsNyha = response; localStorage.setItem('NYHA', JSON.stringify(response));
          });
      } else {
          this.optionsNyha = JSON.parse(localStorage.getItem('NYHA'));
      }

      d = JSON.parse(localStorage.getItem('YN'));
      if (d == null || d.length === 0) {
          this.drops.getYNOptions().subscribe((response) => {
              this.optionsYN = response; localStorage.setItem('YN', JSON.stringify(response));
          });
      } else {
          this.optionsYN = JSON.parse(localStorage.getItem('YN'));
      }
      d = JSON.parse(localStorage.getItem('gender'));
      if (d == null || d.length === 0) {
          this.drops.getGenderOptions().subscribe((response) => {
              this.optionsGender = response; localStorage.setItem('gender', JSON.stringify(response));
          });
      } else {
          this.optionsGender = JSON.parse(localStorage.getItem('gender'));
      }
      d = JSON.parse(localStorage.getItem('WeightIntervention'));
      if (d == null || d.length === 0) {
          this.drops.getWoIntOptions().subscribe((response) => {
              this.optionsWoint = response; localStorage.setItem('WeightIntervention', JSON.stringify(response));
          });
      } else {
          this.optionsWoint = JSON.parse(localStorage.getItem('WeightIntervention'));
      }
      d = JSON.parse(localStorage.getItem('LVFunction'));
      if (d == null || d.length === 0) {
          this.drops.getLVFunction().subscribe((response) => {
              this.optionsLvef = response; localStorage.setItem('LVFunction', JSON.stringify(response));
          });
      } else {
          this.optionsLvef = JSON.parse(localStorage.getItem('LVFunction'));
      }
      d = JSON.parse(localStorage.getItem('Pulmonary_hypertension'));
      if (d == null || d.length === 0) {
          this.drops.getPH().subscribe((response) => {
              this.optionsPht = response; localStorage.setItem('Pulmonary_hypertension', JSON.stringify(response));
          });
      } else {
          this.optionsPht = JSON.parse(localStorage.getItem('Pulmonary_hypertension'));
      }
      d = JSON.parse(localStorage.getItem('weight'));
      if (d == null || d.length === 0) {
          this.drops.getWeightOptions().subscribe((response) => {
              this.optionsWeight = response; localStorage.setItem('weight', JSON.stringify(response));
          });
      } else {
          this.optionsWeight = JSON.parse(localStorage.getItem('weight'));
      }
  }

  Fmt(x: number) {
      let v = '';
      if (x >= 0) { v = '' + (x + 0.005); } else { v = '' + (x - 0.005); }
      return v.substring(0, v.indexOf('.') + 3);
  }

  influence_age() {
      let t = +this.wl.age;
      if (t !== 0) {
          t = t + 1;
          t = t * 0.0285181;
          if (t <= 1.711086) { t = 0.0285181; } else { t = t - 60 * 0.0285181; }
          return this.Fmt(t);
      }

  }

  influence_renal_impairment() {
      let help = '';
      const cc = this.creatinine_clearance();
      if (cc !== 0) {
          // tslint:disable-next-line:max-line-length
          if (cc >= 85) { help = ''; } else { if (cc < 50) { help = '.8592256'; } else { if (cc >= 50 && cc <= 85) { help = '.303553'; } } }
          if (this.wl.dialysis === true) { help = '.6421508'; }
      }
      return help;
  }

  influence_gender() { let help = ''; if (this.wl.gender === '2') { help = '.2196434'; } else { help = ''; } return help; }
  influence_thor() { let help = ''; if (this.wl.surgery_on_thoracic_aorta === '1') { help = '.6527205'; } else { help = ''; } return help; }
  influenceArteriopathy = function () { let help = ''; if (this.wl.extra_cardiac_arteriopathy === '1') { help = '.5360268'; } else { help = ''; } return help; };
  influence_poor_mobility() { let help = ''; if (this.wl.poor_mobility === '1') { help = '.2407181'; } else { help = ''; } return help; }
  influence_previous_cardiac_surgery() { let help = ''; if (this.wl.previous_cardiac_surgery === '1') { help = '1.118599'; } else { help = ''; } return help; }
  influence_chronic_lung_disease() { let help = ''; if (this.wl.copd === '1') { help = '.1886564'; } else { help = ''; } return help; }
  influence_endocarditis() { let help = ''; if (this.wl.active_endocarditis === '1') { help = '.6194522'; } else { help = ''; } return help; }
  influence_diabetes() { let help = ''; if (this.wl.diabetes_on_insulin === '1') { help = '.3542749'; } else { help = ''; } return help; }
  influence_mi() { let help = ''; if (this.wl.recent_mi === '1') { help = '.1528943'; } return help; }
  influence_ccs() { let help = ''; if (this.wl.ccs === '1') { help = '.2226147'; } else { help = ''; } return help; }


  influence_crit_state() {
      let help = '';
      const cs = this.critical_state();
      if (cs === 'No') { help = ''; }
      if (cs === 'Yes') { help = '1.086517'; }
      return help;
  }
  influence_nyha() {
      let help = '';
      if (this.wl.nyha === '1') { help = ''; }
      if (this.wl.nyha === '2') { help = '.1070545'; }
      if (this.wl.nyha === '3') { help = '.2958358'; }
      if (this.wl.nyha === '4') { help = '.5597929'; }
      return help;
  }
  influence_lv() {
      let help = '';
      if (this.wl.lvef === '1') { help = ''; }
      if (this.wl.lvef === '2') { help = '.3150652'; }
      if (this.wl.lvef === '3') { help = '.8084096'; }
      if (this.wl.lvef === '4') { help = '.9346919'; }
      return help;
  }

  influence_pht() {
      let help = '';
      if (this.wl.systolic_pa_pressure === '1') { help = ''; }
      if (this.wl.systolic_pa_pressure === '2') { help = '.1788899'; }
      if (this.wl.systolic_pa_pressure === '3') { help = '.3491475'; }
      return help;
  }
  influence_urgency() {
      let help = '';
      if (this.wl.timing === '0') { help = ''; }
      if (this.wl.timing === '1') { help = ''; }
      if (this.wl.timing === '2') { help = '.3174673'; }
      if (this.wl.timing === '3') { help = '.7039121'; }
      if (this.wl.timing === '4') { help = '1.362947'; }
      return help;
  }
  influence_weight_intervention() {
      let help = '';
      if (this.wl.weight_of_intervention === '1') { help = ''; }
      if (this.wl.weight_of_intervention === '2') { help = '.0062118'; }
      if (this.wl.weight_of_intervention === '3') { help = '.5521478'; }
      if (this.wl.weight_of_intervention === '4') { help = '.9724533'; }
      return help;
  }


  calculate_euroscore() {
      let help = '';
      let z = 0;
      if (this.influence_age() !== '') { z = z + parseFloat(this.influence_age()); }
      if (this.influence_gender() !== '') { z = z + parseFloat(this.influence_gender()); }
      if (this.influence_renal_impairment() !== '') { z = z + parseFloat(this.influence_renal_impairment()); }
      if (this.influenceArteriopathy() !== '') { z = z + parseFloat(this.influenceArteriopathy()); }
      if (this.influence_poor_mobility() !== '') { z = z + parseFloat(this.influence_poor_mobility()); }
      if (this.influence_previous_cardiac_surgery() !== '') { z = z + parseFloat(this.influence_previous_cardiac_surgery()); }
      if (this.influence_chronic_lung_disease() !== '') { z = z + parseFloat(this.influence_chronic_lung_disease()); }
      if (this.influence_endocarditis() !== '') { z = z + parseFloat(this.influence_endocarditis()); }
      if (this.influence_crit_state() !== '') { z = z + parseFloat(this.influence_crit_state()); }
      if (this.influence_diabetes() !== '') { z = z + parseFloat(this.influence_diabetes()); }
      if (this.influence_nyha() !== '') { z = z + parseFloat(this.influence_nyha()); }
      if (this.influence_ccs() !== '') { z = z + parseFloat(this.influence_ccs()); }
      if (this.influence_lv() !== '') { z = z + parseFloat(this.influence_lv()); }
      if (this.influence_mi() !== '') { z = z + parseFloat(this.influence_mi()); }
      if (this.influence_pht() !== '') { z = z + parseFloat(this.influence_pht()); }
      if (this.influence_urgency() !== '') { z = z + parseFloat(this.influence_urgency()); }
      if (this.influence_weight_intervention() !== '') { z = z + parseFloat(this.influence_weight_intervention()); }
      if (this.influence_thor() !== '') { z = z + parseFloat(this.influence_thor()); }
      z = z - 5.324537;
      z = Math.exp(z) / (1 + Math.exp(z));
      help = this.Fmt(100 * z) + ' %';
      return help;
  }
  displayCritState() { if (this.showCriticalState === '1') { return true; } else { return false; } }
  displayRenalStuff() { if (this.showRenalStuff === '1') { return true; } else { return false; } }

  test1() {
      this.calculate_euroscore();
      if (this.wl.recent_mi === '1') {
          this.toggle1.nativeElement.style.color = 'red';
      } else {
          this.toggle1.nativeElement.style.color = 'black';
      }
  }
  test3() {
      this.calculate_euroscore();
      if (this.wl.poor_mobility === '1') {
          this.toggle3.nativeElement.style.color = 'red';
      } else {
          this.toggle3.nativeElement.style.color = 'black';
      }
  }
  test2() {
      this.calculate_euroscore();
      if (this.wl.extra_cardiac_arteriopathy === '1') {
          this.toggle2.nativeElement.style.color = 'red';
      } else {
          this.toggle2.nativeElement.style.color = 'black';
      }
  }
  test4() {
      this.calculate_euroscore();
      if (this.wl.previous_cardiac_surgery === '1') {
          this.toggle4.nativeElement.style.color = 'red';
      } else {
          this.toggle4.nativeElement.style.color = 'black';
      }
  }
  test5() {
      this.calculate_euroscore();
      if (this.wl.copd === '1') {
          this.toggle5.nativeElement.style.color = 'red';
      } else {
          this.toggle5.nativeElement.style.color = 'black';
      }
  }
  test6() {
      this.calculate_euroscore();
      if (this.wl.active_endocarditis === '1') {
          this.toggle6.nativeElement.style.color = 'red';
      } else {
          this.toggle6.nativeElement.style.color = 'black';
      }
  }
  test7() {
      this.calculate_euroscore();
      if (this.wl.diabetes_on_insulin === '1') {
          this.toggle7.nativeElement.style.color = 'red';
      } else {
          this.toggle7.nativeElement.style.color = 'black';
      }
  }
  test8() {
      this.calculate_euroscore();
      if (this.wl.surgery_on_thoracic_aorta === '1') {
          this.toggle8.nativeElement.style.color = 'red';
      } else {
          this.toggle8.nativeElement.style.color = 'black';
      }
  }
  test9() {
      this.calculate_euroscore();
      if (this.wl.ccs === '1') {
          this.toggle9.nativeElement.style.color = 'red';
      } else {
          this.toggle9.nativeElement.style.color = 'black';
      }
  }
  creatinine_clearance(): number {
      let help = 0;
      let wt = 0;
      let creat = 0;
      let years = 0;
      let sex = 0;
      if (this.wl.age !== 0 && this.wl.creat_number !== 0) {
          if (this.wl.gender === '2') { sex = 0.85; } else { sex = 1; }
          try {
              wt = +this.wl.weight;
              years = +this.wl.age;
              creat = +this.wl.creat_number;
              help = (140 - years) * wt * sex / (72 * (creat / 88.4));
          } catch (error) { console.log(error); }
      }
      let result = 0;
      if (help !== 0) { result = +help.toFixed(2); }
      return result;
      // return this.Fmt(help);
      // return help;
  }


  critical_state() {
      let help = 'Yes';
      if (
          this.wl.crit_shock === false &&
          this.wl.crit_inotropes === false &&
          this.wl.crit_arrythmia === false &&
          this.wl.crit_resuscitation === false &&
          this.wl.crit_iabp === false &&
          this.wl.crit_ventilated === false &&
          this.wl.crit_renal_failure === false) { help = 'No'; return help; }
      return help;
  }
  openModal(template: TemplateRef<any>) { this.modalRef = this.modalService.show(template); }

  saveEuroScore() {
      this.wl.log_score = this.calculate_euroscore();
      let currentUserId = 0;
      this.auth.currentUser$.pipe(take(1)).subscribe((u) => {currentUserId = u.UserId;})
      this.patient.updatePatient(this.wl, currentUserId).subscribe((next) => { })
  }

  canDeactivate() {
      this.saveEuroScore();
      return true;
  }

  saveES() { this.saveEuroScore(); this.alertify.show('Euroscore details Saved ...');}




}


