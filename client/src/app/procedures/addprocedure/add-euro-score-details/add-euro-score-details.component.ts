import { Component, OnInit, Output, EventEmitter, ViewChild, TemplateRef } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AddPatient } from 'src/app/_models/AddPatient';
import { UserService } from 'src/app/_services/user.service';
import { User } from 'src/app/_models/User';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ProcedureService } from 'src/app/_services/procedure.service';
import { ProcedureDetails } from 'src/app/_models/procedureDetails';
import { dropItem } from 'src/app/_models/dropItem';
import { Patient } from 'src/app/_models/Patient';
import { ToastrService } from 'ngx-toastr';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { AccountService } from 'src/app/_services/account.service';
import { PatientService } from 'src/app/_services/patient.service';
import { map, take } from 'rxjs/operators';
import { RefPhysService } from 'src/app/_services/refPhys.service';

@Component({
    // tslint:disable-next-line:component-selector
    selector: 'app-addEuroScoreDetails',
    templateUrl: './add-euro-score-details.component.html',
    exportAs: 'addFormDetails'
})
export class AddEuroScoreDetailsComponent implements OnInit {
    @Output() sendPatientId = new EventEmitter();
    @Output() sendRef = new EventEmitter();
    @ViewChild('addFormDetails') addFormDetails: NgForm;
    euroScore: dropItem[];
    currentUserId = 0;

    modalRef: BsModalRef;


    yearsOptions: Array<number> = [];
    creatOptions: Array<number> = [];
    genderOptions: Array<dropItem> = [];
    weightOptions: Array<number> = [];
    heightOptions: Array<number> = [];
    foundProcedures: Array<number> = [];
    procedures: Array<ProcedureDetails> = [];

    decentHis = false;
    currentUser: User;
    mrn = '';
    patient: AddPatient;
    fullPatient: Patient;
    showPanel = false;
    showPrevP = false;
    selectedRef = 0;
    refphysicians: Array<dropItem> = [];



    constructor(private alertify: ToastrService,
        private modalService: BsModalService,
        private drops: DropdownService,
        private refphys: RefPhysService,
        private router: Router,
        private auth: AccountService,
        private userservice: UserService,
        private proc: ProcedureService,
        private patientservice: PatientService) { }


    ngOnInit() {
      this.auth.currentUser$.pipe(take(1)).subscribe((u) => {this.currentUserId = u.UserId;})
      this.userservice.getUser(this.currentUserId).subscribe((next) => {
            this.currentUser = next;
            this.refphys.getRefPhys(this.currentUser.hospital_id).subscribe((nex) => {
                this.refphysicians = nex;
                this.selectedRef = this.refphysicians[0].value;
            });
        });

        this.loadDrops();


        this.patient = {
            mrn: '',
            age: 65,
            gender: '0',
            creat_number: 50,
            weight: 70,
            height: 172,
        }

    }

    findDataInHIS(test: string) { return false; } // this only works if there is a connection with the HIS

    showDetailsPanel() { if (this.showPanel) { return true; } }
    showPreviousProcedures() { if (this.showPrevP) { return true; } }

    openModal(template: TemplateRef<any>) {
        this.modalRef = this.modalService.show(template);
    }

    showDetails(patienttemplate: TemplateRef<any>) {
        if (this.mrn === '') { this.alertify.error("Identification # can't be empty") }
        else {
            if (!this.findDataInHIS(this.mrn)) { // find data in HIS
                this.patientservice.getPatientinDatabase(this.mrn)
                    .subscribe(response => {
                        if (response.toString() === '0') {
                            this.modalRef = this.modalService.show(patienttemplate);
                        } else {
                            if (response.toString() === '1') { // a patient was found, get list of procedure ids
                                this.patientservice.getProceduresFromMRN(this.mrn).subscribe((next) => {
                                    this.foundProcedures = next;
                                    this.procedures = [];
                                    this.showPanel = false;
                                    this.showPrevP = true;
                                    for (let i = 0; i < this.foundProcedures.length; i++) {
                                        this.proc.getProcedure(this.foundProcedures[i]).subscribe((next) => {
                                            this.procedures.push(next);
                                        })
                                    }
                                })
                            }
                        }
                    }, (error) => { this.alertify.error(error) });
            } else {
                // get the data from the hospital
            }
        }
        // if (this.patient.PatientId !== 0) { this.showPanel = true; }
    }

    confirm(): void {
        // show additional patient data entry page
        this.showPanel = true;
        this.showPrevP = false;
        this.modalRef?.hide();
      }
      
      decline(): void {
        this.modalRef?.hide();
      }

    addProcedure() {
        // send the patientId up to the parent
        this.patientservice.getPatientDetails(this.mrn).subscribe((next)=>{
            this.sendPatientId.emit(next.patientId); // send patient up to the parent
            this.sendRef.emit(this.selectedRef);     // send selected ref to the parent
            this.showPanel = false;
            this.showPrevP = false;
            this.alertify.show("Adding to the list");
        })
      
    }


    addNewPatient() {
        if(this.refphysicians.length === 0){this.alertify.error("This hospital should have ref physicians ..")}
        else{





        let error = '';
        if (this.checkAge(this.patient.age)) {
            if (this.patient.gender !== '0') {
                if (this.patient.creat_number > 0) {
                    if (this.patient.height > 0) {
                        if (this.patient.weight > 0) { error = 'Alles compleet'; } else { error = 'Please enter weight'; }
                    } else { error = 'Please enter height'; }
                } else { error = 'Please enter creat'; }
            } else { error = 'Please select gender'; }
        } else { error = 'Please enter age of the patient'; }

        if (error === 'Alles compleet') {
            // add a new record to the patients database
            this.patient.mrn = this.mrn;
            this.patientservice.addPatientToDatabase(this.patient, this.currentUserId)
                .subscribe(result => {
                    this.sendPatientId.emit(result.patientId); // send patient up to the parent
                    this.sendRef.emit(this.selectedRef);       // send selected ref to the parent
                    this.showPanel = false;
                },
                    (err) => {

                        console.log(err);

                    });
        } else { this.alertify.error(error); }
    }}

    loadDrops() {
        for (let _i = 18; _i < 100; _i++) { this.yearsOptions.push(_i); }

        for (let _i = 40; _i < 400; _i++) { this.creatOptions.push(_i); }
        for (let _i = 40; _i < 200; _i++) { this.weightOptions.push(_i); }
        for (let _i = 120; _i < 210; _i++) { this.heightOptions.push(_i); }

        this.drops.getGenderOptions().subscribe((next) => { this.genderOptions = next; });



    }
    save() {
        // this.scoreOptions.getEuroScore
    }
    cancel() {
        this.router.navigate(['/procedures']);
    }
    checkAge(x: number): boolean {
        let help = false;
        if (this.patient.age > 0 && this.patient.age < 100) { help = true; }
        return help;
    }
}

