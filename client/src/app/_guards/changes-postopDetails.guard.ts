import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { PostopComponent } from '../procedures/postop/postop.component';


@Injectable()

// tslint:disable-next-line:class-name
export class changesPOSTOPDetails implements CanDeactivate<PostopComponent>{
    canDeactivate(component: PostopComponent) {
        if (component.postopForm.dirty) {
            const can = component.canDeactivate();
            return can;
        }
        return true;
    }
}