import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { DischargeComponent } from '../discharge/discharge.component';


@Injectable()

// tslint:disable-next-line:class-name
export class changesDischarge implements CanDeactivate<DischargeComponent>{
    canDeactivate(component: DischargeComponent) {
        if (component.testForm.dirty) {
            const can = component.canDeactivate();
            return can;
        }
        return true;
    }
}