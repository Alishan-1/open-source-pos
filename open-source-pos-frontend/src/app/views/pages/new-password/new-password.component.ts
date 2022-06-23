import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { RegistrationServic } from '../../../services/registration.services';
import { RegularExpressions } from '../../../app.reg-validations';

import { ViewChild} from '@angular/core';
import { ToasterComponent, ToasterPlacement } from '@coreui/angular';
import { AppPosToastComponent } from '../../notifications/pos-toasters/toast-simple/toast.component';



@Component({
  selector: 'app-new-password',
  templateUrl: './new-password.component.html',
  styleUrls: ['./new-password.component.scss']
})
export class NewPasswordComponent implements OnInit {
    @ViewChild('ToasterViewChild') ToasterViewChild!:ToasterComponent;
  public posNewPasswordFormValidated: boolean = false;
  public valid = {
    form: false,    
    UserPassword: false,
    ReTypePassword: false,
    CurrentPassword: false
}

public PasswordInfoModel = {
    "CurrentPassword": "",
    "UserPassword": "",
    "ReTypePassword": "",
    "UserID": 0,
    "UserEmail": "",
    "IsForgetPassword": false      
}

public isRequestProcessing: boolean;
public StrengthMessage: string = "";
public ttErrors: any = {};
public msg: string = "";
public meter_value: number = 0;
constructor(private _registerservices: RegistrationServic, private _router: Router, public _regExpressions: RegularExpressions, private _route: ActivatedRoute) {

    this.isRequestProcessing = false;
}

ngOnInit() {
    this.valid.form = false;
    this._route.queryParams
        .subscribe(params => {
            this.msg = params['msg'];
            switch (this.msg) {
                //check email and get password from it to login
                
                case "chngforgettmpwd": {
                    // "Forget Password";
                    // "You are logging in With the temperary password, please click on ok button to change your temporary password.";
                    this.PasswordInfoModel.IsForgetPassword = false;
                    break;
                }
                default: {

                    break;
                }
            }
        });
}

  Submit() {
    debugger;
    if(!this.valid.form){
        this.posNewPasswordFormValidated = true;
        return;
    }

      
      this.isRequestProcessing = true;
      let currentUser = JSON.parse(localStorage.getItem('currentUser') || '');

      if (currentUser == null) {
          this._router.navigate(['login']);
          return;
      }

      this.PasswordInfoModel.UserEmail = currentUser.UserEmail;
      this.PasswordInfoModel.UserID = currentUser.UserID;
      
      this._registerservices.ChangePassword(this.PasswordInfoModel).subscribe(
        (          srSuccess: any) => {
              this.isRequestProcessing = false;
              this.notificationSuccess(srSuccess);
              this._router.navigate(['/manager/location/list', currentUser.UserID]);
          },
        (          srError: { Errors: any; }) => {
              this.isRequestProcessing = false;
              this.ttErrors = Object.assign({}, this.ttErrors, srError.Errors);
              this.notificationError(srError);

          }
      )
  }
handleError(error: any): any {
    debugger;
    //throw new Error("Method not implemented.");
}


notificationSuccess(msg: any) {

    // this._notificationService.smallBox({
    //     title: msg.Title,
    //     content: msg.Message,
    //     color: "rgb(115, 158, 115)",
    //     icon: "fa fa-check shake animated",
    //     timeout: 6000
    // });

    let props =
        {
          title: "Success!",
          message: "Login Successfully",
          autohide: true,
          delay: 10000,
          position: 'top-end',
          fade: true,
          closeButton: true,
          color: 'success'
        };
        this.addToast(props);
}

notificationError(msg: any) {

    // this._notificationService.smallBox({
    //     title: msg.Title,
    //     content: msg.Message,
    //     color: "rgb(196, 106, 105)",
    //     icon: "fa fa-warning shake animated",
    //     timeout: 10000
    // });
    let props =
        {
          title: msg.Title,
          message: msg.Message,
          autohide: true,
          delay: 10000,
          position: 'top-end',
          fade: true,
          closeButton: true,
          color: 'danger'
        };
        this.addToast(props);
}
    addToast(props:any) {
            
        if(!props){
            props =
            {
            title: `Toast`,
            message: 'test toast',
            autohide: true,
            delay: 10000,
            position: 'top-end',
            fade: true,
            closeButton: true,
            color: 'danger'
            };
        }
        
        
        const componentRef = this.ToasterViewChild.addToast(AppPosToastComponent, props, {});
        componentRef.instance['closeButton'] = true;
    }
    ngDoCheck() {
        // debugger;
        var strength = this.checkStrength(this.PasswordInfoModel.UserPassword);
        // validate UserPassword
        this.valid.UserPassword = strength >= 4;
        // validate ReTypePassword
        this.valid.ReTypePassword = this.PasswordInfoModel.UserPassword === this.PasswordInfoModel.ReTypePassword
        // validate CurrentPassword
        this.valid.CurrentPassword = this.PasswordInfoModel.CurrentPassword.length >= 7;

        // Form is valid if all fields are valid
        this.valid.form = this.valid.UserPassword && this.valid.ReTypePassword && this.valid.CurrentPassword;
        
        var val = this.PasswordInfoModel.UserPassword;
        

        // Update the password strength meter
        this.meter_value = strength;

        // Update the text indicator
        if (strength > 4) {
            // Max meter value is 4
            this.meter_value = 4;
            this.StrengthMessage = "Strength: Strong";
        }
        else if(val !== "") {
            this.StrengthMessage = "Strength: " + this.strengthMsgs[strength];
        } else {
            this.StrengthMessage = "";
        }
        
    }

  private strengthMsgs = [
       "Worst",
       "Bad",
       "Weak",
       "Good",
       "Strong"
      ];

  checkStrength(password: string) {
      var strength = 0
      if (password.length < 6) {            
          return strength;
      }
      if (password.length > 7) strength += 1
      // If password contains both lower and uppercase characters, increase strength value.
      if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) strength += 1
      // If it has numbers and characters, increase strength value.
      if (password.match(/([a-zA-Z])/) && password.match(/([0-9])/)) strength += 1
      // If it has one special character, increase strength value.
      if (password.match(/([!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1
      // If it has two special characters, increase strength value.
      if (password.match(/(.*[!,%,&,@,#,$,^,*,?,_,~].*[!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1
      
      return strength;
      
  }
}
