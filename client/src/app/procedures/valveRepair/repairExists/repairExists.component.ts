import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { dropItem } from 'src/app/_models/dropItem';
import { User } from 'src/app/_models/User';
import { Valve } from 'src/app/_models/Valve';
import { AccountService } from 'src/app/_services/account.service';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { HospitalService } from 'src/app/_services/hospital.service';
import { UserService } from 'src/app/_services/user.service';
import { ValveService } from 'src/app/_services/valve.service';

@Component({
  selector: 'app-repairExists',
  templateUrl: './repairExists.component.html',
  styleUrls: ['./repairExists.component.css']
})
export class RepairExistsComponent implements OnInit {
  @Input() procedureValve: Valve;
  @Input() procedureId: number
  @Output() sendNewProcedureValveUp = new EventEmitter<Valve>();

  editCard = false;
  valveDescription = ""; // this is not saved in the valve database, but should come from OVI
  optionsMitralRepairType: Array<dropItem> = [];
  optionsTricuspidRepairType: Array<dropItem> = [];

  currentUserId = 0;
  currentHospitalNo = 0;
  oviHospital = false;
  

  constructor(
    private valveService: ValveService,
    private alertify: ToastrService,
    private hos: HospitalService,
    private auth: AccountService,
    private user: UserService,
    private router: Router,
    private drops: DropdownService) { }

  ngOnInit() {
    if (this.procedureValve.valveDescription === null) {
      this.valveService.getValveDescription(this.procedureValve.MODEL).subscribe(
        (response) => { this.valveDescription = response; });
    }
    else { this.valveDescription = this.procedureValve.valveDescription; }
    this.loadDrops();
  }
  loadDrops() {
    let d = JSON.parse(localStorage.getItem('MitralRepairType'));
    if (d == null || d.length === 0) {
      this.drops.getMitralValveRepair().subscribe((response) => {
        this.optionsMitralRepairType = response;
        localStorage.setItem('MitralRepairType', JSON.stringify(response));
      });
    } else {
      this.optionsMitralRepairType = JSON.parse(localStorage.getItem('MitralRepairType')
      );
    }
    d = JSON.parse(localStorage.getItem('TricuspidRepairType'));
    if (d == null || d.length === 0) {
      this.drops.getTricuspidValveRepair().subscribe((response) => {
        this.optionsTricuspidRepairType = response;
        localStorage.setItem('TricuspidRepairType', JSON.stringify(response));
      });
    } else {
      this.optionsTricuspidRepairType = JSON.parse(localStorage.getItem('TricuspidRepairType')
      );
    }

  }
  getRepairDescription(test: string) {
    if (test !== null) {
      var index = 0;
      if (this.procedureValve.Implant_Position === 'Mitral') {
        index = this.optionsMitralRepairType.findIndex(x => x.value === +test);
        return this.optionsMitralRepairType[index].description;
      } else {
        index = this.optionsTricuspidRepairType.findIndex(x => x.value === +test);
        return this.optionsTricuspidRepairType[index].description;
      };
    } return "n/a";
  }
  receiveEditValve(t: Valve) {
    this.procedureValve = t;
    this.editCard = false;
    this.sendNewProcedureValveUp.emit(t);
  }
  showEditCard() { return this.editCard; }
  showExistingRing() { if (this.procedureValve.RING_USED === 'true') { return true } }
  editRepair() {
    // mark this valve as ready to be implanted again
    this.valveService.markValve(this.procedureValve.SERIAL_IMP, 0, 0).subscribe((next) => { this.alertify.show(next) })
    this.editCard = true;
  }
  deleteRepair() {
    // mark this valve as ready to be implanted again only when it originates from the OVI
    this.auth.currentUser$.pipe(take(1)).subscribe((u) => { this.currentUserId = u.UserId; });
    this.user.getUser(this.currentUserId).subscribe((next) => {
      this.currentHospitalNo = next.hospital_id;
      this.hos.IsThisHospitalUsingOVI(next.hospital_id).subscribe((r) => { 
        this.oviHospital = r;
        if(this.oviHospital){
          this.valveService.markValve(this.procedureValve.SERIAL_IMP, 0, 0).subscribe((next) => { this.alertify.show(next) })
          }
      })
    });


    this.valveService.deleteValve(this.procedureValve.Id).subscribe((next) => {
      this.alertify.info(next);
    }, error => { this.alertify.error(error); }, () => {
      this.router.navigate(['procedureDetails/valverepair/' + this.procedureId])
    })
  }

}
