import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { CandA } from 'src/app/_models/CandA';
import { ProcedureDetails } from 'src/app/_models/procedureDetails';
import { AccountService } from 'src/app/_services/account.service';
import { PatientService } from 'src/app/_services/patient.service';
import { ProcedureService } from 'src/app/_services/procedure.service';

@Component({
  selector: 'app-proceduredetails',
  templateUrl: './proceduredetails.component.html',
  styleUrls: ['./proceduredetails.component.css']
})
export class ProceduredetailsComponent implements OnInit {
  currentPatientId = 0;
  dischargeFlag = 0;
 
  procedures: Array<ProcedureDetails> = [];
  currentProcedureId = 0;

  currentHospitalName = "";
  destinationUrl = 'detailsMain';
  procedureDescription = '';
  modalRef: BsModalRef;

  cap: CandA;

  button_1 = 'button 1'; button_2 = 'button 2'; button_3 = 'button 3';
  button_4 = 'button 4'; button_5 = 'button 5'; button_6 = 'button 6';
  button_7 = ''; button_8 = '';


  action_1 = '/about'; action_2 = '/home'; action_3 = '/users';
  action_4 = '/about'; action_5 = '/home'; action_6 = '/users';
  action_7 = ''; action_8 = '';

  procedureDetails: ProcedureDetails;




  constructor(
    private router: Router,
    private procedureService: ProcedureService,
    private patientService: PatientService,
    private modalService: BsModalService,
    private auth: AccountService,
    private alertify: ToastrService) { }

  ngOnInit(): void {

    this.auth.currentHospitalName.subscribe((next) => {
      this.currentHospitalName = next;


    });
    

    this.auth.currentProcedure$.pipe(take(1)).subscribe((u) => {
      this.currentProcedureId = u;

    })
    this.procedureService.getProcedure(this.currentProcedureId).subscribe((result) => {
      this.shouldDischargeButtonBeShown(result.patientId);
      this.procedureDescription = result.description;

      this.procedureService.getButtonsAndCaptions(result.fdType).subscribe(response => {

        this.cap = response;

        this.button_1 = this.cap.button_caption[0];
        this.button_2 = this.cap.button_caption[1];
        this.button_3 = this.cap.button_caption[2];
        this.button_4 = this.cap.button_caption[3];
        this.button_5 = this.cap.button_caption[4];
        this.button_6 = this.cap.button_caption[5];
        this.button_7 = this.cap.button_caption[6];
        this.button_8 = this.cap.button_caption[7];


      }, error => { this.alertify.error(error + ' van mijn'); });
    });
    this.goToDestination(this.destinationUrl);
  }

  shouldDischargeButtonBeShown(patientId: number) {
    // the idea here is that if there are multiple procedures for this patient only the first procedure has a discharge button
      this.patientService.getProceduresFromPatientId(patientId).subscribe((next) => {
        if (next.length <= 1) { this.dischargeFlag = 1; };
        if (next.length > 1) { if (this.currentProcedureId === next[0]) { this.dischargeFlag = 1; } }
      })
  }

  showDischarge() { if (this.dischargeFlag === 1) { return true; } }

  goToDestination(d: string) {

    switch (d) {

      case 'avr': {
        // save this information to the BehaviorSubject, so the valve page can re-arrange itself
        this.auth.changeSoortOperatie('avr');
        this.destinationUrl = 'valve';
        break;
      };
      case 'mvr': {
        // save this information to the BehaviorSubject, so the valve page can re-arrange itself
        this.auth.changeSoortOperatie('mvr');
        this.destinationUrl = 'valve';
        break;
      };
      case 'tvr': {
        // save this information to the BehaviorSubject, so the valve page can re-arrange itself
        this.auth.changeSoortOperatie('tvr');
        this.destinationUrl = 'valve';
        break;
      };
      case 'mvp': {
        // save this information to the BehaviorSubject, so the valve page can re-arrange itself
        this.auth.changeSoortOperatie('mvp');
        this.destinationUrl = 'valverepair';
        break;
      };
      case 'tvp': {
        // save this information to the BehaviorSubject, so the valve page can re-arrange itself
        this.auth.changeSoortOperatie('tvp');
        this.destinationUrl = 'valverepair';
        break;
      };
      case 'previewReport': {
        // check to see if this user has paid or not
        this.auth.currentServiceLevel$.pipe(take(1)).subscribe((n) => {
           if (n === 1) {
            // this means user is okay to go
            this.destinationUrl = 'previewReport';
          } else {
            this.alertify.error('You need a subscription to use reporting');
            this.destinationUrl = 'detailsMain';
          }
        });
        break;
      }
    
      default: { this.destinationUrl = d; break; }
    }
    this.router.navigate(['/procedureDetails', { outlets: { details: [this.destinationUrl, this.currentProcedureId] } }]);

  }
  goDelete(template: TemplateRef<any>) { //ask if the user wants to delete this procedure
    this.modalRef = this.modalService.show(template);
  }
  confirm(): void {
    this.procedureService.deleteProcedure(this.currentProcedureId).subscribe(
      (next) => {
        this.alertify.success('Procedure deleted');
        this.router.navigate(['/procedures']);

      },
      (error) => { this.alertify.error(error) })
    this.modalRef?.hide();
  }
  decline(): void {
    this.modalRef?.hide();
  }

}
