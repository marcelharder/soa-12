import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CPB } from 'src/app/_models/CPB';
import { dropItem } from 'src/app/_models/dropItem';
import { CPBService } from 'src/app/_services/cpb.service';
import { DropdownService } from 'src/app/_services/dropdown.service';

@Component({
  selector: 'app-cpb',
  templateUrl: './cpb.component.html',
  styleUrls: ['./cpb.component.css']
})
export class CpbComponent implements OnInit  {
  @ViewChild('cpbForm') cpbForm: NgForm;
  pd: CPB;

  optionsYN: Array<dropItem> = [];
  optionsMPT: Array<dropItem> = [];
  optionsNonCardiopleg: Array<dropItem> = [];
  optionsDelivery: Array<dropItem> = [];
  optionsIabpTiming: Array<dropItem> = [];
  optionsIabpIndication: Array<dropItem> = [];
  optionsCardioplegia: Array<dropItem> = [];

  ArterialChoices: Array<dropItem> = [];
  VenousChoices: Array<dropItem> = [];
  OcclusionMethod: Array<dropItem> = [];
  Temperature: Array<string> = [];

  optionsCardioplegiaTemperature: Array<dropItem> = [];
  optionsCardioplegiaTiming: Array<dropItem> = [];


  hours: Array<string> = [];
  mins: Array<string> = [];

  constructor(
      private route: ActivatedRoute,
      private cpb: CPBService,
      private drops: DropdownService,
      private alertify: ToastrService) { }

  ngOnInit() {

      this.route.data.subscribe(data => { this.pd = data.cpb; });

      this.loadDrops();

  }

  loadDrops() {

      for (let x = 0; x < 24; x++) { this.hours.push(x.toString()); }
      for (let x = 0; x < 60; x++) { this.mins.push(x.toString()); }

      this.Temperature = ['14','15','16','17','18','19','20','21','22','23','24','25','26','27','28','29','30','31','32','33','34','35','36','37'];


      let d = JSON.parse(localStorage.getItem('mpt')); // myocardial protection technique
      if (d == null || d.length === 0) {
          this.drops.getMPT().subscribe((response) => {
              this.optionsMPT = response; localStorage.setItem('mpt', JSON.stringify(response));
          });
      } else {
          this.optionsMPT = JSON.parse(localStorage.getItem('mpt'));
      }
      d = JSON.parse(localStorage.getItem('YN')); // myocardial protection technique
      if (d == null || d.length === 0) {
          this.drops.getYNOptions().subscribe((response) => {
              this.optionsYN = response; localStorage.setItem('YN', JSON.stringify(response));
          });
      } else {
          this.optionsYN = JSON.parse(localStorage.getItem('YN'));
      }

      d = JSON.parse(localStorage.getItem('nccp')); // myocardial protection technique
      if (d == null || d.length === 0) {
          this.drops.getNCCP().subscribe((response) => {
              this.optionsNonCardiopleg = response; localStorage.setItem('nccp', JSON.stringify(response));
          });
      } else {
          this.optionsNonCardiopleg = JSON.parse(localStorage.getItem('nccp'));
      }
      d = JSON.parse(localStorage.getItem('achoice')); // myocardial protection technique
      if (d == null || d.length === 0) {
          this.drops.getArterialCanulation().subscribe((response) => {
              this.ArterialChoices = response; localStorage.setItem('achoice', JSON.stringify(response));
          });
      } else {
          this.ArterialChoices = JSON.parse(localStorage.getItem('achoice'));
      }
      d = JSON.parse(localStorage.getItem('vchoice')); // myocardial protection technique
      if (d == null || d.length === 0) {
          this.drops.getVenousCanulation().subscribe((response) => {
              this.VenousChoices = response; localStorage.setItem('vchoice', JSON.stringify(response));
          });
      } else {
          this.VenousChoices = JSON.parse(localStorage.getItem('vchoice'));
      }
      d = JSON.parse(localStorage.getItem('occlusion_method')); // myocardial protection technique
      if (d == null || d.length === 0) {
          this.drops.getOcclusionMethod().subscribe((response) => {
              this.OcclusionMethod = response; localStorage.setItem('occlusion_method', JSON.stringify(response));
          });
      } else {
          this.OcclusionMethod = JSON.parse(localStorage.getItem('occlusion_method'));
      }

      d = JSON.parse(localStorage.getItem('cardioplegtemp')); // myocardial protection technique
      if (d == null || d.length === 0) {
          this.drops.getCardioplegTemp().subscribe((response) => {
              this.optionsCardioplegiaTemperature = response; localStorage.setItem('cardioplegtemp', JSON.stringify(response));
          });
      } else {
          this.optionsCardioplegiaTemperature = JSON.parse(localStorage.getItem('cardioplegtemp'));
      }
      d = JSON.parse(localStorage.getItem('cardioplegtiming')); // myocardial protection technique
      if (d == null || d.length === 0) {
          this.drops.getCardioplegTiming().subscribe((response) => {
              this.optionsCardioplegiaTiming = response; localStorage.setItem('cardioplegtiming', JSON.stringify(response));
          });
      } else {
          this.optionsCardioplegiaTiming = JSON.parse(localStorage.getItem('cardioplegtiming'));
      }
      d = JSON.parse(localStorage.getItem('cardioplegdelivery')); // myocardial protection technique
      if (d == null || d.length === 0) {
          this.drops.getDelivery().subscribe((response) => {
              this.optionsDelivery = response; localStorage.setItem('cardioplegdelivery', JSON.stringify(response));
          });
      } else {
          this.optionsDelivery = JSON.parse(localStorage.getItem('cardioplegdelivery'));
      }
      d = JSON.parse(localStorage.getItem('iabp_timing')); 
      if (d == null || d.length === 0) {
          this.drops.getIabpTiming().subscribe((response) => {
              this.optionsIabpTiming = response; localStorage.setItem('iabp_timing', JSON.stringify(response));
          });
      } else {
          this.optionsIabpTiming = JSON.parse(localStorage.getItem('iabp_timing'));
      }
      d = JSON.parse(localStorage.getItem('typeCardioplegia'));
      if (d == null || d.length === 0) {
          this.drops.getTypeCardioplegia().subscribe((response) => {
              this.optionsCardioplegia = response; localStorage.setItem('typeCardioplegia', JSON.stringify(response));
          });
      } else {
          this.optionsCardioplegia = JSON.parse(localStorage.getItem('typeCardioplegia'));
      }
      d = JSON.parse(localStorage.getItem('iabp_indication')); // myocardial protection technique
      if (d == null || d.length === 0) {
          this.drops.getIabpIndication().subscribe((response) => {
              this.optionsIabpIndication = response; localStorage.setItem('iabp_indication', JSON.stringify(response));
          });
      } else {
          this.optionsIabpIndication = JSON.parse(localStorage.getItem('iabp_indication'));
      }
  }

  showCPB() { if (this.pd.cpb_used === '2') { return true; } else { return false;} }

  showIabpTiming() { if (this.pd.iabp === '2') { return true; } else { return false;}}


  saveCPB() {
      this.cpb.saveCPB(this.pd).subscribe((next) => {  });
  }

  prefillcpbstart() {
      const help = new Date();
      this.pd.cpb_start_min = help.getUTCMinutes();
      this.pd.cpb_start_hr = help.getUTCHours();
  }
  prefillcpbstop() {
      const help = new Date();
      this.pd.cpb_stop_min = help.getUTCMinutes();
      this.pd.cpb_stop_hr = help.getUTCHours();
  }

  prefillclampOn() {
      const help = new Date();
      this.pd.clamp_start_min = help.getUTCMinutes();
      this.pd.clamp_start_hr = help.getUTCHours();
  }
  prefillclampOff() {
      const help = new Date();
      this.pd.clamp_stop_min = help.getUTCMinutes();
      this.pd.clamp_stop_hr = help.getUTCHours();
  }

  canDeactivate() {
      this.saveCPB();
      this.alertify.show('saving Cpb');
      return true;
  }


}
