import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { dropItem } from 'src/app/_models/dropItem';
import { EmailModel } from 'src/app/_models/EmailModel';
import { SMSModel } from 'src/app/_models/SMSModel';
import { RefPhysService } from 'src/app/_services/refPhys.service';

@Component({
  selector: 'app-email-ref-phys',
  templateUrl: './email-ref-phys.component.html',
  styleUrls: ['./email-ref-phys.component.css']
})
export class EmailRefPhysComponent implements OnInit {
  @Input() email: EmailModel;
  @Output() sendCancel = new EventEmitter<string>();
  sms: SMSModel = { From: '', To: '', Text: ''};
  smsFlag = 0;
  emailFlag = 1;
  editPanel = 0;
  editSMSPanel = 0;
  selectedMethod = 'Email';

  selectComMethod: Array<dropItem> = [];

  constructor(private refPhys: RefPhysService, private alertify: ToastrService,) { }

  ngOnInit() {
    this.loadDrops();
  }

  loadDrops() {

    this.selectComMethod.push({ value: 1, description: 'Email' });
    this.selectComMethod.push({ value: 2, description: 'SMS' });

  }

  selectMethod() {


    if (this.selectedMethod === 'Email') {

      // send message as email
      this.editPanel = 0;
      this.editSMSPanel = 0;
      this.smsFlag = 0;
      this.emailFlag = 1;
    };
    if (this.selectedMethod === 'SMS') {
      // send message as text
      this.editPanel = 0;
      this.editSMSPanel = 0;
      this.smsFlag = 1;
      this.emailFlag = 0;
      // get the phone number of this refPhys
      this.sms.To = this.email.phone;
      this.sms.Text = this.email.body;
      
    }
  }

  editText() {
    this.editPanel = 1;
    this.editSMSPanel = 0;
    this.smsFlag = 0;
    this.emailFlag = 1;

  }
  editSMSText() {
    this.editPanel = 0;
    this.editSMSPanel = 1;
    this.smsFlag = 1;
    this.emailFlag = 0;



  }

  Cancel() { this.sendCancel.emit('1'); }

  showEditPanel() { if (this.editPanel === 1) { return true; } }
  showEditSMSPanel() { if (this.editSMSPanel === 1) { return true; } }
  showSMS() { if (this.smsFlag === 1) { return true; } }
  showEmail() { if (this.emailFlag === 1) { return true; } }


  Send() {
    
      this.refPhys.sendEmail(this.email).subscribe((nex) => {
      this.alertify.success("Procedure notification sent");
      this.sendCancel.emit('1');
    },
      (error) => {this.alertify.error('Sending email failed ...' + error); });
  }

  SendSMS() {
    if (this.sms.To !== '' && this.sms.Text !== '') {
      this.refPhys.sendSMS(this.sms).subscribe((nex) => {
        this.alertify.show(nex);
        this.sendCancel.emit('1');
      },
        (error) => {this.alertify.error('Sending sms failed ...' + error); });
    } else { this.alertify.error('Phone or Text cannot be empty ...') }
  }

}