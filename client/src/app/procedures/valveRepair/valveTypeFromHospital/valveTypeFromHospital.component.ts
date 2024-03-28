import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { dropItem } from 'src/app/_models/dropItem';
import { hospitalValve } from 'src/app/_models/hospitalValve';
import { Valve } from 'src/app/_models/Valve';
import { ValveService } from 'src/app/_services/valve.service';


@Component({
  selector: 'app-valveTypeFromHospital',
  templateUrl: './valveTypeFromHospital.component.html',
  styleUrls: ['./valveTypeFromHospital.component.css']
})
export class ValveTypeFromHospitalComponent implements OnInit {
  @Input() optionsAvailableRings: Array<hospitalValve>;
  @Input() procedureValve: Valve;
  @Input() title = "";
  @Output() sendValveUp = new EventEmitter<Valve>();
  ringArrayNotEmpty = true;
  showDetails = false;
  optionRingSizes:Array<any> = [];
  imageUrl = "";
  hv: hospitalValve = {
    ValveTypeId: 0,
    Description: "",
    Implant_position: "Aortic",
    Type: "Biological",
    hospitalId: "0",
    Vendor_code: 0,
    Vendor_description: "",
    Valve_size: null,
    Patch_size: null,
    No: 0,
    Model_code: '',
    uk_code: '',
    soort: 1,
    image: '',
    countries: ''
  }
  
  constructor(private alertify: ToastrService, private valveService: ValveService) { }

  ngOnInit() {
   if (this.optionsAvailableRings.length !== 0){this.ringArrayNotEmpty = true;}
    this.getRingSizes();
  
  
  }

  recordsFound(){return this.ringArrayNotEmpty;}

  showNoRings(){if(this.optionsAvailableRings.length === 0){return true;}}

  displayDetails(){return this.showDetails;}
  

  getThisValveType(vt: number){
    this.showDetails = true;
    // get the details from this type of valve
    this.valveService.getSpecificValveType(vt).subscribe(
      (next) => {
        this.hv = next;
        this.procedureValve.MODEL = next.uk_code;
        this.procedureValve.valveDescription = next.Description;
        this.imageUrl = next.image;
        this.optionRingSizes = next.Valve_size;
       })
      this.alertify.warning("no:" + vt);
      this.showDetails = true;
  
  }

  getRingSizes(){this.valveService.getValveCodeSizes(this.hv.ValveTypeId).subscribe((next)=>{this.optionRingSizes = next;}) }
  saveRepair(){ this.sendValveUp.emit(this.procedureValve);}
  cancelEditRepair(){}

}
