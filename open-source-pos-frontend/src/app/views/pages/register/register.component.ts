import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { RegistrationServic } from '../../../services/registration.services';

import { ToasterComponent, ToasterPlacement } from '@coreui/angular';
import { AppPosToastComponent } from '../../notifications/pos-toasters/toast-simple/toast.component';
import { ViewChild} from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {

    @ViewChild('ToasterViewChild') ToasterViewChild!:ToasterComponent;

  public UserInfo = {
      "RoleId": 0,
      "UserID": 0,
      "AppID": 1, //The open source pos Application has app id of 1
      "AppRoleID": 2, //When the user registers from open source pos it is assigned the id of 2 = ADMIN
      "UserEmail": "",
      "FirstName": "",
      "MiddleName": "",
      "LastName": "",
      "PhoneNumber": ""
  }
  
  constructor(private _registerservices: RegistrationServic, private router: Router) { }
  public isRequestProcessing: boolean = false;
  public isFormSubmitted: boolean = false;
  posRegisterFormValidated = false;
  registerUser() {
      debugger;
      this.isFormSubmitted = true;
      this.posRegisterFormValidated = true;
      // if (!this.valid.form)
      //     return;
      this.isRequestProcessing = true;
      debugger;
      //var ff = this.UserInfo;  
      this._registerservices.registerUser(this.UserInfo).subscribe((res: any) => {
          this.notificationSuccess(res);
          this.isRequestProcessing = false;
          this.router.navigate(['confirm'], { queryParams: { msg: 'chkemlandgetpwd' } });
          //this.router.navigate(['home/login']);
      }, (error: any) => {
          this.isRequestProcessing = false;
          this.notificationError(error);
          

      });
  }
  notificationSuccess(msg: any) {

      // this._notificationService.smallBox({
      //     title: "Success",
      //     content: "User Has been registered Successfully",
      //     color: "rgb(115, 158, 115)",
      //     icon: "fa fa-check shake animated",
      //     timeout: 6000
      // });

      let props =
        {
          title: "Success",
          message: "User Has been registered Successfully",
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
      //     title: "Error!",
      //     content: msg.message,
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

}
