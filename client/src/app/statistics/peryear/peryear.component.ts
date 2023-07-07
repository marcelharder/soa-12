import { Component, Input, OnInit } from '@angular/core';
import { GraphModel } from 'src/app/_models/GraphModel';

@Component({
  selector: 'app-peryear',
  templateUrl: './peryear.component.html',
  styleUrls: ['./peryear.component.css']
})
export class PeryearComponent implements OnInit {


  @Input() gm: GraphModel;
  title = "";
  type = "ColumnChart";
  data = [];
  columnNames = [];
  width = 1000;
  height = 500;
  options = { hAxis: { title: 'Years' }, vAxis: { title: '# cases' },};

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
