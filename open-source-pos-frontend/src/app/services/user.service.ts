import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Observable, Subject, BehaviorSubject } from "rxjs";
// import { JsonApiService } from "./json-api.service";
import { User } from "../models/user.model";





@Injectable({
    providedIn: 'root',
  })
export class UserService {
    //  Start - New Change - Fazi
    private user: Subject<any>;
    //  End - New Change - Fazi
    //public user: Subject<any>;
    //public locations: BehaviorSubject<any>;
    //public currentLocation: BehaviorSubject<any>;
    private currentEmployeeTimeLog: BehaviorSubject<any>;
    //public managerUser: BehaviorSubject<User>;
    //public selOrderStatus: BehaviorSubject<any>;
    

    public userInfo = {
        username: 'Guest'
    };
    //  Start - New Change - Fazi

    //public selectedPackageItem: any = {};
    private selectedPackageItem: any = {};
    public errorForErrorPage: any = {};        
    //public userLocations: [];    
    //public userCurrentLocation: any = {};
    //public currentUser: User;
    private userLocations = [];
    /*private userCurrentLocation: Location; 
    private managerUser: User;
    private currentUser: User;*/
    private userCred: any;
    private userTimeLogs = [];
    //  End - New Change - Fazi 
    /***
     * This Property is used in menu list to keep track of current tab ie: menu=0, deal=1, item list=2, special offer=3, MenuGroup=5
     * */
    private selectedMenuTab: number = 0;
    public selectedId: number = 0;
    public selOrderStatus: number = 0;


    constructor(/*private jsonApiService: JsonApiService*/) {
        debugger;
        this.user = new Subject();
        //this.locations = new BehaviorSubject<any>(null);
        this.currentEmployeeTimeLog = new BehaviorSubject<any>(null);
        //this.managerUser = new BehaviorSubject<any>(null);
        console.log('....new user service instance....');
        //  New Change - Fazi
        /*
        var user: User = JSON.parse(localStorage.getItem('SScurrentUser'));
        if (user) {
            this.currentUser = user;            
        }
        var locations: Location[] = JSON.parse(localStorage.getItem('SSLocations'));
        if (locations) {
            this.userLocations = locations;            
        }
        var location: Location = JSON.parse(localStorage.getItem('SSuserCurrentLocation'));
        if (location) {
            this.userCurrentLocation = location;            
        }
        var packageItem: any = JSON.parse(localStorage.getItem('SSselectedPakageItem'));
        if (packageItem) {
            this.selectedPackageItem = packageItem;
        } 
        //var currentTimelog: BehaviorSubject<any> = JSON.parse(localStorage.getItem('SScurrentEmployeeTimeLog'));
        //if (currentTimelog) {
        //    this.currentEmployeeTimeLog = currentTimelog;
        //}
        var xSelectedMenuTab: number = JSON.parse(localStorage.getItem('SSselectedMenuTab'));
        if (xSelectedMenuTab) {
            this.selectedMenuTab = xSelectedMenuTab;
        }*/
        
        /*var userCred1: any = JSON.parse(localStorage.getItem('currentUser'));
        if (userCred1) {
            this.userCred = userCred1;
        } 

        var xUserTimeLogs: any = JSON.parse(localStorage.getItem('SSuserTimeLogs'));
        if (xUserTimeLogs) {
            this.userTimeLogs = xUserTimeLogs;
            this.currentEmployeeTimeLog.next(this.userTimeLogs[this.userTimeLogs.length-1]);
        }    
        var xManagerUser: any = JSON.parse(localStorage.getItem('SSmanagerUser'));
        if (xManagerUser) {
            this.managerUser = xManagerUser;
            
        } */  
        //var pageError: any = JSON.parse(localStorage.getItem('SSerrorForPage'));
        //if (pageError) {
        //    this.errorForErrorPage = pageError;
        //}
    }

    /**
     * this function clears the local storage of shared service. use this function when the location list loads.
     * Author: Ali Shan
     * */
    ClearStorage() {
        // We can use the clear method but it may delete any item required even if loged out.
        // localStorage.clear()
        localStorage.removeItem('SScurrentUser');
        localStorage.removeItem('SSLocations');
        localStorage.removeItem('SSuserCurrentLocation');
        localStorage.removeItem('SSselectedPakageItem');
        localStorage.removeItem('SSselectedMenuTab');
        
        localStorage.removeItem('SSuserTimeLogs');
        this.currentEmployeeTimeLog.next(null);
    }
    getLoginInfo() /*: Observable<any> */{
       /* return this.jsonApiService.fetch('/user/login-info.json')
            .do((user) => {
                this.userInfo = user;
                this.user.next(user)
            })*/
    }

    private subject = new Subject<any>();

    //  Start - New Change For Current User - Fazi

    /**
     * This function is storing user data on local storage. For Refresh page
     * @param user
     */
    setCurrentUser(user: any) {
        localStorage.setItem('SScurrentUser', JSON.stringify(user));
        
        /*this.currentUser = user;*/
        
        
        //this.user.next(user);
    } 
    /**
     * This function is for getting user stored data from local storage.
     * */
    getCurrentUser() {
        // return this.currentUser;
    }
    //  End - New Change For Current User - Fazi
    //  Start - New Change For Current Location - Fazi
    //  For Location List
    /**
     * This function is for storing all user locations on local storage.
     * @param locations
     */
    setLocations(locations: any) {
        localStorage.setItem('SSLocations', JSON.stringify(locations));
        this.userLocations = locations;
        //this.locations.next(locations);
    }
    getLocations() {
        return this.userLocations;
    }
    /**
     * This function is for storing current user location on local storage.
     * @param location
     */
    setUserCurrentLocation(location: any) {
        localStorage.setItem('SSuserCurrentLocation', JSON.stringify(location));
      //   this.userCurrentLocation = location;
        //this.locations.next(locations);
    }    

    getUserCurrentLocation() {
     //   return this.userCurrentLocation;
    }

    
    //  End - New Change For Current Location - Fazi
    //setLocations(locations: any) {
    //    this.userLocations = locations;
    //    this.locations.next(locations);
    //}

    //clearLocations() {
    //    this.locations.next({});
    //}

    //getLocations(): Observable<any> {
    //    return this.locations.asObservable();
    //}

    setCurrentEmployeeTimeLog(timeLog: any) {
        debugger;
        

        // this.userTimeLogs.push(timeLog);
        localStorage.setItem('SSuserTimeLogs', JSON.stringify(this.userTimeLogs));
        
        // this.userCurrentLocation = this.userLocations.find(ul => ul.LocationId == timeLog.LocationId);

        // localStorage.setItem('SSuserCurrentLocation', JSON.stringify(this.userCurrentLocation));

        this.currentEmployeeTimeLog.next(timeLog);
    }

    clearCurrentEmployeeTimeLog() {
        localStorage.removeItem('SSuserTimeLogs');
        localStorage.removeItem('SSuserCurrentLocation');
        this.currentEmployeeTimeLog.next(null);
    }

    getCurrentEmployeeTimeLog(): Observable<any> {
        return this.currentEmployeeTimeLog .asObservable();
    }

    /**
     * 
     * @param managerUser
     */
    setManagerUser(managerUser: any) {
        //this.currentUser = managerUser;
        //this.managerUser.next(managerUser);
        // this.managerUser = managerUser;
       // localStorage.setItem('SSmanagerUser', JSON.stringify(this.managerUser));
    }

    clearManagerUser() {
        //this.managerUser.next(null);
    }
    
    getManagerUser() {
        debugger;    
        //return this.managerUser.asObservable();
        // return this.managerUser;
        
        //return this.locations.asObservable();
    }

    setOrderStatus(orderStatus: any) {
        this.userLocations = orderStatus;
        //this.selOrderStatus.next(orderStatus);
    }
    /**
     * This function is for storing user selected pakage data on local storage.
     * @param PackageItem
     */
    setSelectedPackageItem(PackageItem: any) {
        localStorage.setItem('SSselectedPakageItem', JSON.stringify(PackageItem));
        this.selectedPackageItem = PackageItem;
    }
    //  New Change
    getSelectedPackageItem() {
        return this.selectedPackageItem;
    }  
    /**
     * GET Selected Menu Tab. 
     * This Property is used in menu list to keep track of current tab ie: menu, deal, item list or special offer
     * */
    getSelectedMenuTab() {
        return this.selectedMenuTab;
    }
    /**
     * SET Selected Menu Tab. 
     * This Property is used in menu list to keep track of current tab ie: menu, deal, item list or special offer
     * */
    setSelectedMenuTab(selectedMenuTab: number) {
        localStorage.setItem('SSselectedMenuTab', JSON.stringify(selectedMenuTab));
        this.selectedPackageItem = selectedMenuTab;
    }
    /**
     * 
     * @param Error
     */
    //setErrorForErrorPage(Error: any) {
    //    localStorage.setItem('SSerrorForPage', JSON.stringify(Error));
    //    this.errorForErrorPage = Error;
    //}      
    //getErrorForErrorPage() {
    //    return this.errorForErrorPage;
    //} 
    
    setUserCred() {
        // this.userCred = JSON.parse(localStorage.getItem('currentUser'));        
    }
    //  New Change
    getUserCred() {
        return this.userCred;
    }
}   


// @Injectable()  
// export class CanActivateViaAuthGuard implements CanActivate {

//     constructor(private _userService: UserService) { }

//     canActivate() {
//         return this._userService.currentUser ? this._userService.currentUser.IsAdmin : false;
//     }
// }
