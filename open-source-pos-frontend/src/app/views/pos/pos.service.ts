//1. Import all dependencies
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';

// import { Http, Response, Request, RequestOptions, Headers } from '@angular/http';
import { Configuration } from '../../app.constants';
import { AuthService } from '../login/auth.service';

import { ServiceResponse } from '../../models/service-response.model';

// import { ChowChoiceRequestOptions } from '../../../app.request-options';

import { UtilService } from '../../services/util.service';
import { POS, InvoiceMasterListing, InvoiceDetailItems, InvoiceMaster } from '../../models/posTrans';

@Injectable({
  providedIn: 'root'
})
export class PosService {


    
    apiUrl: string ;
    
    
    headers = new HttpHeaders().set('Content-Type', 'application/json');

    //4. Passsing the Http dependency to the constructor to access Http functions
  constructor(private http: HttpClient, private _configuration: Configuration, private _utilService: UtilService, 
    private _authService: AuthService) {
       this.apiUrl = _configuration.WebApi;
    }

    GetSearchItems(data: any): Observable<any> {
      let API_URL = `${this.apiUrl}/POS/GetSearchItems`;
      return this.http.post(API_URL, data, { headers: this._authService.GetHttpHeaders() })
        .pipe(
          catchError(this._utilService.handleError)
        )
    }

    GetInvoicesList(data: any): Observable<any> {
      let API_URL = `${this.apiUrl}/POS/getinvoices`;
      return this.http.post(API_URL, data, { headers: this._authService.GetHttpHeaders() })
        .pipe(
          catchError(this._utilService.handleError)
        )
    }
  
    SavePosTrans(data:POS): Observable<any> {
      let API_URL = `${this.apiUrl}/POS`;
      return this.http.post(API_URL, data, { headers: this._authService.GetHttpHeaders() })
        .pipe(
          catchError(this._utilService.handleError)
        )
    }
    
    UpdatePosTrans(data:POS): Observable<any> {
      let API_URL = `${this.apiUrl}/POS`;
      return this.http.put(API_URL, data, { headers: this._authService.GetHttpHeaders() }).pipe(
        catchError(this._utilService.handleError)
      )      
    }
    
    DeleteInvDetail(data:InvoiceDetailItems): Observable<any> {
      let API_URL = `${this.apiUrl}/POS/DeleteInvDetail`;
      return this.http.post(API_URL, data, { headers: this._authService.GetHttpHeaders() }).pipe(
        catchError(this._utilService.handleError)
      )      
    }

    /**
     * Delete whole invoice including all details if it is not posted.
     * @param data Invoice to delete
     * @returns any
     */
    DeleteInvoice(data:InvoiceMaster): Observable<any> {
      let API_URL = `${this.apiUrl}/POS`;
      return this.http.delete(API_URL, { headers: this._authService.GetHttpHeaders(), body: data }).pipe(
        catchError(this._utilService.handleError)
      )      
    }
    
    GetInvoiceDetails(data: InvoiceMasterListing): Observable<any> {
      let API_URL = `${this.apiUrl}/POS/getinvoicedetails`;
      return this.http.post(API_URL, data, { headers: this._authService.GetHttpHeaders() })
        .pipe(
          catchError(this._utilService.handleError)
        )
    }



    // Create
    createTask(data: any): Observable<any> {
      let API_URL = `${this.apiUrl}/create-task`;
      return this.http.post(API_URL, data, { headers: this._authService.GetHttpHeaders() })
        .pipe(
          catchError(this._utilService.handleError)
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
        catchError(this._utilService.handleError)
      )
    }
  
    // Delete
    deleteTask(id: any): Observable<any> {
      var API_URL = `${this.apiUrl}/delete-task/${id}`;
      return this.http.delete(API_URL).pipe(
        catchError(this._utilService.handleError)
      )
    }
}
