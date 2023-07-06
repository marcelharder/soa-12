
import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { PreviewreportComponent } from '../procedures/previewreport/previewreport.component';


@Injectable()

// tslint:disable-next-line:class-name
export class changesPreViewReport implements CanDeactivate<PreviewreportComponent>{
    canDeactivate(component: PreviewreportComponent) {
        if (component.preViewForm.dirty) {
            const can = component.canDeactivate();
            return can;
        }
        return true;
    }
}