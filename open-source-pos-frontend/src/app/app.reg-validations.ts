import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
  })
export class RegularExpressions {

    
    //public OnlyAlphabets = '^[A-Za-z-_. ]+$';// /^([A-Za-z]+)$/;
    //public AlphaNumeric = '^[0-9A-Za-z-_. ]+$';
    //public AlphaNumericSymbol = '^[0-9A-Za-z#&_.,-/ ]+$';
    //public OnlyNumbers = '^[0-9]+$';
    //public OnlyDecimals = '^[0-9]+[.?][0-9]+$';
    //public PhoneNo = '^[0-9]{10}$';//[(0-9)]{5}[ 0-9-]{9}
    //public NumberOrDecimal = '^[0-9]+([.][0-9]+)?$';
    //public PostCode = '^[0-9A-Za-z]{3,7}$';

    //public OnlyAlphabets = '^[A-Za-z-_.]+([A-Za-z-_. ]+)?$';// /^([A-Za-z]+)$/;
    //public AlphaNumeric = '^[0-9A-Za-z-_.]+([0-9A-Za-z-_. ]+)?$';
    //public AlphaNumericSymbol = '^[0-9A-Za-z#&_.,-/]+([0-9A-Za-z#&_.,-/ ]+)?$';
    //public OnlyNumbers = '^[0-9]+$';
    //public OnlyDecimals = '^[0-9]+[.?][0-9]+$';
    //public PhoneNo = '^[0-9]{10}$';//[(0-9)]{5}[ 0-9-]{9}
    //public NumberOrDecimal = '^[0-9]+([.][0-9]+)?$';
    //public PostCode = '^[0-9A-Za-z]{3,7}$';

    public OnlyAlphabets = '^([A-Za-z-_.]?[A-Za-z-_. ]+)?$';// /^([A-Za-z]+)$/;
    public AlphaNumeric = '^([0-9A-Za-z-_.]+[0-9A-Za-z-_. ]+)?$';
    public AlphaNumericSymbol = "^([0-9A-Za-z#&_.,-/']+[0-9A-Za-z#&_.,-/ ']+)?$";  
    public Email = '^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$';
    public Search = '^[0-9A-Za-z#&_.,-]+([0-9A-Za-z#&_.,-@ ]+)?$';
    public OnlyNumbers = '^[0-9]+$';
    public OnlyDecimals = '^[0-9]+[.?][0-9]+$';
    public PhoneNo = '^[0-9]{10}$';//[(0-9)]{5}[ 0-9-]{9}
    public NumberOrDecimal = '^[0-9]+([.][0-9]+)?$';
    public PostCode = '^[0-9A-Za-z]{3,7}$';
    public Time = '^((0?[1-9]|1[012])(:[0-5]\d){0,2}(\ [AP]M))$|^([01]\d|2[0-3])(:[0-5]\d){0,2}$';
    public Password = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
    public PhoneNumber = "\d*";
    public NoOfHours = "/\b(0?[1-9]|1[0-9]|2[0-4])\b/g";
    public isMatch(reg: any, value: any): boolean {

        let rg = new RegExp(reg);
        return rg.test(value);
    }
    /**
    * Checks whather the string is a valid email
    * author: Ali Shan
     
    * https://emailregex.com/ Email Address Regular Expression That 99.99% Works
    * @param value   string to check for email.
    * @returns       returns true if the value is a valid email otherwise returns false.
    */
    public isValidEmail(value: string): boolean {

        return /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(value);
    }
}

///((\[?(\d{1,3}\.){3}\d{1,3}\]?)|(([-a-zA-Z0-9]+\.)+[a-zA-Z]{2,4}))(\:\d+)?(/[-a-zA-Z0-9._?,'+&amp;%$#=~\\]+)*/?)$""