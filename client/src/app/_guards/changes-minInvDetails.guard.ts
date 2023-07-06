import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { MininvComponent } from '../procedures/mininv/mininv.component';


@Injectable()

// tslint:disable-next-line:class-name
export class changesMinInv implements CanDeactivate<MininvComponent>{
    canDeactivate(component: MininvComponent) {
        if (component.minForm.dirty) {
            const can = component.canDeactivate();
            return can;
        }
        return true;
    }
}