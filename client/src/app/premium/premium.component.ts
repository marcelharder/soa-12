import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { dropItem } from '../_models/dropItem';
import { SubscriptionExtensionModel } from '../_models/SubscriptionExtensionModel';
import { AccountService } from '../_services/account.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-premium',
  templateUrl: './premium.component.html',
  styleUrls: ['./premium.component.css']
})
export class PremiumComponent implements OnInit {
  additionalComments = "";
  modalRef: BsModalRef;
  commitTime: any = {};

  vrijeText = "";
  money = 0.00;
  optionTimePeriod: Array<dropItem> = [];

  check_free_1 = true;
  check_free_2 = false;
  check_free_3 = false;
  check_free_4 = false;
  check_free_5 = false;

  check_premium_1 = true;
  check_premium_2 = true;
  check_premium_3 = true;
  check_premium_4 = true;
  check_premium_5 = true;

  req = 0;
  currentUserName = '';
  currentUserId = '';

  semodel: SubscriptionExtensionModel = {
    to : '',
    extensionPeriod:'',
    userName: '',
    userId: "0",
    additionalComments:  '',
    subject:  '',
   
   }

  constructor(
    private modalService: BsModalService,
    private auth: AccountService,
    private alertify: ToastrService, 
    private router: Router, 
    private userService: UserService) { }

  ngOnInit() {

    this.loadDrops();
     this.commitTime = 0;
    

     // get currentUserName and currentUserId
     this.auth.currentUser$.pipe(take(1)).subscribe((u) => {
      this.currentUserName = u.UserName;
      this.currentUserId = u.UserId.toString();
  });
    
  }

  calculateFee(id: number){
    this.money = id;
    this.semodel.subject = "requested tier: " + this.money + " Euro";
  }

  loadDrops(){
    this.optionTimePeriod.push(
      {value:0, description: 'Choose'},
      {value:50, description: '3 months'},
      {value:99, description: '6 months'},
      {value:190, description: '1 year'})
  }

  showPRequest(){if(this.req === 1){return true;} else {return false;}}



  btnClick(no: number) {

    switch (no) {
      case 1: this.showPage("reporting"); break; // reporting
      case 2: this.showPage("configuration"); break; // configuration
      case 3: this.showPage("statistics"); break; // statistics
      
    }
  }
  
  showPage(arg0: string): void {
    const pages = {
      reporting: "https://docs.google.com/document/d/13M-dMYMg-ikCmtFarYyxLs3S2JZY4YhsAZ2n7dLk2FA/edit?usp=sharing",
      configuration: "https://docs.google.com/document/d/1vWHd47vurMdpIlmzEIG2RxPj300oX0_1gQp3e4TshJ0/edit?usp=sharing",
      statistics: "https://docs.google.com/document/d/1dHpsTpBH9RNKliCNt7kXsccqMeBKDbZNZoVrK2ghpM0/edit?usp=sharing"
    };
  
    if (arg0 in pages) {
      window.open(pages[arg0], "_blank");
    } else {
      console.error(`Page ${arg0} not found.`);
    }
  }
  

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }
  
  confirm(): void {
    if(this.commitTime != 0){
    // fill in all the details we need for the email
    this.semodel.extensionPeriod = this.commitTime;
    this.semodel.to = "marcelharder@protonmail.com";
    this.semodel.additionalComments = this.additionalComments;
    this.semodel.userId = this.currentUserId;
    this.semodel.userName = this.currentUserName;

    
    this.userService.sendExtensionRequest(this.semodel).subscribe((next)=>{
      this.alertify.success("Extension Request succesfully sent");
    }, error =>{this.alertify.error(error);});
    this.router.navigate(['/']);
    this.modalRef?.hide();
    } else {this.alertify.error("Please select a extension period ...");}
  }
  
  decline(): void {
    this.modalRef?.hide();
  }

  RequestPremium() {this.req = 1; }
  Cancel() { 
   // go back to the requesting page
   this.router.navigate(['/profile'])

  }

}
