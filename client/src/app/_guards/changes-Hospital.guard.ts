import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { HospitalsComponent } from '../configuration/hospitals/hospitals.component';


@Injectable()

// tslint:disable-next-line:class-name
export class changesHospital implements CanDeactivate<HospitalsComponent>{
    canDeactivate(component: HospitalsComponent) {
        if (component.hospitalForm.dirty) {
            const can = component.canDeactivate();
            return can;
        }
        return true;
    }
}