import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { dropItem } from 'src/app/_models/dropItem';
import { previewReport } from 'src/app/_models/previewReport';
import { Suggestion } from 'src/app/_models/Suggestion';
import { AccountService } from 'src/app/_services/account.service';
import { PreViewReportService } from 'src/app/_services/pre-view-report.service';

@Component({
  selector: 'app-opreport',
  templateUrl: './opreport.component.html',
  styleUrls: ['./opreport.component.css']
})
export class OpreportComponent {

  procedures: Array<dropItem> = [];
  selectedSoort = 0;
  edit = '0';
  currentUserId = 0;
  reportCode = 0;
  sug: Suggestion;
  pre: previewReport = {
      Id: 0,
      procedure_id: 0,
      regel_1: '',
      regel_2: '',
      regel_3: '',
      regel_4: '',
      regel_5: '',
      regel_6: '',
      regel_7: '',
      regel_8: '',
      regel_9: '',
      regel_10: '',
      regel_11: '',
      regel_12: '',
      regel_13: '',
      regel_14: '',
      regel_15: '',
      regel_16: '',
      regel_17: '',
      regel_18: '',
      regel_19: '',
      regel_20: '',
      regel_21: '',
      regel_22: '',
      regel_23: '',
      regel_24: '',
      regel_25: '',
      regel_26: '',
      regel_27: '',
      regel_28: '',
      regel_29: '',
      regel_30: '',
      regel_31: '',
      regel_32: '',
      regel_33: '',
      patientName: '',
      MedicalRecordNumber: '',
  Diagnosis:''};
  modalRef: BsModalRef;

  constructor(private alertify: ToastrService,
      private preview: PreViewReportService,
      private modalService: BsModalService,
      private auth: AccountService) {
  }

  ngAfterContentInit(): void {
      this.preview.getIndividualSuggestions().subscribe((next) => { this.procedures = next});
  }



  editReport() {
      // do not do anything when there are no report templates
      if(this.procedures.length === 0){
          this.alertify.show("For this to work you must have saved at least one procedure template");
      } else
      {
      this.edit = '1';
      this.preview.getSuggestionBySoort(this.selectedSoort).subscribe((next) => {
          this.sug = next;
          this.transform();
          // this.preview.getReportCode(this.selectedSoort).subscribe((next) => { this.reportCode = next; });
        });
      }
  }

  saveChanges()
  {
      this.preview.saveSuggestion(this.pre, this.selectedSoort).subscribe((next) =>
      {this.edit = '0'; }, (error) => { this.alertify.error('Report was not saved ...' + error); });
  }
  cancelChanges(){this.edit = '0';  }
  showCabg() { if (this.reportCode === 1) { return true; } }
  showValveBlok1() { if (this.reportCode === 1) { return false; }}
  showValveBlok2() { if (this.reportCode === 1) { return false; } }

  showEditDetails() {if (this.edit === '1') { return true; } else { return false; } }

  transform() {
      this.pre.regel_1 = this.sug.regel_1_a + this.sug.regel_1_b + this.sug.regel_1_c;
      this.pre.regel_2 = this.sug.regel_2_a + this.sug.regel_2_b + this.sug.regel_2_c;
      this.pre.regel_3 = this.sug.regel_3_a + this.sug.regel_3_b + this.sug.regel_3_c;
      this.pre.regel_4 = this.sug.regel_4_a + this.sug.regel_4_b + this.sug.regel_4_c;
      this.pre.regel_5 = this.sug.regel_5_a + this.sug.regel_5_b + this.sug.regel_5_c;
      this.pre.regel_6 = this.sug.regel_6_a + this.sug.regel_6_b + this.sug.regel_6_c;
      this.pre.regel_7 = this.sug.regel_7_a + this.sug.regel_7_b + this.sug.regel_7_c;
      this.pre.regel_8 = this.sug.regel_8_a + this.sug.regel_8_b + this.sug.regel_8_c;
      this.pre.regel_9 = this.sug.regel_9_a + this.sug.regel_9_b + this.sug.regel_9_c;
      this.pre.regel_10 = this.sug.regel_10_a + this.sug.regel_10_b + this.sug.regel_10_c;
      this.pre.regel_11 = this.sug.regel_11_a + this.sug.regel_11_b + this.sug.regel_11_c;
      this.pre.regel_12 = this.sug.regel_12_a + this.sug.regel_12_b + this.sug.regel_12_c;
      this.pre.regel_13 = this.sug.regel_13_a + this.sug.regel_13_b + this.sug.regel_13_c;
      this.pre.regel_14 = this.sug.regel_14_a + this.sug.regel_14_b + this.sug.regel_14_c;
      this.pre.regel_15 = this.sug.regel_15;
      this.pre.regel_16 = this.sug.regel_16;
      this.pre.regel_17 = this.sug.regel_17;
      this.pre.regel_18 = this.sug.regel_18;
      this.pre.regel_19 = this.sug.regel_19;
      this.pre.regel_20 = this.sug.regel_20;
      this.pre.regel_21 = this.sug.regel_21;
      this.pre.regel_22 = this.sug.regel_22;
      this.pre.regel_23 = this.sug.regel_23;
      this.pre.regel_24 = this.sug.regel_24;
      this.pre.regel_25 = this.sug.regel_25;
      this.pre.regel_26 = this.sug.regel_26;
      this.pre.regel_27 = this.sug.regel_27;
      this.pre.regel_28 = this.sug.regel_28;
      this.pre.regel_29 = this.sug.regel_29;
      this.pre.regel_30 = this.sug.regel_30;
      this.pre.regel_31 = this.sug.regel_31;
      this.pre.regel_32 = this.sug.regel_32;
      this.pre.regel_33 = this.sug.regel_33;
}

  openModal(template: TemplateRef<any>) { this.modalRef = this.modalService.show(template); }
}

