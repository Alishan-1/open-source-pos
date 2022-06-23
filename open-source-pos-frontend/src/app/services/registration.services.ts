//1. Import all dependencies
import { Injectable } from '@angular/core';
// import {
//     Component, OnInit, trigger,
//     state,
//     style,
//     transition,
//     animate, OnChanges, Input, DoCheck,
// } from '@angular/core';
import {
    NG_VALIDATORS, Validator,
    Validators, AbstractControl, ValidatorFn
} from '@angular/forms';
import { HttpClient as Http, HttpResponse as Response,   
    HttpHeaders as Headers } from '@angular/common/http';

import { Observable, throwError  } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { ServiceResponse } from '../models/service-response.model';
import { Configuration } from '../app.constants';

import { User } from '../models/user.model';
import { UtilService } from './util.service';
import { UserService } from "./user.service";
import { AuthService } from '../views/login/auth.service';

//2. The service class
@Injectable({
    providedIn: 'root',
  })
export class RegistrationServic {

    // 4. Passsing the Http dependency to the constructor to access Http functions
    constructor(private _http: Http, private _configuration: Configuration, 
        private _utilService: UtilService, private _userService: UserService, 
        private _authService: AuthService ) {
        this.loginUserUrl = `${_configuration.WebApi}/User/authenticate/`;
        this.registerUserUrl = `${_configuration.WebApi}/User/register/`;
        this.testapiUrl = `${_configuration.WebApi}/User/get/`;

    }

    IsUserLogedInAndRemembered(user: User): any {
        // return this
        //     ._http.post(`${this._configuration.WebApi}api/User/IsUserLogedInAndRemembered/`, JSON.stringify(user), this._ccro.GetChowChoiceRequestOptions())
        //     .map(this.extractData)
        //     .catch(err => { return err; });

        return this._http.post(`${this._configuration.WebApi}/User/IsUserLogedInAndRemembered/`, user, { headers: this._authService.GetHttpHeaders() })
        .pipe(
            catchError(this._utilService.handleError)
        )
    }

    // /**
    //  * 
    //  * used to logout user and end his session if found
    //  * */
    logout(): any {       
        
        debugger;
        var user: User = JSON.parse(localStorage.getItem('currentUser') || '{}');
        this._userService.ClearStorage();
        // if user found in local storage
        if (user) {
            // used as session end date
            user.SessionDate = new Date();            
            
            let header = this._authService.GetHttpHeaders();
            localStorage.removeItem('currentUser');
            return this._http.post(`${this._configuration.WebApi}/User/LogOut/`, user, { headers:  header})
            .pipe(
                catchError(this._utilService.handleError)
            ); 
        }
        else {
            // user not found in local storage
            return new Observable();
        }
    }
    
    private registerUserUrl: string;
    private loginUserUrl: string;
    private testapiUrl: string;
    // //private getAllCustomerOrderSummaryUrl: string;
    // //private updateOrderStatusUrl: string;
    // //private updateDeliveryStatusUrl: string;
    // //private getAllDeliveryStatusUrl: string;
    // private errorMsg: string = "Error message!!";
    
   

    // getDomain() {
    //     return this._configuration.domain;
    // }
    
    // /**
    // * get validation messages for login screen
    // * author: Ali Shan
    // * @returns   Returns array of validation messages    
    // */
    // getLoginValidations(): Observable<ServiceResponse> {
    //     return this
    //         ._http.get(`${this._configuration.WebApi}api/User/getLoginValidations/`, this._ccro.GetChowChoiceRequestOptions())
    //         .map(this.extractData)
    //         .catch(this._utilService.handleError);
    // }
    // /**
    // * get validation messages for register screen
    // * author: Ali Shan
    // * @returns   Returns array of validation messages    
    // */
    // getRegisterValidations(): any {
    //     return this
    //         ._http.get(`${this._configuration.WebApi}api/User/getRegisterValidations/`, this._ccro.GetChowChoiceRequestOptions())
    //         .map(this.extractData)
    //         .catch(this._utilService.handleError);
    // }

    registerUser(model: any): Observable<any> {
        debugger;
        // let header = new Headers({ 'Content-Type': 'application/json' });
        // let options = new RequestOptions({ headers: header });
        // return this
        //     ._http
        //     .post(this.registerUserUrl, JSON.stringify(model), options)
        //     .map(this.extractData)
        //     .catch(this._utilService.handleError);

        return this._http.post(this.registerUserUrl, model, { headers: this._authService.GetHttpHeaders() })
        .pipe(
            catchError(this._utilService.handleError)
        )
    }

    loginUser(model: any): Observable<any> {
        debugger;
        /*let header = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: header });
        return this
            ._http
            .post(this.loginUserUrl, JSON.stringify(model), options)
            .map(this.extractData)
            .catch(this._utilService.handleError); */ 
            
            return this._http.post(this.loginUserUrl, model, { headers: this._authService.GetHttpHeaders() })
            .pipe(
              catchError(this._utilService.handleError)
            )
    
        }

  

    ChangePassword(user: any): Observable<any> {
        
        let params = user;
        let ChangePasswordApiUrl = `${this._configuration.WebApi}/User/change-password/`;

        // return this
        //     ._http.post(ChangePasswordApiUrl, JSON.stringify(params), this._ccro.GetChowChoiceRequestOptions())
        //     .map(this.extractData)
        //     .catch(this._utilService.handleError);

        return this._http.post(ChangePasswordApiUrl, params, { headers: this._authService.GetHttpHeaders() })
        .pipe(
            catchError(this._utilService.handleError)
        )
    }

    // testApi(): Observable<any> {
    //    debugger;
    //    let params: any = {test: "A"};
      
       
    //    return this
    //        ._http.post(this.testapiUrl, JSON.stringify(params), this._ccro.GetChowChoiceRequestOptions())
    //        .map(this.extractData)
    //        .catch(this._utilService.handleError);
    // }
    ForgerPassword(userInfo: any): Observable<any> {

        let params = userInfo;
        let forgerPasswordApiUrl = `${this._configuration.WebApi}/User/ForgetPassword/`;

        // return this
        //     ._http.post(forgerPasswordApiUrl, JSON.stringify(params), this._ccro.GetChowChoiceRequestOptions())
        //     .map(this.extractData)
        //     .catch(this._utilService.handleError);
        return this._http.post(forgerPasswordApiUrl, params, { headers: this._authService.GetHttpHeaders() })
        .pipe(
            catchError(this._utilService.handleError)
        )
    }

    // //checkandVerifyUser(userEmail: string): Observable<any> {

    // //    let header = new Headers({ 'Content-Type': 'application/json' });
    // //    let options = new RequestOptions({ headers: header });
    // //    return this
    // //        ._http
    // //        .get(this.verifyEmail + "?userId=" + userEmail)
    // //        .map(this.extractData)
    // //        .catch(this.handleError);
    // //}

    // private extractData(res: Response) {
    //     debugger;
    //     let body = res.json();
    //     return body || {};
    // }

    // private handleError(error: any) {
    //     debugger;
    //     let serResponse: any = JSON.parse(error._body);
    //     let errMsg = (error.message) ? error.message :
    //         error.status ? `${error.status} - ${error.statusText}` : this.errorMsg;
    //     console.error(error, errMsg);
    //     return Observable.throw(serResponse);
    // }
}