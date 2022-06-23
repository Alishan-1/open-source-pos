import { Injectable } from '@angular/core';
import {
    Component, OnInit, trigger,
    state,
    style,
    transition,
    animate, OnChanges, Input, DoCheck,
} from '@angular/core';
import {
    NG_VALIDATORS, Validator,
    Validators, AbstractControl, ValidatorFn
} from '@angular/forms';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import {
    VmCustomerOrderSummary, VmCustomerOrderDailyNew, VmPopularMenus,
    VmCustomerOrderDailySales, single, single1, multi, VmGraphData, VmMultiGraphData, RolePermission,
} from "app/+manager/+dashboard/+models/dashboard.model";
//import { CustomerOrder } from '../+models/customer-order.model';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';
import 'rxjs/add/operator/toPromise';
import { ServiceResponse } from "app/core/models/service-response.model";
import { ChowChoiceRequestOptions } from '../../app.request-options';
import { UserService, User } from "app/shared/user";
import { Configuration } from "app/app.constants";
import { UtilService } from 'app/services/util.service';
import { NotificationService } from "app/shared/utils/notification.service";

//2. The service class
@Injectable()
export class AuthorizationService {
   private AuthLocation: string;
    locationId: number;
    private errorMsg: string = "Error message!!";
    private AuthapiUrl: string;
    public RolePermissions: RolePermission[];

    private ControlPermission = {};
    //private LogicalScreenName: string = "Dashboard";
    AuthorizedUser: any;
    private SystemControles: any;
    currentUser: User;
    //used for All Screens Except Location List
    private featureRuleType: string = "";
    private dbMentionsControl: string[] = [];

//Only Used For LocationList
    private locWisefeatureRuleType = {};
    private locWiseDbMentionControl = {};


    constructor(private _http: Http, private _configuration: Configuration, private _ccro: ChowChoiceRequestOptions, private _userService: UserService, private _utilService: UtilService, private _notificationService: NotificationService ) {

        this.AuthapiUrl = `${_configuration.WebApi}api/AuthorizedUser/get-all`;
        this.AuthLocation = `${_configuration.WebApi}api/AuthorizedUser/get`;

    }

    //Call this mathod first on init of every screen

    loadRoleAuthorizationVersion2(LocationID: number, UserId: number, Screen: string) {
      
        this.getAuthorized(LocationID, UserId, Screen).subscribe(
            sr => {
                debugger;
                this.RolePermissions = sr.Data;
                this.featureRuleType = "";
                this.dbMentionsControl = [];
                for (let element of this.RolePermissions) {
                    if (element.Type == 0 && element.Screen == Screen) {
                        this.featureRuleType = element.Feature;
                        //console.log(element);
                    }
                    else if (element.Type == 1 && element.Screen == Screen) {
                        this.dbMentionsControl.push(element.Feature);
                    }
                }
            }, error => { this.handleError(error) }
        )
    }

    /**
     * async wala version as on 21-Dec-2018
     * @param LocationID
     * @param UserId
     * @param Screen
     */
    async loadRoleAuthorizationVersion3(LocationID: number, UserId: number, Screen: string) {

        let sr = await this.getAuthorizedAsync(LocationID, UserId, Screen);
        debugger;
        this.RolePermissions = sr.Data;
        this.featureRuleType = "";
        this.dbMentionsControl = [];
        for (let element of this.RolePermissions) {
            if (element.Type == 0 && element.Screen == Screen) {
                this.featureRuleType = element.Feature;
                //console.log(element);
            }
            else if (element.Type == 1 && element.Screen == Screen) {
                this.dbMentionsControl.push(element.Feature);
            }
        }
        return true;
    }



    //used for All Screens Except Location List



    isShowControls(ControlName: string) {
       
        if (this.featureRuleType == 'All') {
            let flag = 0;
            for (let control of this.dbMentionsControl) {
                if (control === ControlName) {
                    debugger; 
                    flag = 1;
                }
                else {
                    //flag = 0;
                }
            }
            if (flag == 0) {
                return true;
            }
            else if (flag == 1) {
                return false;
            } 
        }
        else if (this.featureRuleType == 'View') {
            let flag = 0;
            for (let control of this.dbMentionsControl) {
                if (control == ControlName) {
                    flag = 1;
                }
            }
            if (flag == 1) {
                return true;
            }
            else if (flag == 0) {
                return false;
            }
        }

    }

    //Only Used For LocationList


    loadRoleAuthorizationLocationList(UserId: number, Screen: string) {
        debugger;
        
        this.getAuthorizedLocation(UserId, Screen).subscribe(
            sr => {
                debugger;
                var data: Array<RolePermission> = sr.Data;
               
                this.locWisefeatureRuleType = {};
                this.locWiseDbMentionControl = {};
                for (let element of data) {

                    if (element.Type == 0 && element.Screen == Screen) {
                        this.locWisefeatureRuleType[element.LocationID.toString()] = element.Feature;
                        //console.log(element);
                    }
                    else if (element.Type == 1 && element.Screen == Screen) {
                        if (this.locWiseDbMentionControl[element.LocationID.toString()] == undefined) {
                            this.locWiseDbMentionControl[element.LocationID.toString()] = new Array<string>();
                            this.locWiseDbMentionControl[element.LocationID.toString()].push(element.Feature);
                        }
                        else {
                            this.locWiseDbMentionControl[element.LocationID.toString()].push(element.Feature);
                        }
                    }
                }
        }, error => { this.handleError(error) })
    }


    //Only Used For LocationList
    //isShowControlsLoc(ControlName: string, LocationID: number) {
    //    debugger;
    //    if (this.locWisefeatureRuleType[LocationID] == 'All') {
    //        let flag = 0;
    //        for (let control of this.locWiseDbMentionControl[LocationID]) {
    //            if (control === ControlName){
    //                flag = 1;
    //            }
    //            else {
    //                //flag = 0;
    //            }
    //        }
    //        if (flag == 0) {
    //            return true;
    //        }
    //        else if (flag == 1) {
    //            return false;
    //        } 
    //    }
    //    else if (this.locWisefeatureRuleType[LocationID] == 'View') {
    //        for (let control of this.locWiseDbMentionControl[LocationID]) {
    //            if (control === ControlName) {
    //                return true;
    //            }
               
    //        }
    //        return false;
    //    }
    //    return false;
    //}
    
    isShowControlsLoc(ControlName: string, LocationID: number)  {
      
        if (this.locWisefeatureRuleType[LocationID.toString()] == 'All') {
            let flag = 0;

            if (this.locWiseDbMentionControl[LocationID.toString()]) {
                for (let control of this.locWiseDbMentionControl[LocationID.toString()]) {
                    if (control === ControlName) {
                        debugger;
                        flag = 1;
                        //return false;
                    }
                    else {
                        //flag = 0;
                    }
                }
            }
            if (flag == 0) {
                return true;
            }
            else if (flag == 1) {
                return false;
            } 
        }
        else if (this.locWisefeatureRuleType[LocationID.toString()] == 'View') {
            for (let control of this.locWiseDbMentionControl[LocationID.toString()]) {
                if (control === ControlName) {
                    return true;
                }
            }
            return false;
        }
    }
    //loadRoleAuthorization.ControlPermission.G

    getAuthorizedLocation(UserId: number, Screen: string): Observable<any> {
        let params: any = {};
        params.UserId = UserId; 
        params.Screen = Screen;
         
        return this
            ._http.post(this.AuthLocation, JSON.stringify(params), this._ccro.GetChowChoiceRequestOptions())
            .map(this.extractData)
            .catch(this._utilService.handleError);
    }

    getAuthorized(LocationID: number, UserId: number, Screen: string): Observable<any> {
        debugger;
        let params: any = {};
        params.UserId = UserId;
        params.LocationID = LocationID;
        params.Screen = Screen;
        return this
            ._http.post(this.AuthapiUrl, JSON.stringify(params), this._ccro.GetChowChoiceRequestOptions())
            .map(this.extractData)
            .catch(this._utilService.handleError);
    }

    async getAuthorizedAsync(LocationID: number, UserId: number, Screen: string) {
        debugger;
        let params: any = {};
        params.UserId = UserId;
        params.LocationID = LocationID;
        params.Screen = Screen;
        const response = await this
            ._http.post(this.AuthapiUrl, JSON.stringify(params), this._ccro.GetChowChoiceRequestOptions()).toPromise();
        return response.json();
    }
    private extractData(res: Response) {
        debugger;
        let body = res.json();
        return body || {};
    }

    private handleError(error: any) {
        this.notificationError(error);
    }

    notificationSuccess(msg: any) {

        this._notificationService.smallBox({
            title: msg.Title,
            content: msg.Message,
            color: "rgb(115, 158, 115)",
            icon: "fa fa-check shake animated",
            timeout: 6000
        });
    }

    notificationError(msg: any) {

        this._notificationService.smallBox({
            title: msg.Title,
            content: msg.Message,
            color: "rgb(196, 106, 105)",
            icon: "fa fa-warning shake animated",
            timeout: 10000
        });
    }
}
