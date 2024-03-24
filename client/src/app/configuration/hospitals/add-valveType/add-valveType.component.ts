import {
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';
import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import {
  AbstractControl,
  Form,
  FormBuilder,
  FormControl,
  FormGroup,
  NgForm,
  Validators,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CardData } from 'src/app/_models/CardData';
import { dropItem } from 'src/app/_models/dropItem';
import { hospitalValve } from 'src/app/_models/hospitalValve';
import { valveSize } from 'src/app/_models/valveSize';
import { AccountService } from 'src/app/_services/account.service';
import { UserService } from 'src/app/_services/user.service';
import { ValveService } from 'src/app/_services/valve.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-add-valveType',
  templateUrl: './add-valveType.component.html',
  styleUrls: ['./add-valveType.component.css'],
  animations: [
    trigger('cardFlip', [
      state('default', style({ transform: 'none' })),
      state('flipped', style({ transform: 'rotateY(180deg)' })),
      transition('default => flipped', [animate('400ms')]),
      transition('flipped => default', [animate('200ms')]),
    ]),
  ],
})
export class AddValveTypeComponent implements OnInit {
  @ViewChild('addValveForm') addValveForm: NgForm;
  optionsVendors: Array<dropItem> = [];
  valveTypes: Array<dropItem> = [];
  valvePositions: Array<dropItem> = [];

  @Input() new_hv: hospitalValve;
  @Output() newHospitalValve: EventEmitter<hospitalValve> = new EventEmitter();
  @Output() ct: EventEmitter<number> = new EventEmitter();
  selectedVendor = 2;
  targetUrl = '';
  baseUrl = environment.apiUrl;
  showAdd = 0;
  newsize = 0;
  neweoa = 0.0;
  valvesize: valveSize = { SizeId: 0,Size: 0, VTValveTypeId: 0, EOA: 0.0, ValveTypeId: 0};
  listOfSizes: Array<valveSize> = [];

  constructor(
    private vs: ValveService,
    private user: UserService,
    private auth: AccountService,
    private alertify: ToastrService
  ) {}

  ngOnInit() {
    this.loadDrops();

    this.optionsVendors.sort(function (a, b) {
      return ('' + a.description).localeCompare(b.description);
    });
    // need to adjust the endpoint on the inventory container so that it will be anonymous
    // this.vs.getVendors().subscribe((next) => { this.optionsVendors = next; });
  }
  data: CardData = {
    imageId: '',
    state: 'default',
  };
  cardClicked() {
    if (this.data.state === 'default') {
      this.data.state = 'flipped';
    } else {
      this.data.state = 'default';
    }
  }
  displayAdd() {
    if (this.showAdd === 1) {
      return true;
    }
  }

  loadDrops() {
    this.optionsVendors = [
      {
        value: 9,
        description: 'CryoLife',
      },
      {
        value: 7,
        description: 'Atrium',
      },
      {
        value: 3,
        description: 'LivaNova',
      },
      {
        value: 8,
        description: 'Gore',
      },
      {
        value: 2,
        description: 'Abbott',
      },
      {
        value: 6,
        description: 'KFSRC',
      },
      {
        value: 5,
        description: 'Medtronic',
      },
      {
        value: 4,
        description: 'Edwards',
      },
    ];

    this.valveTypes.push({ value: 0, description: 'Biological' });
    this.valveTypes.push({ value: 1, description: 'Mechanical' });
    this.valveTypes.push({ value: 2, description: 'Annuloplasty_Ring' });
    this.valveTypes.push({ value: 3, description: 'Valved_Conduit' });
    this.valveTypes.push({ value: 4, description: 'Homograft' });

    this.valvePositions.push({ value: 0, description: 'Aortic' });
    this.valvePositions.push({ value: 1, description: 'Mitral' });
    this.valvePositions.push({ value: 2, description: 'Tricuspid' });
  }

  flip() {
    this.cardClicked();
  }

  updatePhoto(url: string) {
    this.new_hv.image = url;
  }

  cancel() {
    this.ct.emit(1);
  }

  IsLoaded() {
    if (+this.new_hv.ValveTypeId !== 0) {
      this.targetUrl =
        this.baseUrl + 'Valve/addValveTypePhoto/' + this.new_hv.ValveTypeId;
      return true;
    } else {
      return false;
    }
  }

  SaveNewValveType() {
    var userId = 0;
    var country = '';
    this.auth.currentUser$.subscribe((next) => {
      userId = next.UserId;
      this.user.getUser(userId).subscribe((response) => {
        country = response.country;
        // get vendor description from vendor_code
        if (this.selectedVendor !== 0) {
          var hep = this.optionsVendors.find(
            (x) => x.value == this.selectedVendor
          );
          this.new_hv.Vendor_description = hep.description;
          this.new_hv.Vendor_code = hep.value;
          this.new_hv.countries = country;
          this.cardClicked();
        }
      });
    });
  }

  saveBacktoToDB() {
    if (this.everythingOk()) {
      this.newHospitalValve.emit(this.new_hv);
    }
  }

  addSize() {
    this.showAdd = 1;
  }

  saveSize() {
    if (this.neweoa !== 0) {
      this.showAdd = 0;

      this.valvesize.VTValveTypeId = this.new_hv.ValveTypeId;
      this.valvesize.Size = this.newsize;
      this.valvesize.EOA = this.neweoa;

      // add to the local list
      this.listOfSizes.push(this.valvesize);
      this.listOfSizes.sort(function (a, b) {return a.Size - b.Size;});
      // upload to the database
      this.vs.addValveSize(this.valvesize).subscribe((next)=>{})
    } else {
      this.alertify.error(
        'The effective orfice area is required, because we want to establish possible Patient Prosthesis Mismatch'
      );
    }
  }

  deleteSize(id: number) {
    // remove from the local list
  }

  everythingOk(): boolean {
    // checks if all the fields are properly filled
    var help = true;

    if (this.listOfSizes.length === 0) {
      help = false;
      this.alertify.error('A valve has usually at least one size ...');
    }

    if (this.new_hv.Model_code === undefined) {
      help = false;
      this.alertify.error('Pls enter the model code ...');
    }

    if (this.new_hv.Description === undefined) {
      help = false;
      this.alertify.error('Pls enter the valve description ...');
    }

    return help;
  }
}
