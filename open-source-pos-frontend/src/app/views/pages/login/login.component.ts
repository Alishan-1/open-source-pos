import { Component, OnInit, ViewChild} from '@angular/core';
// import { ModalDirective, } from "ngx-bootstrap";
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { RegistrationServic } from '../../../services/registration.services';
//import { NotificationService } from '../../shared/utils/notification.service';
import { RegularExpressions } from '../../../app.reg-validations';
import { GeoLocationService } from '../../../services/geo-location.service';
import { UserGeoLocation } from '../../../models/UserGeoLocation.model';
import { DeviceInfo, DeviceDetectorService } from 'ngx-device-detector';
//import { exDeviceInfo } from '../../../services/device-detector/exDeviceInfo.model'
import { User } from '../../../models/user.model'
import { exDeviceInfo } from '../../../models/exDeviceInfo.model'
import { UserService } from "../../../services/user.service";

import { ToasterComponent, ToasterPlacement } from '@coreui/angular';
import { AppPosToastComponent } from '../../notifications/pos-toasters/toast-simple/toast.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

    @ViewChild('ToasterViewChild') ToasterViewChild!:ToasterComponent;
  
    deviceInfo:DeviceInfo = {
      userAgent: "",
      os: "",
      browser: "",
      device: "",
      os_version: "",
      browser_version:"",
      deviceType:"",
      orientation:""
      
    };
    exDeviceInfo: exDeviceInfo = 
    {
      userAgent: "",
      os: "",
      browser: "",
      device: "",
      os_version: "",
      browser_version: "",
      deviceType:"",
      orientation:"",

      isMobile: false,
      isTablet: false,
      isDesktop: false
    };
    public valid = {
        form: false,
        UserEmail: false,
        UserPassword: false,
    };
    public posLoginFormValidated: boolean = false;
    public UserInfoModel = {
        "UserEmail": "",
        "UserPassword": "",
        "RememberUser": false,
    }
    private isFormSubmitted: boolean = false;
    private UserId: string = "";
    public isRequestProcessing: boolean;
    private msg: string = " ";
    //paragraph for showing some message to user
    private paragraph: string = "";
    private redrictURL: string = "";
    subValidation: any;
    ttErrors: any;
    LockoutEndTime: Date = new Date();
    public showLoader: boolean = true;
  
    constructor(private _registerservices: RegistrationServic, private router: Router,
        private activatedroute: ActivatedRoute,
        public _regExpressions: RegularExpressions, /*private _notificationService: NotificationService,*/
        private _route: ActivatedRoute, private _geoLocSrvc: GeoLocationService, 
        private _deviceService: DeviceDetectorService, private _userService: UserService) {
  
        this.valid.form = false;
        this.valid.UserEmail = false;
        this.valid.UserPassword = false;
        this.isRequestProcessing = false;
        this.isFormSubmitted = false;
        this.isRequestProcessing = false;
  
        //console.log("domain = " + this._registerservices.getDomain());
        this.LockoutEndTime = new Date();
  
        this.LockoutEndTime.setMinutes(this.LockoutEndTime.getMinutes() + 10);
        
    }
  
   
  
    ngOnInit() {
        debugger;
        this.showLoader = false;
        
        this._route.queryParams
            .subscribe(params => {
                this.msg = params['msg'];
                // first look for msg parameter an if it is not provided than look for redrictUrl parameter
                switch (this.msg) {
                    //check email and get password from it to login
                    case "chngpwd": {
  
                        this.paragraph = "Please login again to change your pasword";
                        this.redrictURL = "home/newPassword";
                        break;
                    }
                    // if msg parameter is not giver than look for redrictUrl parameter
                    default: {                        
                        this.redrictURL = params['redrictUrl'];
  
                        if (this.redrictURL && this.redrictURL.length >= 2) {
                            this.paragraph = "Please login again to continue your work!";
                        }
  
  
                        break;
                    }
                }
            });
        this.IsUserAlreadyLogedIn();
    }
  
    IsUserAlreadyLogedIn() {
        debugger;
        var user: User = JSON.parse(localStorage.getItem('currentUser') || "{}" );
        if (user) {
            if (user.RememberUser) {
  
                this._registerservices.IsUserLogedInAndRemembered(user).subscribe((serviceResponse: { Data: boolean; }) => {
                    //data is true if all conditions pass
                    debugger;
                    if (serviceResponse.Data === true) {
                        this.isRequestProcessing = false;
                        //redrict to location list
                        this.router.navigate([`dashboard`], { queryParams: { msg: 'alreadyLogedIn' } });
                    }
                    return;
  
                } , (error: any) => {
                    debugger;
                    this.isRequestProcessing = false;
                    console.log(error);
                });
            }
        }        
    }
  
    ForgerPassword() {
        debugger;
        this.isFormSubmitted = true;
        if (!this.valid.UserEmail) {
            let res = {
                Title: "Error",
                Message: "Please enter correct email address in Email field first"
            };            
            this.notificationError(res);
            return;
        }
        this.isRequestProcessing = true;
  
        this._registerservices.ForgerPassword(this.UserInfoModel).subscribe(
            
            {
                next:(serviceResponse: any) => {
            
                    this.notificationSuccessWithServuceResponse(serviceResponse);
                    this.isRequestProcessing = false;
                    this.router.navigate(['confirm'], { queryParams: { msg: 'forgtpwd' } });
                } ,
                error: error => {
                    this.isRequestProcessing = false;
                    this.notificationError(error);
                }
            }
        );
    }
  
    
    loginUser() {
        debugger;
        this.posLoginFormValidated = true;
        this.isFormSubmitted = true;
      //   if (!this.valid.form)
      //       return;
        this.isRequestProcessing = true;
        let userInfo = {
            "UserEmail": this.UserInfoModel.UserEmail,
            "UserPassword": this.UserInfoModel.UserPassword,
            "StartDate": new Date(),
            "UsersGeoLocation": {},
            "DeviceInfo": this.exDeviceInfo,
            "RememberUser": this.UserInfoModel.RememberUser,
            //session start date
            "SessionDate": new Date()
        }

        // get the information of device
        this.deviceInfo = this._deviceService.getDeviceInfo();
        this.exDeviceInfo.browser = this.deviceInfo.browser;
        this.exDeviceInfo.browser_version = this.deviceInfo.browser_version;
        this.exDeviceInfo.device = this.deviceInfo.device;
        this.exDeviceInfo.os = this.deviceInfo.os;
        this.exDeviceInfo.os_version = this.deviceInfo.os_version;
        this.exDeviceInfo.userAgent = this.deviceInfo.userAgent;
        this.exDeviceInfo.deviceType = this.deviceInfo.deviceType;
        this.exDeviceInfo.orientation = this.deviceInfo.orientation;
        
        this.exDeviceInfo.isMobile = this._deviceService.isMobile();
        this.exDeviceInfo.isTablet = this._deviceService.isTablet();
        this.exDeviceInfo.isDesktop = this._deviceService.isDesktop();
                        
        userInfo.DeviceInfo = this.exDeviceInfo;
        /***
         * 
         *  console.log(this.deviceInfo);
         *  console.log(isMobile);  // returns if the device is a mobile device (android / iPhone / windows-phone etc)
         *  console.log(isTablet);  // returns if the device us a tablet (iPad etc)
         *  console.log(isDesktopDevice); // returns if the app is running on a Desktop browser.
         ***/

        // get users geo location from ipapi
        this._geoLocSrvc.GetLocationFromIp().subscribe(loc => {
            debugger;
            userInfo.UsersGeoLocation = loc;            
            this.authenticate(userInfo);
  
        }, error => {
            // error on geolocation info.
            console.log(error);
            this.authenticate(userInfo);
        });
  
  
    }

    authenticate(userInfo: any){
        // clear previously saved data in shared serveice so that new data can be loaded
        this._userService.ClearStorage();

        // now login user 
        this._registerservices.loginUser(userInfo).subscribe((user: { Token: any; IsTemp: any; IsPasswordExpired: any; IsForgetPassword: any; }) => {
            debugger;

            if (user && user.Token) {
                
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('currentUser', JSON.stringify(user));
                //this.notificationSuccess(user);
                this.isRequestProcessing = false;
                this.showLoader = true;
                if (this.redrictURL) {
                    //basically msg does not have any meaning here yet
                    this.router.navigate([this.redrictURL], { queryParams: { msg: '0x25f3' } });
                    return;
                }
                else if (user.IsTemp) {
                    this.router.navigate(['confirm'], { queryParams: { msg: 'chngfrsttmpwd' } });
                }
                else if (user.IsPasswordExpired) { ///change password
                    this.router.navigate(['confirm'], { queryParams: { msg: 'chngExppwd' } });
                }
                else if (user.IsForgetPassword) {
                    this.router.navigate(['confirm'], { queryParams: { msg: 'chngforgettmpwd' } });
                }
                else {
                    this.router.navigate([`dashboard`]);
                }
            }
            return user;
        }, error => {
            debugger;
            this.isRequestProcessing = false;
            if (error && error.Data && error.Data.IsLockoutEnabled) {
                // show clock mobel here
                this.LockoutEndTime = new Date();

                this.LockoutEndTime.setMinutes(this.LockoutEndTime.getMinutes() + 10);
                this.showlockedModel();
                
            }
            else {
                this.notificationError(error);
            }
            
        });
    }

    public lockedModelVisible = false;

    togglelockedModel() {
      this.lockedModelVisible = !this.lockedModelVisible;
    }
    showlockedModel() {
        this.lockedModelVisible = true;
      }
    hidelockedModel() {
    this.lockedModelVisible = false;
    }
  
    notificationSuccessWithServuceResponse(sr: any) {
        /*this._notificationService.smallBox({
            title: sr.Title,
            content: sr.Message,
            color: "rgb(115, 158, 115)",
            icon: "fa fa-check shake animated",
            timeout: 6000
        });*/

        let props =
        {
          title: sr.Title,
          message: sr.Message,
          autohide: true,
          delay: 10000,
          position: 'top-end',
          fade: true,
          closeButton: true,
          color: 'success'
        };
        this.addToast(props);
    }
  
    notificationSuccess(msg: any) {
  
        /*this._notificationService.smallBox({
            title: "Success!",
            content: "Login Successfully",
            color: "rgb(115, 158, 115)",
            icon: "fa fa-check shake animated",
            timeout: 6000
        });*/
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
        
        /*this._notificationService.smallBox({
            title: msg.Title,
            content: msg.Message,
            color: "rgb(196, 106, 105)",
            icon: "fa fa-warning shake animated",
            timeout: 10000
        });*/
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
  
    ngDoCheck() {
  
        //Email validations
        let v1: boolean = (this.UserInfoModel.UserEmail.length >= 5);
        let v2: boolean = (this.UserInfoModel.UserEmail.length <= 44);
        let v3: boolean = this._regExpressions.isValidEmail(this.UserInfoModel.UserEmail);
        this.valid.UserEmail = (v1 && v2 && v3);
  
        //UserPassword Validations
        let v4: boolean = (this.UserInfoModel.UserPassword.length >= 7);
        let v5: boolean = (this.UserInfoModel.UserPassword.length <= 15);
        this.valid.UserPassword = (v4 && v5);
  
        this.valid.form = (this.valid.UserEmail && this.valid.UserPassword);
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
  
  