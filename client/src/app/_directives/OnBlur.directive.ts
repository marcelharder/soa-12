import { Directive, EventEmitter, HostListener, Output } from '@angular/core';


@Directive({
  selector: '[OnBlur]'
})
export class OnBlurDirective {

  constructor() { }

  @Output('onBlur') onBlurEvent: EventEmitter<any> = new EventEmitter();

  @HostListener('focusout',['$event.target'])
  onFocusout(target){
    console.log("Focus out called");
    this.onBlurEvent.emit()
  }

}
