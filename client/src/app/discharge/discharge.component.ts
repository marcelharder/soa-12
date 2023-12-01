import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { discharge } from '../_models/Discharge';
import { dropItem } from '../_models/dropItem';
import { DischargeService } from '../_services/discharge.service';
import { DropdownService } from '../_services/dropdown.service';

@Component({
  selector: 'app-discharge',
  templateUrl: './discharge.component.html',
  styleUrls: ['./discharge.component.css']
})
export class DischargeComponent implements OnInit {
  @ViewChild('dischargeForm', { static: true }) testForm: NgForm;
  pd: discharge;

  optionsDead: Array<dropItem> = [];
  optionsDeadLocation: Array<dropItem> = [];
  optionsDeadCause: Array<dropItem> = [];

  optionsSentTo: Array<dropItem> = [];
  optionsActivitiesDischarge: Array<dropItem> = [];
  optionsDischargeDiagnosis: Array<dropItem> = [];

  showPD = false;

  constructor(
    private route: ActivatedRoute,
    private disch: DischargeService,
    private drops: DropdownService,
    private router: Router,
    private alertify: ToastrService) { }

  ngOnInit(): void {
    this.loadDrops();
    this.route.data.subscribe((data) => {
      this.pd = data.dis;
      if (this.pd.discharge_diagnosis === 6) {
        this.showPD = true;
      }
    });
  }

  discharge_diagnosis_changed() {
    if (+this.pd.discharge_diagnosis === 6) {
      this.showPD = true;
    }
  }

  showFullText() {
    if (this.showPD) {
      return true;
    } else {
      return false;
    }
  }
  hideButtonAction() {
    this.pd.discharge_diagnosis = 0;
    this.showPD = false;
  }

  loadDrops() {
    let d = JSON.parse(localStorage.getItem('dead'));
    if (d == null || d.length === 0) {
      this.drops.getMortality().subscribe((response) => {
        this.optionsDead = response;
        localStorage.setItem('dead', JSON.stringify(response));
      });
    } else {
      this.optionsDead = JSON.parse(localStorage.getItem('dead'));
    }
    d = JSON.parse(localStorage.getItem('dead_location'));
    if (d == null || d.length === 0) {
      this.drops.getMortalityLocation().subscribe((response) => {
        this.optionsDeadLocation = response;
        localStorage.setItem('dead_location', JSON.stringify(response));
      });
    } else {
      this.optionsDeadLocation = JSON.parse(
        localStorage.getItem('dead_location')
      );
    }
    d = JSON.parse(localStorage.getItem('dead_cause'));
    if (d == null || d.length === 0) {
      this.drops.getMortalityCause().subscribe((response) => {
        this.optionsDeadCause = response;
        localStorage.setItem('dead_cause', JSON.stringify(response));
      });
    } else {
      this.optionsDeadCause = JSON.parse(localStorage.getItem('dead_cause'));
    }
    d = JSON.parse(localStorage.getItem('discharge_direction'));
    if (d == null || d.length === 0) {
      this.drops.getDischargeDirection().subscribe((response) => {
        this.optionsSentTo = response;
        localStorage.setItem('discharge_direction', JSON.stringify(response));
      });
    } else {
      this.optionsSentTo = JSON.parse(
        localStorage.getItem('discharge_direction')
      );
    }
    d = JSON.parse(localStorage.getItem('discharge_activities'));
    if (d == null || d.length === 0) {
      this.drops.getActivitiesDischarge().subscribe((response) => {
        this.optionsActivitiesDischarge = response;
        localStorage.setItem('discharge_activities', JSON.stringify(response));
      });
    } else {
      this.optionsActivitiesDischarge = JSON.parse(
        localStorage.getItem('discharge_activities')
      );
    }
    d = JSON.parse(localStorage.getItem('discharge_diagnosis'));
    if (d == null || d.length === 0) {
      this.drops.getDischargeDiagnosis().subscribe((response) => {
        this.optionsDischargeDiagnosis = response;
        localStorage.setItem('discharge_diagnosis', JSON.stringify(response));
      });
    } else {
      this.optionsDischargeDiagnosis = JSON.parse(
        localStorage.getItem('discharge_diagnosis')
      );
    }
  }

  isDead() { if (this.pd.dead.toString() === '2') { return true; } else { return false; } }

  zeroTheDeadData(): void {
    this.pd.dead_location = '0';
    this.pd.dead_cause = '0';
    this.pd.dead_date = new Date(0);
  }

  zeroTheAliveData() {
    this.pd.discharged_to = '0';
    this.pd.discharge_activities = '0';
    this.pd.discharge_diagnosis = 0;
    this.pd.discharge_date = new Date(0);
    this.pd.full_description = "";
  }

  setCurrentDateToDischarge() {
    this.pd.discharge_date = new Date();
  }
  setCurrentDateToMotality() {
    this.pd.dead_date = new Date();
  }

  deadChanged() {
    if (this.pd.dead.toString() === '2'){this.zeroTheAliveData(); this.alertify.show("pat is dead");}
    if (this.pd.dead.toString() === '1'){this.zeroTheDeadData(); this.alertify.show("pat is alive");}
   

  }

  saveDischarge() {
    /*  const d = new Date();
    const n = d.getTimezoneOffset() * 60 * 1000;

    if (this.isDead()) {
      this.pd.dead_date = new Date(this.pd.dead_date.getTime() - n);
      this.zeroTheAliveData();
    } else {
      this.pd.discharge_date = new Date(this.pd.discharge_date.getTime() - n);
      this.zeroTheDeadData();
    } */


    

    this.disch.saveDischarge(this.pd).subscribe((next) => { });




  }

  canDeactivate() {
    this.saveDischarge();
    this.alertify.show('saving Discharge');
    return true;
  }

  saveDis() {
    this.saveDischarge();
    this.router.navigate(['/procedures']);
  }
}

