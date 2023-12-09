import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { dropItem } from 'src/app/_models/dropItem';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class EditComponent implements OnInit  {
  selectedPerson = "";
  @Input() profession: string;
  @Input() list: Array<dropItem>;
  @Output() edit = new EventEmitter<string>();
  @Output() add = new EventEmitter<string>();
  editButton = false;

  constructor(private alertify: ToastrService) {

  }
  ngOnInit(): void {
      // this.selectedPerson = this.list[0].description;
   }

   showEditButton(){
    let help = false;
    if(this.list.length > 1 || this.selectedPerson !== "0"){help = true;}
    return help;
   }

  checkEditButton(){
   if(this.showEditButton()){this.editButton = true;}}

  displayEditButton(){return this.editButton}


  editEmployee(id: number) {
    if(this.selectedPerson !== "0"){
    this.edit.emit(id.toString());}
    else {this.alertify.warning("Select a valid person first ...")}
  
  }
  addEmployee() { this.add.emit(this.profession); }
}
