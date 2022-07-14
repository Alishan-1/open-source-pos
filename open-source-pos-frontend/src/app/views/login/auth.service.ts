//1. Import all dependencies
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';

import { Configuration } from '../../app.constants';


import { ServiceResponse } from '../../models/service-response.model';

// import { ChowChoiceRequestOptions } from '../../../app.request-options';

import { UtilService } from '../../services/util.service';
import { POS } from '../../models/posTrans';

@Injectable({
  providedIn: 'root'
})
export class AuthService {


    
    apiUrl: string ;
    
    
    headers = new HttpHeaders().set('Content-Type', 'application/json');

    //4. Passsing the Http dependency to the constructor to access Http functions
    constructor(private http: HttpClient, private _configuration: Configuration, private _utilService: UtilService, ) {
       this.apiUrl = _configuration.WebApi;
    }

  /**
   * Used to get user from token provided from asp.net form.
   * @param data data.Token is the required token
   */
  GetCurrentUser(data: any): Observable<any> {
    debugger;
    this.headers = new HttpHeaders().set('Content-Type', 'application/json');
    this.headers = this.headers.append('Authorization', `Bearer ${data.Token}`);


    let API_URL = `${this.apiUrl}/User/GetCurrentUser`;
    return this.http.post(API_URL, data, { headers: this.headers })
      .pipe(
        catchError(this.error)
      )
  }
  /**
   * Get the Current user object
   * @returns Currently loged in user object
   */
  GetlocalStorageUser(): any {
    debugger;
    let currentUser = JSON.parse(localStorage.getItem('currentUser') || "{}");
    return currentUser;
  }

  GetHttpHeaders(): HttpHeaders {
    let headrs = new HttpHeaders().set('Content-Type', 'application/json');
    let currentUser = JSON.parse(localStorage.getItem('currentUser') || "{}");
    if (currentUser && currentUser.Token) {
      headrs = headrs.append('Authorization', `Bearer ${currentUser.Token}`);
    }
    return headrs;
  }


  GetSearchItems(data: any): Observable<any> {
      let API_URL = `${this.apiUrl}/POS/GetSearchItems`;
      return this.http.post(API_URL, data)
        .pipe(
          catchError(this.error)
        )
    }
  
    SavePosTrans(data:POS): Observable<any> {
      let API_URL = `${this.apiUrl}/POS`;
      return this.http.post(API_URL, data)
        .pipe(
          catchError(this.error)
        )
    }
    
    UpdatePosTrans(data:POS): Observable<any> {
      let API_URL = `${this.apiUrl}/POS`;
      return this.http.put(API_URL, data, { headers: this.headers }).pipe(
        catchError(this.error)
      )      
    }
    // Create
    createTask(data: any): Observable<any> {
      let API_URL = `${this.apiUrl}/create-task`;
      return this.http.post(API_URL, data)
        .pipe(
          catchError(this.error)
        )
    }
  
    // Read
    showTasks() {
      return this.http.get(`${this.apiUrl}`);
    }
  
    // Update
    updateTask(id: any, data: any): Observable<any> {
      let API_URL = `${this.apiUrl}/update-task/${id}`;
      return this.http.put(API_URL, data, { headers: this.headers }).pipe(
        catchError(this.error)
      )
    }
  
    // Delete
    deleteTask(id: any): Observable<any> {
      var API_URL = `${this.apiUrl}/delete-task/${id}`;
      return this.http.delete(API_URL).pipe(
        catchError(this.error)
      )
    }
  
    // Handle Errors 
    error(error: HttpErrorResponse) {
      let errorMessage = '';
      if (error.error instanceof ErrorEvent) {
        errorMessage = error.error.message;
      } else {
        errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
      }
      console.log(errorMessage);
      return throwError(errorMessage);
    }
  
  //   public saveImage(image: string): Observable<Response> {                
  //       let params: any = {};
  //       params.Path = image;
  //       return this._http.post(this.createUploadImageUrl, JSON.stringify(params), this._ccro.GetChowChoiceRequestOptions())
  //           .map(this.extractData)
  //           .catch(this._utilService.handleError);
  //   }
  //   //  End Latest Change For Image -   Fazi
  //   createEmployeeInvitationJwt(Employee: EmployeeModel): Observable<ServiceResponse> {//service response
                
  //       return this
  //           ._http
  //           .post(`${this._configuration.WebApi}api/User/add/employee`,
  //                   JSON.stringify(Employee), this._ccro.GetChowChoiceRequestOptions())
  //           .map(this.extractData)
  //           .catch(this._utilService.handleError);
  //   }

  //   createUserInID(Employee: EmployeeModel): Observable<ServiceResponse> {//service response

  //       let header = new Headers({ 'Content-Type': 'application/json' });
  //       let options = new RequestOptions({ headers: header });
  //       return this
  //           ._http
  //           .post(this.createUserIdpUrl, JSON.stringify(Employee), options)
  //           .map(this.extractData)
  //           .catch(this._utilService.handleError);
  //   }
  //   private extractData(res: Response) {
  //     debugger;
  //     let body = res.json();
  //     return body || {};
  // }

   
  //   updEmployeeJobs(ManagerUserId: number, LocationId: number, EmployeeJobId: number, IsDelivery: boolean): Observable<ServiceResponse> {//service response

  //       let params: any = {};
  //       params.ManagerUserId = ManagerUserId;
  //       params.LocationId = LocationId;
  //       params.EmployeeJobId = EmployeeJobId;
  //       params.IsDeliveryPerson = IsDelivery;

        
  //       return this
  //           ._http
  //           .post(this.updEmployeeJobUrl, JSON.stringify(params), this._ccro.GetChowChoiceRequestOptions())
  //           .map(this.extractData)
  //           .catch(this._utilService.handleError);
  //   }

    
  //   /**
  //    * For Deleting employee attandance
  //    * @param employeeTimeLogId
  //    */
  //   DeleteTimeLog(employeeTimeLogId: number): Observable<ServiceResponse> {

       
  //       let DeleteTimeLogUrl = `${this._configuration.WebApi}api/EmployeeTimeLog/?employeeTimeLogID=${employeeTimeLogId}`;
  //       return this
  //           ._http.delete(DeleteTimeLogUrl, this._ccro.GetChowChoiceRequestOptions())
  //           .map(this.extractData)
  //           .catch(this._utilService.handleError);

  //       // service written by alishan
  //       //.catch(this._utilService.handleErrorWithNotification);
  //   }

  //   getValidations(): Observable<ServiceResponse> {

        
  //       return this
  //           ._http.get(this.getValidationUrl, this._ccro.GetChowChoiceRequestOptions())
  //           .map(this.extractData)
  //           .catch(this._utilService.handleError);
  //   }
    
    
    

}
