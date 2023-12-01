import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { DetailsmainComponent } from '../procedures/detailsmain/detailsmain.component';

@Injectable()

export class ChangesProcedureDetails implements CanDeactivate<DetailsmainComponent>{
    canDeactivate(component: DetailsmainComponent) {
         if (component.editForm.dirty) {
            const can = component.canDeactivate();
            return can;
        }
        // if (component.editForm.dirty){ return confirm('Áre you sure you want to continue? Any unsaved changes will be lost ...'); }
        return true;
    }
}
