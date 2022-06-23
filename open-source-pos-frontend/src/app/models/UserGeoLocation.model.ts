
/**
 * represents the complete location information for an IP address.
 * 
 * */
export class UserGeoLocation {
    constructor() {

    }
    /**
    * public (external) IP address
    */
    ip: string = '';
    /**
    * city name
    */
    city: string = '';
    /**
    * region name (administrative division)
    */
    region: string = '';
    /**
    * region code
    */
    region_code: string = '';
    /**
    * country code (two letter, ISO 3166-1 alpha-2)
    */
    country: string = '';
    /**
    * country name
    */
    country_name: string = '';
    /**
    * continent code
    */
    continent_code: string = '';
    /**
    * whether IP address belongs to a country that is a member of European Union (EU)
    */
    in_eu: boolean = false;
    /**
    * postal code
    */
    postal: string = '';
    /**
    * latitude
    */
    latitude: number = 0;
    /**
    * longitude
    */
    longitude: number = 0;
    /**
    * timezone (IANA format i.e. �Area/Location�)
    */
    timezone: string = '';
    /**
    * UTC offset as +HHMM or -HHMM (HH is hours, MM is minutes)
    */
    utc_offset: string = '';
    /**
    * country calling code (dial in code, comma separated)
    */
    country_calling_code: string = '';
    /**
    * currency code (ISO 4217)
    */
    currency: string = '';
    /**
    * languages spoken (comma separated 2 or 3 letter ISO 639 code with optional hyphen separated country suffix)
    */
    languages: string = '';
    /**
    * autonomous system number
    */
    asn: string = '';
    /**
    * organinzation name
    */
    org: string = '';
}
