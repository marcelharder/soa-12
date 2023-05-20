import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Valve } from 'src/app/_models/Valve';
import { ValveService } from 'src/app/_services/valve.service';

@Component({
  selector: 'app-valve-present',
  templateUrl: './valve-present.component.html',
  styleUrls: ['./valve-present.component.css']
})
export class ValvePresentComponent {
  @Input() pd: Valve;
  @Output() del = new EventEmitter<String>(); 
  

  constructor(private vs: ValveService, private alertify: ToastrService) { }
  
  

  deleteValve() {
    this.vs.deleteValve(this.pd.Id).subscribe((next) => {
      this.del.emit("record deleted");
   }, (error) => { this.alertify.error(error) })
   // unmark the online valve
   this.vs.markValve(this.pd.SERIAL_IMP, 0, 0).subscribe((next) => {
    
    this.alertify.show("Valve unmarked");})
  }

  getValveDescription(m: string): string{
    let result = "";
    if(m !== ""){
      this.vs.getValveDescription(m).subscribe(
        (next) => {result = next;}, 
        (error) => { this.alertify.error(error.error) });
    }
    return result;
    }

}
