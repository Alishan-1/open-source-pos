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
import { posItem, POS } from '../../models/posTrans';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  apiUrl: string ;
    
    
  headers = new HttpHeaders().set('Content-Type', 'application/json');

  //4. Passsing the Http dependency to the constructor to access Http functions
  constructor(private http: HttpClient, private _configuration: Configuration, private _utilService: UtilService, 
    private _authService: AuthService) {
      this.apiUrl = _configuration.WebApi;
      this.apiUrl = `${this.apiUrl}/item`;
    }
  
  GetItems(data: any): Observable<any> {
    debugger;
    let API_URL = `${this.apiUrl}/getitems`;
    return this.http.post(API_URL, data, { headers: this._authService.GetHttpHeaders() })
      .pipe(
        catchError(this.error)
      )
  }

  SaveItem(data:posItem): Observable<any> {    
    debugger;
    return this.http.post(this.apiUrl, data, { headers: this._authService.GetHttpHeaders() })
      .pipe(
        catchError(this.error)
      )
  }
  
  UpdateItem(data:posItem): Observable<any> {
    return this.http.put(this.apiUrl, data, { headers: this._authService.GetHttpHeaders() }).pipe(
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
      debugger;
      let errorMessage = '';
      if(error.error.Errors){
        for (const property in error.error.Errors) {
          
          console.log(`${property}: ${error.error.Errors[property]}`);
          error.error.Errors[property].forEach(function(err: any){
            errorMessage += property+': '+err+' \n ';
          })

        }
      }      
      else if (error.error instanceof ErrorEvent) {
        errorMessage = error.error.message;
      }
      else if(error.error){
        errorMessage += error.error;
      }
      else {
        errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
      }
      console.log(errorMessage);
      return throwError(errorMessage);
    }
}