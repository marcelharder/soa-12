import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { reportHeader } from 'src/app/_models/reportHeader';
import { AccountService } from 'src/app/_services/account.service';
import { PatientService } from 'src/app/_services/patient.service';
import { ProcedureService } from 'src/app/_services/procedure.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
    selector: 'app-report-header',
    templateUrl: './report-header.component.html',
    styleUrls: ['./report-header.component.css']
})
export class ReportHeaderComponent {
    @Input() refPhysNo: string;
    @Input() rh: reportHeader;
    @Output() sendMessage = new EventEmitter();
    @Output() sendHeader: EventEmitter<reportHeader> = new EventEmitter<reportHeader>();
    header31 = 'Hospital number';
    header32 = 'Patient name:';
    header33 = 'Preop diagnosis';
    header34 = 'Clinical Unit: Cardiac Services';

    constructor(private userservice: UserService,
        private proceduredetails: ProcedureService,
        private alertify: ToastrService,
        private patientService: PatientService,
        private auth: AccountService,
        private router: Router) { }

    // OperativeDetails() { this.router.navigate(['/procedureDetails/' + this.rh.Id]); }
    Clear() { this.sendMessage.emit(1); }
    Email() {
        // see if the privacy data are filled and the demo user is not here
        if(this.isNotDemoUser()){
        if (this.isPrivacyFilled()) {this.sendMessage.emit(2);
        } else { this.alertify.error('Please complete patient details, otherwise the report makes no sense ...') }
         
    } else {this.alertify.error('The demo user cannot SMS or email ...')}}
    SaveAndPrint() {
        // see if the privacy data are filled
        if (this.isPrivacyFilled()) {
             this.sendHeader.emit(this.rh); 
        } else { this.alertify.error('Please complete patient details, otherwise the report makes no sense ...') }

    }
    SaveSuggestion() { this.sendMessage.emit(4); }

    isPrivacyFilled(): boolean {
        let result = false;
        if (this.rh.hospital_number !== "Hospital No:"
            && this.rh.patient_name !== null
            && this.rh.diagnosis !== "") { result = true; }
        return result;
    }
    isNotDemoUser(): boolean {
        let result = true;
        let username = "";
        this.auth.currentUser$.subscribe((next) => { username = next.UserName });
        if (username === "demo") { result = false; }
        return result;
    }
    showEmailButton(){if(this.refPhysNo === "99"){return false;} else {return true;}}



}
