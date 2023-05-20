import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private auth: AccountService) {
   
    }

  ngOnInit() {  }

  linkToCSD() { window.location.href = "http://77.173.53.32:8046"; }

  
}


