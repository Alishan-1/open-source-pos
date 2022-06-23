import { ChangeDetectorRef, Component, ElementRef, forwardRef, Input, Renderer2 } from '@angular/core';

import { ToastComponent, ToasterService } from '@coreui/angular';

@Component({
  selector: 'app-pos-toast-simple',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.scss'],
  providers: [{ provide: ToastComponent, useExisting: forwardRef(() => AppPosToastComponent) }]
})
export class AppPosToastComponent extends ToastComponent {

  @Input() closeButton = true;
  @Input() title = '';
  @Input() message = '';

  constructor(
    public override hostElement: ElementRef,
    public override renderer: Renderer2,
    public override toasterService: ToasterService,
    public override changeDetectorRef: ChangeDetectorRef
  ) {
    super(hostElement, renderer, toasterService, changeDetectorRef);
  }
}
