import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-online-users',
  templateUrl: './online-users.component.html',
  styleUrls: ['./online-users.component.css']
})
export class OnlineUsersComponent implements OnInit {
 users_online: string[];
 
  constructor(private drop: DropdownService, private route: ActivatedRoute, private pres: PresenceService) { }

  ngOnInit(): void {
    this.pres.onlineUserList$.subscribe((next)=>{
     
      this.users_online = next});


  

  }

  Cancel(){}

 
}
