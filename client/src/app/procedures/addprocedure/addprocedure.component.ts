import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map, take } from 'rxjs/operators';
import { dropItem } from 'src/app/_models/dropItem';
import { ProcedureDetails } from 'src/app/_models/procedureDetails';
import { AccountService } from 'src/app/_services/account.service';
import { ProcedureService } from 'src/app/_services/procedure.service';

@Component({
  selector: 'app-addprocedure',
  templateUrl: './addprocedure.component.html',
  styleUrls: ['./addprocedure.component.css']
})
export class AddprocedureComponent implements OnInit {
  currentUserName = '';
  currentUserId = 0;
  selectedPatientNo = 0;
  selectedProcedureNo = 0;
  selectedRef = 0;
  sequence = '';
  addResult = 0;
  db = 1;
  refphysicians: Array<dropItem> = [];
  doINeedTimeOut = 'No';

  procedure: ProcedureDetails = {
    procedureId: 0, description: '', dateOfSurgery: new Date(), hospital: 0, patientId: 0, fdType: 0,
    sequence: '', refPhys: '', selectedSurgeon: 0, selectedResponsibleSurgeon: 0,selectedAnaesthesist: 0, selectedAssistant: 0, selectedPerfusionist: 0,
    selectedNurse1: 0, selectedNurse2: 0, surgeryBeforeNextWorkingDay: false, selectedTiming: 0,
    selectedUrgentTiming: 0, selectedEmergencyTiming: 0, selectedStartHr: 0, selectedStartMin: 0, selectedStopHr: 0,
    selectedStopMin: 0, totalTime: 0, selectedInotropes: 0, selectedPacemaker: 0, selectedPericard: 0,
    selectedPleura: 0, comment1: '', comment2: '', comment3: ''
};
  constructor(
    private alertify: ToastrService, 
    private proc: ProcedureService,
    private router: Router, 
    private auth: AccountService) { }

  ngOnInit(): void {
    this.auth.currentUser$.pipe(take(1)).subscribe((u) => {this.currentUserId = u.UserId;})
  }


  showProcedure() { if (this.selectedPatientNo === 0) { return false; } else { return true; } }
  setSelectedRefPhys(pan: number){ this.selectedRef = pan; }


  setProcedureNo(pid: number) {// receives the selected procedure id which is fdType
    this.selectedProcedureNo = pid;
}

  setPatientNo(pan: number) {// receives the selected patient id which is patientId
    this.selectedPatientNo = pan; // if this selectedPatientNo !== 0 then show the procedure add page
    this.sequence = '1';
}

routeToProcedureDetails() {

  /* if (this.doINeedTimeOut === 'Yes') {
    this.alertify.warning('You already added this procedure');
    this.doINeedTimeOut = 'No';
    this.router.navigate(['/procedureDetails', this.addResult]);
  }; */


  this.procedure.fdType = this.selectedProcedureNo;
  this.procedure.sequence = this.sequence;
  this.procedure.refPhys = this.selectedRef.toString();
  this.procedure.patientId = this.selectedPatientNo;
  this.procedure.selectedSurgeon = this.currentUserId;
  this.procedure.selectedAnaesthesist = 0;
  this.procedure.selectedAssistant = 0;
  this.procedure.selectedPerfusionist = 0;
  this.procedure.selectedNurse1 = 0;
  this.procedure.selectedNurse2 = 0;


  this.proc.addProcedure(this.procedure, this.currentUserId, this.selectedPatientNo).subscribe(response => {
      this.addResult = response.procedureId;
      this.alertify.show('New procedure added');
      this.doINeedTimeOut = 'Yes';
      this.auth.setCurrentProcedure(response.procedureId);
      this.router.navigate(['/procedureDetails']);
  });
}





}
