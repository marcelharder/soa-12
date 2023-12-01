import { Component, EventEmitter, Input, OnInit, Output, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.css']
})
export class TextInputComponent implements ControlValueAccessor {
  @Input() databaseStatus: boolean;
  @Input() controlNo: number;
  @Input() label: string;
  @Input() type = 'text';
  @Output() ev = new EventEmitter<number>();

  constructor(@Self() public ngControl: NgControl, private alertify: ToastrService) {
    this.ngControl.valueAccessor = this;
  }
  writeValue(obj: any): void {

  }
  registerOnChange(fn: any): void {

  }
  registerOnTouched(fn: any): void {

  }

  checkUserAndereManier(){
     if(!this.databaseStatus && this.controlNo === 1){
      this.alertify.error("User exists already in the database ...");
      this.ev.emit(1); // tell the parent to hide the rest of the page
      } 
      else {this.ev.emit(0);}
  }



}
