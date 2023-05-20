import { Component, Input, OnInit } from '@angular/core';
import { GraphModel } from 'src/app/_models/GraphModel';

@Component({
  selector: 'app-fiveriskbands',
  templateUrl: './fiveriskbands.component.html',
  styleUrls: ['./fiveriskbands.component.css']
})
export class FiveriskbandsComponent implements OnInit {

  @Input() gm: GraphModel;
  title = "";
  type = "ColumnChart";
  data = [];
  columnNames = [];
  width = 1000;
  height = 500;
  options = { hAxis: { title: 'Risk bands' }, vAxis: { title: 'Euroscore' },};

  constructor() { }

  ngOnInit(): void {
    var num: number = 0;
    var i: number;
    var help: Array<any> = [];
    for (i = num; i < this.gm.dataXas.length; i++) { help.push([this.gm.dataXas[i], this.gm.dataYas[i]]); }
    this.data = help;
   this.title = this.gm.caption;
  }

}
