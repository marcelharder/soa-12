import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { CpbComponent } from '../procedures/cpb/cpb.component';


@Injectable()

// tslint:disable-next-line:class-name
export class changesCPBDetails implements CanDeactivate<CpbComponent>{
    canDeactivate(component: CpbComponent) {
        if (component.cpbForm.dirty) {
            const can = component.canDeactivate();
            return can;
        }
        // if (component.editForm.dirty){ return confirm('Áre you sure you want to continue? Any unsaved changes will be lost ...'); }
        return true;
    }
}