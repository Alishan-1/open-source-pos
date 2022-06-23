




export interface AuthenticationResult {
    IsAuthorisedCurrently: boolean;
    IsCredentialValid: boolean;
    IsLoginSuccessful: boolean;
    IsEmailCorrect: boolean;
    IsPasswordExpired: boolean;
    IsLockoutEnabled: boolean;
    LockoutEndTime?: any;
}

export interface UsersGeoLocation {
    ip: string;
    city: string;
    region: string;
    region_code: string;
    country: string;
    country_name: string;
    continent_code: string;
    in_eu: boolean;
    postal: string;
    latitude: number;
    longitude: number;
    timezone: string;
    utc_offset: string;
    country_calling_code: string;
    currency: string;
    languages: string;
    asn: string;
    org: string;
}

export interface User {
    UserID: number;
    AppID: number;
    AppRoleID: number;
    UserPassword?: any;
    UserEmail: string;
    FirstName: string;
    MiddleName: string;
    LastName: string;
    PasswordHash?: any;
    PasswordSalt?: any;
    CreateDate: Date;
    PhoneNumber: string;
    Token: string;
    UserPhoto?: any;
    PreviousPassword: string;
    ExpirePassword: Date;
    IsTemp: boolean;
    IsDeleted: boolean;
    IsAdmin: boolean;
    IsCustomer: boolean;
    EmailConfirmed: boolean;
    LockoutEnabled: boolean;
    IsPasswordExpired: boolean;
    IsPasswordCorrect: boolean;
    LockoutEnd?: any;
    AccessFailedCount: number;
    IsActive: boolean;
    StartDate: Date;
    EndDate?: any;
    LinkOrCode?: any;
    IsForgetPassword: boolean;
    CurrentPassword?: any;
    authenticationResult: AuthenticationResult;
    UsersGeoLocation: UsersGeoLocation;
    DeviceInfo: any;
    RememberUser: boolean;
    SessionToken: string;
    TokenExpirationDate: Date;
    SessionDate: Date;
    // OLD NOT TO BE USED PROPERTIES
    /**
     * OLD NOT TO BE USED PROPERTIES
     * */
    name: string,
    /**
     * OLD NOT TO BE USED PROPERTIES
     * */
    contact: string,
    /**
     * OLD NOT TO BE USED PROPERTIES
     * */
    email: string
}



