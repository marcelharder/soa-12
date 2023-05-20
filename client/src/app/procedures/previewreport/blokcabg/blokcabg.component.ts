import { Component, Input, OnInit } from '@angular/core';
import { CABG } from 'src/app/_models/CABG';
import { dropItem } from 'src/app/_models/dropItem';
import { DropdownService } from 'src/app/_services/dropdown.service';

@Component({
  selector: 'app-blokcabg',
  templateUrl: './blokcabg.component.html',
  styleUrls: ['./blokcabg.component.css']
})
export class BlokcabgComponent implements OnInit  {
  @Input() ca: CABG;

  arteryList: Array<dropItem> = [];
  qualityList: Array<dropItem> = [];
  angleList: Array<dropItem> = [];
  diameterList: Array<dropItem> = [];



  constructor(private dropService: DropdownService) { }


  ngOnInit() {
    this.loadArrays();

  }



  showRegel_2() { if (this.ca.b2_site.toString() !== '0') { return true; } else { return false; } }
  showRegel_3() { if (this.ca.b3_site.toString() !== '0') { return true; } else { return false; } }
  showRegel_4() { if (this.ca.b4_site.toString() !== '0') { return true; } else { return false; } }
  showRegel_5() { if (this.ca.b5_site.toString() !== '0') { return true; } else { return false; } }
  showRegel_6() { if (this.ca.b6_site.toString() !== '0') { return true; } else { return false; } }


  loadArrays(){

    let d = JSON.parse(localStorage.getItem('cabg_locatie'));
    if (d == null || d.length === 0) {
        this.dropService.getCabgLocatie().subscribe((response) => {
          this.arteryList = response; localStorage.setItem('cabg_locatie', JSON.stringify(response));
        });
    } else {
      this.arteryList = JSON.parse(localStorage.getItem('cabg_locatie'));
    }
    d = JSON.parse(localStorage.getItem('cabg_quality'));
    if (d == null || d.length === 0) {
        this.dropService.getCabgQuality().subscribe((response) => {
          this.qualityList = response; localStorage.setItem('cabg_quality', JSON.stringify(response));
        });
    } else {
      this.qualityList = JSON.parse(localStorage.getItem('cabg_quality'));
    }
    d = JSON.parse(localStorage.getItem('cabg_angle'));
    if (d == null || d.length === 0) {
        this.dropService.getCabgAngle().subscribe((response) => {
          this.angleList = response; localStorage.setItem('cabg_angle', JSON.stringify(response));
        });
    } else {
      this.angleList = JSON.parse(localStorage.getItem('cabg_angle'));
    }
    d = JSON.parse(localStorage.getItem('cabg_diameter'));
    if (d == null || d.length === 0) {
        this.dropService.getCabgDiameter().subscribe((response) => {
          this.diameterList = response; localStorage.setItem('cabg_diameter', JSON.stringify(response));
        });
    } else {
      this.diameterList = JSON.parse(localStorage.getItem('cabg_diameter'));
    }
  }


  getArtery(sel: string) {
    if (sel !== null) {
      const _sel = parseInt(sel,10);
      // const help = this.arteryList.findIndex(s => s.value === _sel);
      const result = this.arteryList[_sel].description;
      return result;}
      else { return 'n/a'; }
   }

  getAngle(sel: string) {
    if (sel !== null) {
      const _sel = parseInt(sel,10);
     // const help = this.angleList.findIndex(s => s.value === _sel);
      const result = this.angleList[_sel].description;
      return result; }
    else { return 'n/a'; }
  }

  getQuality(sel: string) {
    if (sel !== null) {
      const _sel = parseInt(sel,10);
     // const help = this.qualityList.findIndex(s => s.value === _sel);
      const result = this.qualityList[_sel].description;
      return result; }
    else { return 'n/a';}
  }

  getDiameter(sel: string) {
    if (sel !== null) {
      const _sel = parseInt(sel,10);
    //  const help = this.diameterList.findIndex(s => s.value === _sel);
      const result = this.diameterList[_sel].description;
      return result; }
    else { return 'n/a'; }
  }

}
