import { Component, OnInit, ViewChild } from '@angular/core';


import { ToasterComponent, ToasterPlacement } from '@coreui/angular';
import { AppPosToastComponent } from './toast-simple/toast.component';



@Component({
  selector: 'app-pos-toasters',
  templateUrl: './toasters.component.html',
  styleUrls: ['./toasters.component.scss']
})
export class PosToastersComponent implements OnInit {  

  @ViewChild('ToasterViewChild') ToasterViewChild!:ToasterComponent;

  ngOnInit(): void {
   
  }


  addvcToast() {
    let props =
      {
        title: `Toast`,
        message: 'test toast',
        autohide: true,
        delay: 10000,
        position: ToasterPlacement.TopEnd,
        fade: true,
        closeButton: true,
        color: 'danger'
      };
      debugger;
      const componentRef = this.ToasterViewChild.addToast(AppPosToastComponent, props, {});
      componentRef.instance['closeButton'] = true;
    
  }
}
