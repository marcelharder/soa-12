import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { ValveComponent } from '../procedures/valve/valve.component';


@Injectable()


export class changesValveDetails implements CanDeactivate<ValveComponent>{
    canDeactivate(component: ValveComponent) {
        if (component.valveForm.dirty) {
            const can = component.canDeactivate();
            return can;
        }
        return true;
    }
}