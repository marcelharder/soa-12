import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { CABG } from 'src/app/_models/CABG';
import { EmailModel } from 'src/app/_models/EmailModel';
import { previewReport } from 'src/app/_models/previewReport';
import { ProcedureDetails } from 'src/app/_models/procedureDetails';
import { RefPhysModel } from 'src/app/_models/RefPhysModel';
import { reportHeader } from 'src/app/_models/reportHeader';
import { Suggestion } from 'src/app/_models/Suggestion';
import { Valve } from 'src/app/_models/Valve';
import { AccountService } from 'src/app/_services/account.service';
import { CABGService } from 'src/app/_services/cabg.service';
import { FinalReportService } from 'src/app/_services/final-report.service';
import { PreViewReportService } from 'src/app/_services/pre-view-report.service';
import { ProcedureService } from 'src/app/_services/procedure.service';
import { RefPhysService } from 'src/app/_services/refPhys.service';
import { ValveService } from 'src/app/_services/valve.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-previewreport',
  templateUrl: './previewreport.component.html',
  styleUrls: ['./previewreport.component.css']
})
export class PreviewreportComponent implements OnInit {
  @ViewChild('preViewForm') preViewForm: NgForm;
  finalReportCompiled = false;
  currentUserName = '';
  currentHospitalName = "";
  procedureId = 0;
  reportCode = 0;
  locationURL = environment.locationUrl;
  refEmail = '0';


  reportHeader: reportHeader = {
    Id: 0,
    surgeon_picture: '',
    hospital_name: '',
    hospital_image: '',
    hospital_unit: '',
    hospital_dept: '',
    hospital_city: '',
    hospital_number: '',
    patient_name: '',
    physician: '',
    clinical_unit: '',
    title: '',
    diagnosis: '',
    operation: '',
    operation_date: new Date(),
    surgeon: '',
    assistant: '',
    anaesthesiologist: '',
    perfusionist: '',
    Comment_1: '',
    Comment_2: '',
    Comment_3: ''
  };

  prev: previewReport;
  sug: Suggestion;
  proc: ProcedureDetails;
  cabgDetails: CABG;

  ref: RefPhysModel = { Id: 0, hospital_id: 0, name: '', image: '', address: '', street: '', postcode: '', city: '', state: '', country: '', tel: '', fax: '', email: '', send_email: false, active: false };
  email: EmailModel = { id: 0, hospital: '', hours:'', callback:'', from: '', to: '', subject: '', body: '', surgeon: '', phone: '', surgeon_image: '', soort: '', ref_phys: '' };
  MitralValveDetails = { Model: '', Serial: '', Size: '' };
  AorticValveDetails = { Model: '', Serial: '', Size: '' };

  blok1 = 0;
  blok2 = 0;
  cabg = 0;
  aorticvalve = 0;
  mitralvalve = 0;
  blok6 = 0;

  baseUrl = environment.apiUrl;


  constructor(private route: ActivatedRoute,
    private refPhys: RefPhysService,
    private router: Router,
    private auth: AccountService,
    private procedureservice: ProcedureService,
    private cabgService: CABGService,
    private valveService: ValveService,
    private final: FinalReportService,
    private alertify: ToastrService,
    private preview: PreViewReportService) { }

  ngOnInit() {

   
    this.auth.currentServiceLevel$.pipe(take(1)).subscribe((n) => {
        this.auth.currentUser$.pipe(take(1)).subscribe((u) => {this.currentUserName = u.UserName;});
        this.auth.HospitalName.subscribe((n)=>{this.currentHospitalName = n;});
        this.route.data.subscribe(data => {
          this.prev = data.preView;
          this.procedureId = this.prev.procedure_id;
          this.procedureservice.getProcedure(this.procedureId).subscribe((next) => {
            this.proc = next;
            if(this.proc.refPhys != "99")
                {this.refPhys.getSpecificRefPhys(+this.proc.refPhys).subscribe((ne) => {
                  
                  this.ref = ne; })}
            
            this.preview.getReportCode(this.proc.fdType).subscribe((nex) => {
              this.reportCode = nex;
              this.getAdditionalStuff(this.reportCode);// gets the cabg / valve details
            });
          });
        });
        this.preview.getReportHeader(this.procedureId).subscribe((next) => { this.reportHeader = next; });
    });
  }

  acceptMessage(id: number) {// get the button actions from the header component
    switch (id) {
      case 1: this.clearReport(); break;
      case 2: this.composeAndSendMailMessage(); break;
      case 4: this.saveAsSuggestion(); break;
    }
  };

  cancelEmailRef($event: any) {this.refEmail = '0';}

  composeAndSendMailMessage() {
    // check if the report is saved
    // only works when the emailbutton is shown, when refPhys !== '99'
    if (this.finalReportCompiled) {
      
      this.procedureservice.getRefPhysEmailHash(this.proc.procedureId)
        .subscribe((next) => {

          let hash = next;

          this.email.id = this.reportHeader.Id;
          this.email.hours = "24";
          this.email.hospital = this.currentHospitalName;
          this.email.surgeon = this.reportHeader.surgeon;
          this.email.subject = "Procedure notification";
          this.email.ref_phys = this.ref.name;
          this.email.surgeon_image = this.reportHeader.surgeon_picture;
          this.email.to = this.ref.email;
          this.email.phone = this.ref.tel;
          this.email.soort = '1';
          this.email.body = "here is the body";
          this.email.callback = this.locationURL + 'FinalReport/getRefReport/' + hash;

        });
        this.refEmail = '1'; // show the email/sms page
    }
    else { this.alertify.error("Save and print report first ..."); }

  }
  showEmailRefPhys() { if (this.refEmail === '1') { return true; } }

  saveAndPrint(rh: reportHeader) {
    this.finalReportCompiled = true; // to stop the email button if the report is not saved

    this.prev = {
      ...this.prev,
      Diagnosis: rh.diagnosis,
      MedicalRecordNumber: rh.hospital_number,
      patientName: rh.patient_name
    };
    this.final.postReportId(this.prev).subscribe({
      next: () => { },
      error: () => { },
      complete: () => {
        this.preview.isFinalReportPresentable(this.prev.procedure_id).subscribe((next)=>{
          switch(next){
            case 1: window.location.href = `${this.baseUrl}FinalReport/${this.prev.procedure_id}`;break;
            case 2: this.alertify.error("pls fill the cabg details before printing the final report");break;
            case 3: window.location.href = `${this.baseUrl}FinalReport/${this.prev.procedure_id}`;break;
            case 4: this.alertify.error("pls fill the valve details before printing the final report");break;
          }
        })
      }
    });
  }

  clearReport() {
    // wipe out the current content of pre
    this.wipeOutPre();
    // this.preview.resetPreView(this.procedureId).subscribe((next)=>{this.prev = next;})
    this.preview.resetPreView(this.procedureId).subscribe((next) => { this.prev = next; })
  }

  saveAsSuggestion() {
    // save the current preview report details which get transformed to a suggestion on the server
    this.preview.savePreViewSuggestion(this.prev).subscribe((next) => {
      this.alertify.show('Saved as custom suggestion');
    }, (error) => { this.alertify.error(error); });

  }
  wipeOutPre() {
    this.prev.regel_1 = '';
    this.prev.regel_2 = '';
    this.prev.regel_3 = '';
    this.prev.regel_4 = '';
    this.prev.regel_5 = '';
    this.prev.regel_6 = '';
    this.prev.regel_7 = '';
    this.prev.regel_8 = '';
    this.prev.regel_9 = '';
    this.prev.regel_10 = '';
    this.prev.regel_11 = '';
    this.prev.regel_12 = '';
    this.prev.regel_13 = '';
    this.prev.regel_14 = '';
    this.prev.regel_15 = '';
    this.prev.regel_16 = '';
    this.prev.regel_17 = '';
    this.prev.regel_18 = '';
    this.prev.regel_19 = '';
    this.prev.regel_20 = '';
    this.prev.regel_21 = '';
    this.prev.regel_22 = '';
    this.prev.regel_23 = '';
    this.prev.regel_24 = '';
    this.prev.regel_25 = '';
    this.prev.regel_26 = '';
    this.prev.regel_27 = '';
    this.prev.regel_28 = '';
    this.prev.regel_29 = '';
    this.prev.regel_30 = '';
    this.prev.regel_31 = '';
    this.prev.regel_32 = '';
    this.prev.regel_33 = '';



  }

  getAdditionalStuff(code: number) {
    switch (code) {

      case 1: this.getCabgStuff(this.prev.procedure_id); break;
      case 2: this.getCabgStuff(this.prev.procedure_id); break;
      case 3: this.getAorticValveStuff(this.prev.procedure_id); break;
      case 4: this.getMitralValveStuff(this.prev.procedure_id); break;
      case 5: this.getMitralValveStuff(this.prev.procedure_id); this.getAorticValveStuff(this.prev.procedure_id); break;


    }

  }
  getMitralValveStuff(id: number) {
    this.valveService.getValves(id).subscribe((next) => {
      // find the mitralvalve first
      var index = next.findIndex(a => a.Implant_Position === "Mitral");
      // and get model/size/serial
      this.MitralValveDetails.Model = next[index].MODEL;
      this.MitralValveDetails.Size = next[index].SIZE;
      this.MitralValveDetails.Serial = next[index].SERIAL_IMP;
    }, (error) => {
      this.alertify.error(error);
    })
  }
  getAorticValveStuff(id: number) {
    this.valveService.getValves(id).subscribe((next) => {
      // find the aorticvalve first
      var index = next.findIndex(a => a.Implant_Position === "Aortic");
      this.AorticValveDetails.Model = next[index].MODEL;
      this.AorticValveDetails.Size = next[index].SIZE;
      this.AorticValveDetails.Serial = next[index].SERIAL_IMP;
    }, (error) => {
      this.alertify.error(error);
    })
  }
  getCabgStuff(id: number) {
    this.cabgService.getCABG(id.toString()).subscribe((next) => {
      this.cabgDetails = next;
    }, (error) => { this.alertify.error(error); })
  }




  canDeactivate() {

    return true;
  }


}



