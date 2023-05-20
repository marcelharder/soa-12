import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { CasesPerMonthModel } from '../_models/CasesPerMonthModel';
import { dropItem } from '../_models/dropItem';
import { GraphModel } from '../_models/GraphModel';
import { AccountService } from '../_services/account.service';
import { DropdownService } from '../_services/dropdown.service';
import { GraphService } from '../_services/graph.service';
import { UserService } from '../_services/user.service';

@Component({
    selector: 'app-statistics',
    templateUrl: './statistics.component.html',
    styleUrls: ['./statistics.component.css']
})
export class StatisticsComponent implements OnInit {
    currentUserId = 0;
    hospitals: Array<dropItem> = [];
    selectedHospital = 0;
    AllButton = '0';
    showGraphNo = 0;
    gm: GraphModel = { dataXas: [], dataYas: [], caption: "" };

    constructor(private drop: DropdownService,
        private router: Router,
        private userservice: UserService,
        private graph: GraphService,
        private auth: AccountService,
        private alertify: ToastrService) { }

    ngOnInit() {

        this.auth.currentServiceLevel$.pipe(take(1)).subscribe((s) => {
            if (s === 1) {
                this.auth.currentUser$.pipe(take(1)).subscribe((u) => { this.currentUserId = u.UserId; });

                this.drop.getHospitals(this.currentUserId).subscribe(response => {
                    this.hospitals = response;
                    this.hospitals.unshift({ value: 0, description: 'All hospitals' });
                }, (error) => { console.log(error); });

                this.userservice.getUser(this.currentUserId).subscribe((response) => {
                    this.selectedHospital = response.hospital_id;
                    this.getAG();
                })
            } else {
                this.router.navigate(['/']);
                this.alertify.error("You need a premium subscription ...")
            }
        })
    }


    getVL() { this.graph.getVlad(this.currentUserId, this.selectedHospital).subscribe((next) => { this.gm = next; this.showGraphNo = 1; }); }
    getCM() { this.graph.getCM(this.currentUserId, this.selectedHospital).subscribe((next) => { this.gm = next; this.showGraphNo = 2; }); }
    getAG() { this.graph.getAge(this.currentUserId, this.selectedHospital).subscribe((next) => { this.gm = next; this.showGraphNo = 3; }); }
    getEU() { this.graph.getBand(this.currentUserId, this.selectedHospital).subscribe((next) => { this.gm = next; this.showGraphNo = 4; }); }
    getPY() { this.graph.getPY(this.currentUserId, this.selectedHospital).subscribe((next) => { this.gm = next; this.showGraphNo = 5; }); }
    getPM() { this.graph.getPM(this.currentUserId, this.selectedHospital).subscribe((next) => { this.gm = next; this.showGraphNo = 6; }); }
}

