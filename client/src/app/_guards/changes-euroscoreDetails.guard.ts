import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { EuroscoredetailsComponent } from '../procedures/euroscoredetails/euroscoredetails.component';

@Injectable()

export class changesEuroscoreDetails implements CanDeactivate<EuroscoredetailsComponent>{
    canDeactivate(component: EuroscoredetailsComponent) {
        if (component.editForm.dirty) {
            const can = component.canDeactivate();
            return can;
        }
        //if (component.editForm.dirty){ return confirm('Áre you sure you want to continue? Any unsaved changes will be lost ...'); }
        return true;
    }
}
