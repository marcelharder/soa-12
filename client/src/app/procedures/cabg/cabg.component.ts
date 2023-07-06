import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { CABG } from 'src/app/_models/CABG';
import { dropItem } from 'src/app/_models/dropItem';
import { CABGService } from 'src/app/_services/cabg.service';
import { DropdownService } from 'src/app/_services/dropdown.service';

@Component({
  selector: 'app-cabg',
  templateUrl: './cabg.component.html',
  styleUrls: ['./cabg.component.css']
})
export class CabgComponent implements OnInit  {
  @ViewChild('cabgForm') cabgForm: NgForm;
  pd: CABG;

  showVSM= false;
  showRadial= false;
  show80= false;

  optionsYN: Array<dropItem> = [];
  optionsDA: Array<dropItem> = [];
  optionsTypeDa: Array<dropItem> = [];
  optionsLocProcDa: Array<dropItem> = [];
  optionsConduit: Array<dropItem> = [];
  optionsSitePa: Array<dropItem> = [];
  optionsAngle: Array<dropItem> = [];
  optionsDiameter: Array<dropItem> = [];
  optionsQuality: Array<dropItem> = [];
  optionsDropList1: Array<dropItem> = [];
  optionsRadial: Array<dropItem> = [];
  optionsLeg: Array<dropItem> = [];
  modalRef: BsModalRef;
  optionsVSMTechnique: Array<dropItem> = [];
  optionsRadialTechnique: Array<dropItem> = [];
 
 

  constructor(private route: ActivatedRoute,
      private modalService: BsModalService,
      private alertify: ToastrService,
      private drops: DropdownService,
      private cabgService: CABGService) { }

  ngOnInit() {
      this.loadDrops();
     
      this.route.data.subscribe(data => {
          this.pd = data.cabg;
          this.cabgService.get80Used(this.pd.procedure_id).subscribe((next) => { this.show80 = next; });
          this.cabgService.getVSMUsed(this.pd.procedure_id).subscribe((next) => { this.showVSM = next; });
          this.cabgService.getRadialUsed(this.pd.procedure_id).subscribe((next) => { this.showRadial = next; });
      });

  }


  showTachtigStuff() { if (this.show80) { return true; } } // show this when a CABG is done combined with a valve procedure
  showVSMHarvesting() { if (this.showVSM) { return true; } } // show this when a VSM was used
  showRadialHarvesting() { if (this.showRadial) { return true; } }// show this when a Radial was used

  showRow2() { if (this.pd.b1_site !== '0') { return true; } else { return false; } }
  showRow3() { if (this.pd.b2_site !== '0') { return true; } else { return false; } }
  showRow4() { if (this.pd.b3_site !== '0') { return true; } else { return false; } }
  showRow5() { if (this.pd.b4_site !== '0') { return true; } else { return false; } }
  showRow6() { if (this.pd.b5_site !== '0') { return true; } else { return false; } }

  loadDrops() {
      let d = JSON.parse(localStorage.getItem('YN'));
      if (d == null || d.length === 0) {
          this.drops.getYNOptions().subscribe((response) => {
              this.optionsYN = response; localStorage.setItem('YN', JSON.stringify(response));
          });
      } else {
          this.optionsYN = JSON.parse(localStorage.getItem('YN'));
      }
      d = JSON.parse(localStorage.getItem('quality'));
      if (d == null || d.length === 0) {
          this.drops.getCabgQuality().subscribe((response) => {
              this.optionsQuality = response; localStorage.setItem('quality', JSON.stringify(response));
          });
      } else {
          this.optionsQuality = JSON.parse(localStorage.getItem('quality'));
      }

      d = JSON.parse(localStorage.getItem('diameter'));
      if (d == null || d.length === 0) {

          this.drops.getCabgDiameter().subscribe((response) => {
              this.optionsDiameter = response; localStorage.setItem('diameter', JSON.stringify(response));
          });
      } else {

          this.optionsDiameter = JSON.parse(localStorage.getItem('diameter'));
      }

      d = JSON.parse(localStorage.getItem('angle'));
      if (d == null || d.length === 0) {
          this.drops.getCabgAngle().subscribe((response) => {
              this.optionsAngle = response; localStorage.setItem('angle', JSON.stringify(response));
          });
      } else {
          this.optionsAngle = JSON.parse(localStorage.getItem('angle'));
      }

      d = JSON.parse(localStorage.getItem('type'));
      if (d == null || d.length === 0) {
          this.drops.getCabgType().subscribe((response) => {
              this.optionsTypeDa = response; localStorage.setItem('type', JSON.stringify(response));
          });
      } else {
          this.optionsTypeDa = JSON.parse(localStorage.getItem('type'));
      }

      d = JSON.parse(localStorage.getItem('locatie'));

      if (d == null || d.length === 0) {
          this.drops.getCabgLocatie().subscribe((response) => {
              this.optionsDA = response; localStorage.setItem('locatie', JSON.stringify(response));
          });
      } else {
          this.optionsDA = JSON.parse(localStorage.getItem('locatie'));
      }

      d = JSON.parse(localStorage.getItem('proximal'));

      if (d == null || d.length === 0) {
          this.drops.getCabgProximal().subscribe((response) => {
              this.optionsSitePa = response; localStorage.setItem('proximal', JSON.stringify(response));
          });
      } else {
          this.optionsSitePa = JSON.parse(localStorage.getItem('proximal'));
      }


      d = JSON.parse(localStorage.getItem('conduit'));

      if (d == null || d.length === 0) {
          this.drops.getCabgConduit().subscribe((response) => {
              this.optionsConduit = response; localStorage.setItem('conduit', JSON.stringify(response));
          });
      } else {
          this.optionsConduit = JSON.parse(localStorage.getItem('conduit'));
      }

      d = JSON.parse(localStorage.getItem('procedure'));

      if (d == null || d.length === 0) {
          this.drops.getCabgProcedure().subscribe((response) => {
              this.optionsLocProcDa = response; localStorage.setItem('procedure', JSON.stringify(response));
          });
      } else {
          this.optionsLocProcDa = JSON.parse(localStorage.getItem('procedure'));
      }

      d = JSON.parse(localStorage.getItem('dropList1'));

      if (d == null || d.length === 0) {
          this.drops.getCabgDroplist1().subscribe((response) => {
              this.optionsDropList1 = response; localStorage.setItem('dropList1', JSON.stringify(response));
          });
      } else {
          this.optionsDropList1 = JSON.parse(localStorage.getItem('dropList1'));
      }

      d = JSON.parse(localStorage.getItem('radial'));

      if (d == null || d.length === 0) {
          this.drops.getCabgRadial().subscribe((response) => {
              this.optionsRadial = response; localStorage.setItem('radial', JSON.stringify(response));
          });
      } else {
          this.optionsRadial = JSON.parse(localStorage.getItem('radial'));
      }

      d = JSON.parse(localStorage.getItem('leg'));

      if (d == null || d.length === 0) {
          this.drops.getCabgLeg().subscribe((response) => {
              this.optionsLeg = response; localStorage.setItem('leg', JSON.stringify(response));
          });
      } else {
          this.optionsLeg = JSON.parse(localStorage.getItem('leg'));
      }
      d = JSON.parse(localStorage.getItem('leg_harvest_technique'));
      if (d == null || d.length === 0) {
          this.optionsVSMTechnique.push({value:0,description:"Choose"});
          this.optionsVSMTechnique.push({value:1,description:"EVH"});
          this.optionsVSMTechnique.push({value:2,description:"Open"});
          this.optionsVSMTechnique.push({value:3,description:"Interrupted"});
      } else {
          this.optionsLeg = JSON.parse(localStorage.getItem('leg_harvest_technique'));
      }
      
      d = JSON.parse(localStorage.getItem('radial_harvest_technique'));
      if (d == null || d.length === 0) {
          this.optionsRadialTechnique.push({value:0,description:"Choose"});
          this.optionsRadialTechnique.push({value:1,description:"ERAH"});
          this.optionsRadialTechnique.push({value:2,description:"Open"});
          this.optionsRadialTechnique.push({value:3,description:"Interrupted"});
      } else {
          this.optionsLeg = JSON.parse(localStorage.getItem('radial_harvest_technique'));
      }
  }

  openModal(template: TemplateRef<any>) { this.modalRef = this.modalService.show(template); }



  saveCABG() {
      this.cabgService.saveCABG(this.pd).subscribe();
      this.cabgForm.reset(this.pd);
  }
  canDeactivate() {
      this.saveCABG();
      this.alertify.show('saving CABG');
      return true;
  }

}

