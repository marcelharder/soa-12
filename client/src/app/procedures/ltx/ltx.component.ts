import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { dropItem } from 'src/app/_models/dropItem';
import { Ltx } from 'src/app/_models/Ltx';
import { DropdownService } from 'src/app/_services/dropdown.service';
import { LtxService } from 'src/app/_services/ltx.service';

@Component({
  selector: 'app-ltx',
  templateUrl: './ltx.component.html',
  styleUrls: ['./ltx.component.css']
})
export class LtxComponent implements OnInit {
  @ViewChild('ltxForm') ltxForm: NgForm;
  proc:Ltx;

  optionsTypeTx:Array<dropItem> = [];
  optionsIndication:Array<dropItem> = [];
  hours:Array<string>=[];
  minutes:Array<string>=[];

  constructor(private alertify: ToastrService,
    private route: ActivatedRoute,
    private drops: DropdownService,
    private ltxService: LtxService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {this.proc = data.ltx;},);
    this.loadDrops();
  }

  loadDrops(){
    let d = JSON.parse(localStorage.getItem('LtxIndication'));
    if (d == null || d.length === 0) {
        this.drops.getLtxIndication().subscribe((response) => {
            this.optionsIndication = response; localStorage.setItem('LtxIndication', JSON.stringify(response));
        });
    } else {
        this.optionsIndication = JSON.parse(localStorage.getItem('LtxIndication'));
    }
    d = JSON.parse(localStorage.getItem('LtxType'));
    if (d == null || d.length === 0) {
        this.drops.getLtxType().subscribe((response) => {
            this.optionsTypeTx = response; localStorage.setItem('LtxType', JSON.stringify(response));
        });
    } else {
        this.optionsTypeTx = JSON.parse(localStorage.getItem('LtxType'));
    }
    d = JSON.parse(localStorage.getItem('hours'));
    if (d == null || d.length === 0) {
        this.drops.getHours().subscribe((response) => {
            this.hours = response; localStorage.setItem('hours', JSON.stringify(response));
        });
    } else {
        this.hours = JSON.parse(localStorage.getItem('hours'));
    }
    d = JSON.parse(localStorage.getItem('minutes'));
    if (d == null || d.length === 0) {
        this.drops.getMins().subscribe((response) => {
            this.minutes = response; localStorage.setItem('minutes', JSON.stringify(response));
        });
    } else {
        this.minutes = JSON.parse(localStorage.getItem('minutes'));
    }

  }

  saveLTX() {
    this.ltxService.saveLTX(this.proc).subscribe();
    this.ltxForm.reset(this.proc);
}
  canDeactivate() {
    this.saveLTX();
    this.alertify.show('saving LTX');
    return true;
}

}

