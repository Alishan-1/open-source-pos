import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import { RegistrationServic } from '../../../services/registration.services';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss']
})
export class LogoutComponent implements OnInit {

  constructor(private _regServic: RegistrationServic,
    private router: Router) { }

  ngOnInit(): void {
    this.logout();
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
