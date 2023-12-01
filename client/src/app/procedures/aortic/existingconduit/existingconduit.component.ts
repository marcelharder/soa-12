import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Valve } from 'src/app/_models/Valve';
import { ValveService } from 'src/app/_services/valve.service';

@Component({
  selector: 'app-existingconduit',
  templateUrl: './existingconduit.component.html',
  styleUrls: ['./existingconduit.component.css']
})
export class ExistingconduitComponent implements OnInit  {
  @Input() pd:Valve;
  @Output() markAsDeleted = new EventEmitter<string>();
  valveDescription = "";
  edit = 0;

  constructor(private alertify: ToastrService, private vs: ValveService) { }

  ngOnInit() {
    this.vs.getValveTypeDescription(this.pd.MODEL).subscribe((next)=>{
      this.valveDescription = next;
    });
  }

  showEditButton(){if(this.edit === 1){return true;}}

  editExistingConduit(){this.alertify.show("editing ...");}

  deleteExistingConduit(){
    this.vs.deleteValve(this.pd.Id).subscribe((next)=>{
     this.markAsDeleted.emit("1");
    }, (error)=>{this.alertify.error(error);})
    this.alertify.show("deleting ...");}

}
