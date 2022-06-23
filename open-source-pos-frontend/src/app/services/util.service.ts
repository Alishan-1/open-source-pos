import { Injectable } from '@angular/core';
import { ServiceResponse } from '../models/service-response.model';
import { Observable } from 'rxjs';
import { throwError } from 'rxjs';
// import { ErrorObservable } from 'rxjs/observable/ErrorObservable';
// import { NotificationService } from "app/shared/utils/notification.service";

import { Router } from "@angular/router";

// import { RegistrationServic } from "app/home/services/registration.services"

@Injectable()
export class UtilService {

    constructor() { }
    /**
     * Generic error message
     * **/
    private errorMsg: string = "Generic error, Cant determin the error type error message!!";
    /**
     * https://jsfiddle.net/311aLtkz/
     * https://en.wikipedia.org/wiki/Duck_typing
     * https://stackoverflow.com/questions/9847580/how-to-detect-safari-chrome-ie-firefox-and-opera-browser/9851769
     * 
     * */
    GetBrowser() {
        //// Opera 8.0+
        //var isOpera = (!!window.opr && !!opr.addons) || !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;

        //// Firefox 1.0+
        //var isFirefox = typeof InstallTrigger !== 'undefined';

        //// Safari 3.0+ "[object HTMLElementConstructor]" 
        //var isSafari = /constructor/i.test(window.HTMLElement) || (function (p) { return p.toString() === "[object SafariRemoteNotification]"; })(!window['safari'] || safari.pushNotification);

        //// Internet Explorer 6-11
        //var isIE = /*@cc_on!@*/false || !!document.documentMode;

        //// Edge 20+
        //var isEdge = !isIE && !!window.StyleMedia;

        //// Chrome 1+
        //var isChrome = !!window.chrome && !!window.chrome.webstore;

        //// Blink engine detection
        //var isBlink = (isChrome || isOpera) && !!window.CSS;

        //var output = 'Detecting browsers by ducktyping:<hr>';
        //output += 'isFirefox: ' + isFirefox + '<br>';
        //output += 'isChrome: ' + isChrome + '<br>';
        //output += 'isSafari: ' + isSafari + '<br>';
        //output += 'isOpera: ' + isOpera + '<br>';
        //output += 'isIE: ' + isIE + '<br>';
        //output += 'isEdge: ' + isEdge + '<br>';
        //output += 'isBlink: ' + isBlink + '<br>';
        //document.body.innerHTML = output;
    }

    handleErrorInternal(error: any): ServiceResponse {        
        debugger;
        let serResponse: ServiceResponse = new ServiceResponse();
        // check whather the connection to the server was successful or not?
        if (error.ok != undefined) {
            if (!error.ok && error.status == 0 && error.statusText == "" && error.type == 3) {
                // control comes here this means that connection to the server was UN-successful
                serResponse.Title = "Can't connect to server";
                serResponse.Message = "connection to the server was unsuccessful, please check your internet connection";
                serResponse.Flag = false;
                serResponse.IsValid = false;
                return serResponse;
            }

        }
        serResponse = JSON.parse(error._body);
        let errMsg = (error.message) ? error.message :
            error.status ? `${error.status} - ${error.statusText}` : this.errorMsg;
        console.error(error, errMsg);
        if (serResponse) {
            return serResponse;
        }
        // control comes here this means that Service response object is not sent by the server.
        serResponse = new ServiceResponse();
        serResponse.Title = "Something Went Wrong";
        let msg: string = error._body;
        if (msg.length <= 0) {
            msg = "You have experienced a technical error. We apologize. We are working hard to correct this issue.Please wait a few moments and try again or contact support at admin@chowchoice.com";
        }
        serResponse.Message = msg;
        serResponse.Flag = false;
        serResponse.IsValid = false;
        return serResponse;
    }

    /**
     * This method is used to handle errors with no gui notification
     * @param error
     */
    public handleError(error: any): /*ErrorObservable*/ any {
        debugger;
        let serResponse: ServiceResponse = new ServiceResponse();
        if  (error && error.error &&  error.error.message){
            // control comes here this means that server returned a message
            serResponse.Title = "Your request was not successful";
            serResponse.Message = error.error.message;
            serResponse.Flag = false;
            serResponse.IsValid = false;
            if(error.error.data)
                serResponse.Data = error.error.data;
            return throwError( serResponse );
        }
        if  (error && error.error &&  error.error.Message){
            // control comes here this means that server returned a Message
            serResponse = error.error;
            serResponse.Title = "Your request was not successful";
            if(error.error.Title) serResponse.Title =  error.error.Title;
            serResponse.Message = error.error.Message;
            
            if(error.error.data)
                serResponse.Data = error.error.data;
            return throwError( serResponse );
        }
        // check whather the connection to the server was successful or not?
        if (error.ok != undefined) {
            if (!error.ok && error.status == 0 && error.statusText == "" && error.type == 3) {
                // control comes here this means that connection to the server was UN-successful
                serResponse.Title = "Can't connect to server";
                serResponse.Message = "connection to the server was unsuccessful, please check your internet connection";
                serResponse.Flag = false;
                serResponse.IsValid = false;
                return throwError( serResponse );
            }

        }
        // check whather the user was authorised or not
        if (error.status === 401) {
            // control comes here if the jwt token is not present or the token is not valid
            serResponse.Title = "Unauthorized";
            serResponse.Message = "You are not loged in or your session has expired. Please login again to continue";
            serResponse.Flag = false;
            serResponse.IsValid = false;
            serResponse.StatusCode = error.status
            //this._regServic.logout().subscribe(serviceResponse => {
                //this.router.navigate(['home/login']);
                //return;
            //}, error => {
                //this.router.navigate(['home/login']);
            //});

            return throwError(serResponse);
        }
        // try to read service response from body
        try {
            serResponse = new ServiceResponse();
            serResponse = JSON.parse(error._body);
            return throwError(serResponse);
        } catch (e) {
            serResponse = new ServiceResponse();
        }
        serResponse.StatusCode = error.status
        let errMsg = (error.message) ? error.message :
            error.status ? `${error.status} - ${error.statusText}` : this.errorMsg;
        console.error(error, errMsg);

        
        
        // control comes here this means that Service response object is not sent by the server.
        if (!serResponse) {
            serResponse = new ServiceResponse();
        }

        
        serResponse.Title = "Something Went Wrong";
        let msg: string = error._body;
        if (!msg || !msg.length || msg.length <= 0) {
            msg = "You have experienced a technical error. We apologize. We are working hard to correct this issue.Please wait a few moments and try again or contact support";
        }
        serResponse.Message = msg;
        serResponse.Flag = false;
        serResponse.IsValid = false;

            
        return throwError(serResponse);
    }

    /**
     * This method is used to handle errors with gui notification
     * @param error
     */
    public handleErrorWithNotification(error: any): any {
        debugger;       

        let serResponse: ServiceResponse = new ServiceResponse();

        // check whather the connection to the server was successful or not?
        if (error.ok != undefined) {
            if (!error.ok && error.status == 0 && error.statusText == "" && error.type == 3) {
                // control comes here this means that connection to the server was UN-successful
                serResponse.Title = "Can't connect to server";
                serResponse.Message = "connection to the server was unsuccessful, please check your internet connection";
                serResponse.Flag = false;
                serResponse.IsValid = false;
                this.notificationError(serResponse);
                return throwError(serResponse);
            }

        }
        serResponse = JSON.parse(error._body);
        let errMsg = (error.message) ? error.message :
            error.status ? `${error.status} - ${error.statusText}` : this.errorMsg;
        console.error(error, errMsg);
        if (serResponse) {
            this.notificationError(serResponse);
            return throwError(serResponse);
        }
        // control comes here this means that Service response object is not sent by the server.
        serResponse = new ServiceResponse();
        serResponse.Title = "Something Went Wrong";
        let msg: string = error._body;
        if (msg.length <= 0) {
            msg = "You have experienced a technical error. We apologize. We are working hard to correct this issue.Please wait a few moments and try again or contact support at admin@chowchoice.com";
        }
        serResponse.Message = msg;
        serResponse.Flag = false;
        serResponse.IsValid = false;

        this.notificationError(serResponse);

        return throwError(serResponse);
    }

    /**
     * Used to show toaster with error
     * @param msg
     */
    public notificationError(msg: ServiceResponse) {

        // this._notificationService.smallBox({
        //     title: msg.Title,
        //     content: msg.Message,
        //     color: "rgb(196, 106, 105)",
        //     icon: "fa fa-warning shake animated",
        //     timeout: 10000
        // });
    }
}
