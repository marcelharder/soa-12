import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { AorticComponent } from '../procedures/aortic/aortic.component';


@Injectable()

// tslint:disable-next-line:class-name
export class changesAorticDetails implements CanDeactivate<AorticComponent>{
    canDeactivate(component: AorticComponent) {
        if (component.aorticForm.dirty) {
            const can = component.canDeactivate();
            return can;
        }
        return true;
    }
}