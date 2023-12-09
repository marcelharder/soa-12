import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { dropItem } from 'src/app/_models/dropItem';
import { Employee } from 'src/app/_models/Employee';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-editdetails',
  templateUrl: './editdetails.component.html',
  styleUrls: ['./editdetails.component.css']
})
export class EditdetailsComponent implements OnInit {

  @Input() emp: Employee;
  @Output() savePhoto = new EventEmitter<string>();

  

  baseUrl = environment.apiUrl;
  targetUrl = '';
  

  optionsYN: Array<dropItem> = [];


  constructor(private drops: DropdownService,) { }

  ngOnInit(): void {
      this.loadDrops();
  }

   IsLoaded() {
      if (this.emp.id !== 0) {
          this.targetUrl = this.baseUrl + 'Employee/addEmployeePhoto/' + this.emp.id;
          return true;
      } else { return false; }
  }

  loadDrops() {
      const d = JSON.parse(localStorage.getItem('YN'));
      if (d == null || d.length === 0) {
          this.drops.getYNOptions().subscribe((response) => {
              this.optionsYN = response; localStorage.setItem('YN', JSON.stringify(response));
          });
      } else {
          this.optionsYN = JSON.parse(localStorage.getItem('YN'));
      }
  }

  updatePhoto(photoUrl: string) {this.savePhoto.emit(photoUrl);}


  cancel() { }
}
