import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { ValveRepairComponent } from '../procedures/valveRepair/valveRepair.component';


@Injectable()


export class changesValveRepairDetails implements CanDeactivate<ValveRepairComponent>{
    canDeactivate(component: ValveRepairComponent) {
        if (component.valveRepairForm.dirty) {
            const can = component.canDeactivate();
            return can;
        }
        return true;
    }
}