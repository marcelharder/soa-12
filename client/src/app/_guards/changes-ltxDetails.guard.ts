import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { LtxComponent } from '../procedures/ltx/ltx.component';


@Injectable()

// tslint:disable-next-line:class-name
export class changesLtxDetails implements CanDeactivate<LtxComponent>{
    canDeactivate(component: LtxComponent) {
        if (component.ltxForm.dirty) {
            const can = component.canDeactivate();
            return can;
        }
        return true;
    }
}