import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-tutorials',
  templateUrl: './tutorials.component.html',
  styleUrls: ['./tutorials.component.css']
})
export class TutorialsComponent implements OnInit {
  isFirstOpen = true;
  oneAtATime = true;
  constructor() { }

  ngOnInit() {
  }

  linkToCSD(){ window.open("https://csd-website.azurewebsites.net", "_blank");}

  linkToGettingStarted(){window.open("https://docs.google.com/document/d/127ifGSodq6aJ0TlUikeGByjv8wKpgYmRTXVmsRiEqMg/edit?usp=sharing", "_blank")}

  linkToConfiguration(){window.open("https://docs.google.com/document/d/1vWHd47vurMdpIlmzEIG2RxPj300oX0_1gQp3e4TshJ0/edit?usp=sharing", "_blank")}

  linkToAddingProcedures(){window.open("https://docs.google.com/document/d/1jkdBPY98mJhWnwNReY3LSEz0OkEv0_hogKPKCBP6LfY/edit?usp=sharing", "_blank")}

  linkToReporting(){window.open("https://docs.google.com/document/d/13M-dMYMg-ikCmtFarYyxLs3S2JZY4YhsAZ2n7dLk2FA/edit?usp=sharing", "_blank")}

  linkToStatistics(){window.open("https://docs.google.com/document/d/1dHpsTpBH9RNKliCNt7kXsccqMeBKDbZNZoVrK2ghpM0/edit?usp=sharing", "_blank")}

}
