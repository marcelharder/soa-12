import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  oli:Array<string> = [];
  

  constructor(private alertify: ToastrService) { }

  onlineUsers =  new BehaviorSubject<string[]>([]);
  onlineUserList$ = this.onlineUsers.asObservable();


  createHubConnection(user: User){
    this.hubConnection = new HubConnectionBuilder()
    .withUrl(this.hubUrl + 'presence', {
      accessTokenFactory: () => user.Token
    })
    .withAutomaticReconnect()
    .build()

    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on('UserIsOnline', username => {
      this.onlineUserList$.subscribe((next)=>{this.oli = next});
      this.oli.push(username);
      this.setCurrentOnlineUserList(this.oli);
     // this.alertify.info(username + ' has connected')
    });
    this.hubConnection.on('UserIsOffline', username => {
      this.onlineUserList$.subscribe((next)=>{this.oli = next});
      this.oli = this.oli.filter(item => item != username);
      this.setCurrentOnlineUserList(this.oli);
     // this.alertify.warning(username + ' has disconnected')
    });
  }
  stopHubConnection(){
    this.hubConnection?.stop().catch(error => console.log(error));
  }

  setCurrentOnlineUserList(help: string[]){this.onlineUsers.next(help);}
}
