import { Component, OnInit } from '@angular/core';

import { RegistrationServic } from '../../../services/registration.services';
import { Router, ActivatedRoute  } from '@angular/router';

@Component({
  selector: 'app-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.scss']
})
export class ConfirmationComponent implements OnInit {
  UserPackageScreen: any;
  private msg: string = "";
  public heading: string = "";
  public paragraph: string = "";
  private redrictURL: string = "";

  constructor(private _registerservices: RegistrationServic, private router: Router, private _route: ActivatedRoute) { }

  ngOnInit() {
      
      this._route.queryParams            
          .subscribe(params => {
              this.msg = params['msg'];
              switch (this.msg) {
                  //check email and get password from it to login
                  case "chkemlandgetpwd": {
                      this.heading = "Confirm Email";
                      this.paragraph = "Please check your email, a password has been sent to your email, use that password to login";
                      this.redrictURL = "login";
                      break;
                  }
                  case "chngfrsttmpwd": {
                      this.heading = "Change Password";
                      this.paragraph = "You are logging in for the first time, please click on ok button to change your temporary password.";
                      this.redrictURL = "newPassword";
                      break;
                  } 
                  case "chngExppwd": {
                      this.heading = "Change Password";
                      this.paragraph = "A long time ago you changed your password. Its time to change it again!";
                      this.redrictURL = "newPassword";
                      break;
                  } 
                  case "forgtpwd": {
                      this.heading = "Forget Password";
                      this.paragraph = "Please check your email, password was sent to your email If you entered correct email, use that password to login";
                      this.redrictURL = "login";
                      break;
                  }
                  case "chngforgettmpwd": {
                      this.heading = "Forget Password";
                      this.paragraph = "You are logging in With the temperary password, please click on ok button to change your temporary password.";
                      this.redrictURL = "newPassword";
                      break;
                  }
                  default: {
                      
                      break;
                  }
              } 
          });
  }
  OkClick() {
      this.router.navigate([this.redrictURL], { queryParams: { from: 'cnfrmscrn' }, queryParamsHandling: 'merge' });
  }

  
  handleError(error: any): any {
     debugger;
     //throw new Error("Method not implemented.");
  }
}
