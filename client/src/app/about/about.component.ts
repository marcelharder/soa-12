import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {
  contact = 0;
  version = 1;
  credits = 0;

  constructor() { }

  ngOnInit(): void {
    
  }

  displayCredits(){
   this.credits = 1;
   this.contact = 0; 
   this.version = 0;
  }

  displayStart(){
    this.credits = 0;
    this.contact = 0; 
    this.version = 1;
   }
  

  switchPanel(t: number){
    switch (t) {
      case 1: this.contact = 1; this.version = 0;this.credits = 0;break;
      case 2: this.contact = 0; this.version = 1;this.credits = 0;break;
    }
  }

  showCredits(){if(this.credits === 1){return true;}}
  showContact(){if(this.contact === 1){return true;}}
  showVersion(){if(this.version === 1){return true;}}

  linkToCSD(){ window.location.href = "https://csd-website.azurewebsites.net";}
  linkToCredit01(){window.location.href = "https://docs.google.com/document/d/1vLW1bOdBKDYKP-GqfAAlo43xttomhjTniMjOSxUI01c/edit?usp=sharing";}
  linkToCredit02(){window.location.href = "https://docs.google.com/document/d/1a8WQtk5av7YUR9P8oQKNR74S0lrzAaf-V9q5Kic10f8/edit?usp=sharing";}

}
