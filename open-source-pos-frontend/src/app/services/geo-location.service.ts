import { Injectable } from '@angular/core';
import { HttpClient as Http, HttpResponse as  Response, /*RequestOptions,*/ HttpHeaders as Headers } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ServiceResponse } from '../models/service-response.model';

import { UserGeoLocation } from '../models/UserGeoLocation.model'
import { UtilService } from "./util.service";

@Injectable({
    providedIn: 'root',
  })
export class GeoLocationService {
    private errorMsg: string = "Error message!!";
    constructor(private _http: Http, private _utilService: UtilService) { }

    /**
    * find the location of an IP address.
    * Specifically, you can get the following information for any IP address:
    * city, region , country , continent, postal code, latitude, longitude, timezone, utc_offset, european union (EU) membership, country calling code, currency, languages spoken, asn and organization.
    * author: Ali Shan
    *

     * Errors
     *   The API uses the following standard error codes:
     *
     *  Error Code	Reason	Response
     *  400	Bad Request
     *  404	URL Not Found
     *  405	Method Not Allowed
     *  429	Quota exceeded	{ �error� : true, reason": �RateLimited� }
     *  Additional possible errors with a 200 response may be :
     *
     *  For an invalid IP address, the response would be
     *  { "error" : true, "reason": "Invalid IP Address" }
     *
     *  For a reserved / non-public IP address, all fields except IP address would be null
     *  { "ip" : "127.0.0.1", 'city':null, ... }
     *
     *  If there�s no information available for a field, that particular field would be assigned a value of null
     *  @returns   Returns array object of
     */
    GetLocationFromIp()/*: Observable<UserGeoLocation>*/ {
        return this
            ._http.get('https://ipapi.co/111.119.187.13/json') // remove the fix ip from here
            /*.map(this.extractData)
            .catch(this._utilService.handleError)*/;
    }



    // private extractData(res: Response) {
    //     debugger;
    //     let body = res.json();
    //     return body || {};
    // }

    private handleError(error: any) {
        debugger;
        let serResponse: any = JSON.parse(error._body);
        let errMsg = (error.message) ? error.message :
            error.status ? `${error.status} - ${error.statusText}` : this.errorMsg;
        console.error(error, errMsg);
        // return Observable.throw(serResponse);
    }
}
