import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ProcedureService } from 'src/app/_services/procedure.service';

@Component({
  selector: 'app-procedurePictures',
  templateUrl: './procedurePictures.component.html',
  styleUrls: ['./procedurePictures.component.css']
})
export class ProcedurePicturesComponent implements OnInit {
  param1 = 0;
  param2 = 0;

  constructor(
    private proc: ProcedureService,
    private route: ActivatedRoute,
    private alertify: ToastrService,
    
  ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.param1 = +params['id'];
      this.param2 = +params['soort'];
    })
    
   
  }

}
