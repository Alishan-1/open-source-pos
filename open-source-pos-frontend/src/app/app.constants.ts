import { Injectable, Inject } from '@angular/core';
import { WINDOW } from './window.provider';


@Injectable()
export class Configuration {
  url: string = '';
  public domain: string;

  public Server = '';  
  public FileServer = '';

  /**
   * This Property contaion the url for geeting images (Seperate images server) 
   */
  public ImageServerUrl = '';
  /**
   * Main Backend Api URL is contained by this Property  
   */
  public WebApi = '';

  constructor(@Inject(WINDOW) private window:any) {
     debugger;
    let domain = this.window.location.hostname;
    this.domain = domain;

    this.Server = 'https://localhost:44390/';
    this.FileServer = 'https://localhost:44378/';
    // Do not Change anything here it will determin automatically


    if (domain === 'open-source-pos.alishah.pro') {
      this.WebApi = `http://open-source-pos.alishah.pro/api/api`;
      this.ImageServerUrl = 'https://open-source-pos.alishah.pro/api/api/';
    }
    else if (domain === 'localhost') {
      if (window.location.port === '82') { // for spain server
        this.WebApi = `http://localhost:82/api/api`;
        this.ImageServerUrl = 'http://localhost:9096/';
      }
      else if (window.location.port === '') { // for alishan pc
        this.WebApi = `http://localhost/api/api`;
        this.ImageServerUrl = 'http://localhost:9096/';
      }
      else {
        this.WebApi = `https://localhost:44333/api`; // for debuging local
        this.ImageServerUrl = 'http://localhost:9096/';
      }

    }
    else {
      this.WebApi = `http://localhost:27368/api`;
      this.ImageServerUrl = 'http://localhost:27368/';
    }


  }



  /** 
   *  Date Formate Used in application used in places where only date is required.
   *  currently this setting is "MM/dd/yyyy" same as (01/25/2019)
   */
  static readonly DateFormate: string = 'dd-MM-yyyy';
  /** 
   *  Time Formate Used in application used in places where only Time is required.
   *  currently this setting is "shortTime" same as "h:mm a" (9:05 AM)
   */
  static readonly TimeFormate: string = 'shortTime';
  /** 
   *  Date and Time Formate Used in application used in places where both Date and Time are required.     
   *  currently this setting is "medium" same as "MMM d, y h:mm:ss a" (Jan 5, 2016 9:05:05 AM)
   */
  static readonly DateTimeFormate: string = 'medium';


}

/**
 * Sample enum constant
 */
export enum PayTypeEnum {

  BankAccount = 47,
  CreditCard = 48,
  DebitCard = 49,
  Cash = 50
}




