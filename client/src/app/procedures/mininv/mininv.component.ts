import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { dropItem } from 'src/app/_models/dropItem';
import { MinInv } from 'src/app/_models/MinInv';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { MinInvService } from 'src/app/_services/mininv.service';

@Component({
  selector: 'app-mininv',
  templateUrl: './mininv.component.html',
  styleUrls: ['./mininv.component.css']
})
export class MininvComponent implements OnInit {
  @ViewChild('minForm') minForm: NgForm;
  pd: MinInv;

  optionStrategy: Array<dropItem> = [];
  optionsYN: Array<dropItem> = [];
  optionPrimaryIncision: Array<dropItem> = [];
  optionPrimaryIncisionDetails: Array<dropItem> = [];
  optionFollow1: Array<dropItem> = [];
  optionFollow2: Array<dropItem> = [];
  optionFollow3: Array<dropItem> = [];
  optionNumberIncisions: Array<string> = [];
  optionLimaHarvest: Array<dropItem> = [];
  optionStabilization: Array<dropItem> = [];
  optionSutureTechnique: Array<dropItem> = [];
  optionIschemicTime: Array<string> = [];
  optionConversionDetails: Array<dropItem> = [];
  optionIABPWHEN: Array<dropItem> = [];
  optionIABPWHY: Array<dropItem> = [];


  constructor(private route: ActivatedRoute,
      private drops: DropdownService,
      private alertify: ToastrService,
      private min: MinInvService) { }

  ngOnInit(): void {
      this.loadDrops();
      this.route.data.subscribe(data => { this.pd = data.min; });
      this.primary_incision_follow();

  }

  loadDrops() {
      for (let x = 1; x < 31; x++) { this.optionIschemicTime.push(x.toString()); }
      for (let x = 1; x < 6; x++) { this.optionNumberIncisions.push(x.toString()); }


      let d = JSON.parse(localStorage.getItem('YN'));
      if (d == null || d.length === 0) {
          this.drops.getYNOptions().subscribe((response) => {
              this.optionsYN = response; localStorage.setItem('YN', JSON.stringify(response));
          });
      } else {
          this.optionsYN = JSON.parse(localStorage.getItem('YN'));
      }
       d = JSON.parse(localStorage.getItem('conversionDetails'));
      if (d == null || d.length === 0) {
          this.drops.getConversionDetails().subscribe((response) => {
              this.optionConversionDetails = response; localStorage.setItem('conversionDetails', JSON.stringify(response));
          });
      } else {
          this.optionConversionDetails = JSON.parse(localStorage.getItem('conversionDetails'));
      }

      d = JSON.parse(localStorage.getItem('strategy'));
      if (d == null || d.length === 0) {
          this.drops.getStrategy().subscribe((response) => {
              this.optionStrategy = response; localStorage.setItem('strategy', JSON.stringify(response));
          });
      } else {
          this.optionStrategy = JSON.parse(localStorage.getItem('strategy'));
      }
      d = JSON.parse(localStorage.getItem('primaryIncision'));
      if (d == null || d.length === 0) {
          this.drops.getPrimary_incision().subscribe((response) => {
              this.optionPrimaryIncision = response; localStorage.setItem('primaryIncision', JSON.stringify(response));
          });
      } else {
          this.optionPrimaryIncision = JSON.parse(localStorage.getItem('primaryIncision'));
      }
      d = JSON.parse(localStorage.getItem('follow1'));
      if (d == null || d.length === 0) {
          this.drops.getFollow1().subscribe((response) => {
              this.optionFollow1 = response; localStorage.setItem('follow1', JSON.stringify(response));
          });
      } else {
          this.optionFollow1 = JSON.parse(localStorage.getItem('follow1'));
      }
      d = JSON.parse(localStorage.getItem('follow2'));
      if (d == null || d.length === 0) {
          this.drops.getFollow2().subscribe((response) => {
              this.optionFollow2 = response; localStorage.setItem('follow2', JSON.stringify(response));
          });
      } else {
          this.optionFollow2 = JSON.parse(localStorage.getItem('follow2'));
      }
      d = JSON.parse(localStorage.getItem('follow3'));
      if (d == null || d.length === 0) {
          this.drops.getFollow3().subscribe((response) => {
              this.optionFollow3 = response; localStorage.setItem('follow3', JSON.stringify(response));
          });
      } else {
          this.optionFollow3 = JSON.parse(localStorage.getItem('follow3'));
      }
      d = JSON.parse(localStorage.getItem('limaHarvest'));
      if (d == null || d.length === 0) {
          this.drops.getLimaHarvest().subscribe((response) => {
              this.optionLimaHarvest = response; localStorage.setItem('limaHarvest', JSON.stringify(response));
          });
      } else {
          this.optionLimaHarvest = JSON.parse(localStorage.getItem('limaHarvest'));
      }
      d = JSON.parse(localStorage.getItem('suture'));
      if (d == null || d.length === 0) {
          this.drops.getSuture_technique().subscribe((response) => {
              this.optionSutureTechnique = response; localStorage.setItem('suture', JSON.stringify(response));
          });
      } else {
          this.optionSutureTechnique = JSON.parse(localStorage.getItem('suture'));
      }
      d = JSON.parse(localStorage.getItem('stabilization'));
      if (d == null || d.length === 0) {
          this.drops.getStabilization().subscribe((response) => {
              this.optionStabilization = response; localStorage.setItem('stabilization', JSON.stringify(response));
          });
      } else {
          this.optionStabilization = JSON.parse(localStorage.getItem('stabilization'));
      }

      d = JSON.parse(localStorage.getItem('iabp_why'));
      if (d == null || d.length === 0) {
          this.drops.getIABP_WHY().subscribe((response) => {
              this.optionIABPWHY = response; localStorage.setItem('iabp_why', JSON.stringify(response));
          });
      } else {
          this.optionIABPWHY = JSON.parse(localStorage.getItem('iabp_why'));
      }
      d = JSON.parse(localStorage.getItem('iabp_when'));
      if (d == null || d.length === 0) {
          this.drops.getIABP_WHEN().subscribe((response) => {
              this.optionIABPWHEN = response; localStorage.setItem('iabp_when', JSON.stringify(response));
          });
      } else {
          this.optionIABPWHEN = JSON.parse(localStorage.getItem('iabp_when'));
      }

  }

  robot_used() {
      if (this.pd.robot.toString() === '2') {
          return true;
      } else {
          return false
      }
  }
  conversion() {
      if (this.pd.conversion_to_standard.toString() === '2') {
          return true;
      } else {
          return false
      }
  }
  iabp_follow() {
      if (this.pd.iabp.toString() === '2') {
          return true;
      } else {
          return false
      }
  }
  primary_incision_follow() {
      if (this.pd.primary_incision.toString() === '1') { this.optionPrimaryIncisionDetails = this.optionFollow1; }
      if (this.pd.primary_incision.toString() === '2') { this.optionPrimaryIncisionDetails = this.optionFollow2; }
      if (this.pd.primary_incision.toString() === '3') { this.optionPrimaryIncisionDetails = this.optionFollow2; }
  }


  saveMin() { this.min.saveMinInv(this.pd).subscribe((next) => { }); }
  canDeactivate() {
      this.saveMin();
      this.alertify.show('saving Min Inv');
      return true;
  }
}
