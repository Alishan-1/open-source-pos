import { Component, Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

import { ClassToggleService, HeaderComponent } from '@coreui/angular';

import { RegistrationServic } from "../../../services/registration.services";
import {Router, NavigationStart,NavigationEnd} from "@angular/router";

@Component({
  selector: 'app-default-header',
  templateUrl: './default-header.component.html',
})
export class DefaultHeaderComponent extends HeaderComponent {

  @Input() sidebarId: string = "sidebar";

  public newMessages = new Array(4)
  public newTasks = new Array(5)
  public newNotifications = new Array(5)

  constructor(private classToggler: ClassToggleService, private _regServic: RegistrationServic,
    private router: Router) {
    super();
  }

  logout() {
    this._regServic.logout().subscribe({ 
      next:() => {
        this.router.navigate(['login']);
        return;
    }, 
    error: () => {
        this.router.navigate(['login']);
    }});
  }
}
