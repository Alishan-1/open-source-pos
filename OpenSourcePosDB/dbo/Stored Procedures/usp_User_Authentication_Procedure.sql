-- =============================================  
-- Author:Ali Shan   
-- Create date: 22-October-2018  
-- Description: used for user authentication and use log  
  
-- Return Value Codes  
-- 5 = 'You are not authorized at this time please try again later'  
-- 10 = 'Invalid Login or Password'  
-- 15 = 'Login Successfully'  
-- 20 = 'Incorrect Email'  
-- 25 = Your password is expired please change it  
-- =============================================  
/*  
select * from LookUp where Category = 'DEVICETYPE'  
select * from UserLog  
*/  
  
  
  
  
CREATE PROCEDURE [dbo].[usp_User_Authentication_Procedure]  
  @UserEmail varchar(50),  
  @IsPasswordValid bit,  
  @UserLocalDate Datetime,     
    
        -- (external) IP address          
        @ip varchar(MAX),          
        -- city name          
        @city  varchar(MAX),          
        -- region name (administrative division)          
        @region  varchar(MAX),          
        -- region code          
        @region_code  varchar(MAX),          
        -- country code (two letter, ISO 3166-1 alpha-2)          
        @country  varchar(MAX),          
        -- country name          
        @country_name  varchar(MAX),          
        -- continent code          
        @continent_code  varchar(MAX),          
        -- whether IP address belongs to a country that is a member of European Union (EU)          
        @in_eu  bit,          
        -- postal code          
        @postal  varchar(MAX),          
        -- latitude          
        @latitude Decimal(19,10),          
        -- longitude          
        @longitude  Decimal(19,10),          
        -- timezone (IANA format i.e. “Area/Location”)          
        @timezone varchar(MAX),          
        -- UTC offset as +HHMM or -HHMM (HH is hours, MM is minutes)          
        @utc_offset varchar(MAX),          
        -- country calling code (dial in code, comma separated)          
        @country_calling_code varchar(MAX),          
        -- currency code (ISO 4217)          
        @currency varchar(MAX),          
        -- languages spoken (comma separated 2 or 3 letter ISO 639 code with optional hyphen separated country suffix)          
        @languages varchar(MAX),          
        -- autonomous system number          
        @asn varchar(MAX),          
        -- organinzation name          
        @org varchar(MAX),  
  
  @browser VARCHAR(MAX) = '', @os VARCHAR(MAX) = '', @device VARCHAR(MAX) = '', @userAgent VARCHAR(MAX) = '',  
  @os_version VARCHAR(MAX) = '', @isMobile bit = 0, @isTablet bit = 0, @isDesktop bit = 0,  
  --identifies wheather to remember user on next visit  
  @RememberUser bit = 0,  
  -- used to authenticate session and used in RememberUser functionality  
  @SessionToken VARCHAR(MAX) = '',  
  --indicates the date when session will be expired forcibly  
  @TokenExpirationDate DateTimeOffset(2) = '1900-01-01',  
  --used as session start date  
  @SessionDate DateTimeOffset(2) = '1900-01-01'  
  
  AS  
  DECLARE  
 @UserID bigint , @AppID bigint, @AppRoleID bigint, @AccessFailedCount int,  
 @EmailConfirmed bit,@LockoutEnabled bit, @LockoutEnd datetimeoffset, @IsTemp bit,  
 @ExpirePassword datetime,  @IsDeleted bit,  
 @IsActive bit, @DeviceTypeID bigint = null, @AttemptDescription VARCHAR(MAX) = 'Log from usp_User_Authentication_Procedure'  
 DECLARE @IsAuthorisedCurrently bit, @IsCredentialValid bit, @IsLoginSuccessful bit, @IsEmailCorrect bit, @IsPasswordExpired bit,  
   @WebDeviceTypeId bigint, @MobileDeviceTypeId bigint, @TabletDeviceTypeId bigint  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
 select @WebDeviceTypeId= LookUpID from LookUp where Category = 'DEVICETYPE' and Description = 'WEB'  
 select @MobileDeviceTypeId= LookUpID from LookUp where Category = 'DEVICETYPE' and Description = 'MOBILE'  
 select @TabletDeviceTypeId= LookUpID from LookUp where Category = 'DEVICETYPE' and Description = 'tablet'  
  
 --it is gurenteed that one of these flags will be true  
 IF @isMobile = 1  
 BEGIN  
  SET @DeviceTypeID = @MobileDeviceTypeId  
 END  
 ELSE IF @isDesktop = 1  
 BEGIN  
  SET @DeviceTypeID = @WebDeviceTypeId  
 END  
 ELSE IF @isTablet = 1  
 BEGIN  
  SET @DeviceTypeID = @TabletDeviceTypeId  
 END  
  
    set @IsLoginSuccessful = 0;  
 set @IsPasswordExpired = 0;  
          IF EXISTS(select UserID, AppID, AppRoleID, EMAIL as UserEmail, FirstName,  MiddleName, LastName, PasswordHash, PasswordSalt, CreateDate, PhoneNumber,ExpirePassword,  
  IsTemp, IsDeleted, IsAdmin, IsCustomer, EmailConfirmed, LockoutEnabled, AccessFailedCount, IsActive   
  from Users   
  where EMAIL = @UserEmail)  
    BEGIN  
    SET @IsEmailCorrect = 1;  
    SET @IsAuthorisedCurrently = @IsPasswordValid;  
    SET @IsCredentialValid = @IsPasswordValid;  
    select @UserID = UserID,@AppID=AppID,@AppRoleID=AppRoleID,@AccessFailedCount=AccessFailedCount, @EmailConfirmed=EmailConfirmed,@LockoutEnabled= LockoutEnabled,@LockoutEnd= LockoutEnd,  
     @IsTemp = IsTemp,@ExpirePassword= ExpirePassword, @IsDeleted = IsDeleted, @IsActive = IsActive  
    from UVW_EmployeeUserAuthentication where    UserEmail =  @UserEmail  
  
     /*Check if the user is locked out and lockout is not ended*/  
     if (@LockoutEnabled = 1 and @LockoutEnd >= SYSDATETIMEOFFSET() )  
     BEGIN  
      IF (@IsPasswordValid = 0)  
        BEGIN  
                                    Update Users     
                                            Set AccessFailedCount = AccessFailedCount + 1,  
                                                LockoutEnabled = 1,  
                                                LockoutEnd = DATEADD(MINUTE,10,SYSDATETIMEOFFSET())  
            Where EMAIL = @UserEmail  
          SET @IsCredentialValid = 0;            
          SET @AttemptDescription = @AttemptDescription + ', Still user''s password is invalid';  
        END        
                           SET @IsAuthorisedCurrently = 0;  
         SET @AttemptDescription = @AttemptDescription + ', User is locked out because of un successful attempts';  
     END  
     /*Check if the user is locked out and lockout ended*/  
     ELSE if (@LockoutEnabled = 1 and @LockoutEnd < SYSDATETIMEOFFSET() )  
     BEGIN  
      IF (@IsPasswordValid = 0)  
        BEGIN  
                                    Update Users     
                                            Set AccessFailedCount = AccessFailedCount + 1,  
                                                LockoutEnabled = 1,  
                                                LockoutEnd = DATEADD(MINUTE,10,SYSDATETIMEOFFSET())  
            Where EMAIL = @UserEmail  
          SET @IsCredentialValid = 0;            
          SET @AttemptDescription = @AttemptDescription + ', Still user''s password is invalid';  
        END        
                           SET @IsAuthorisedCurrently = 0;  
         SET @AttemptDescription = @AttemptDescription + ', User is locked out because of un successful attempts';  
         If (@IsPasswordValid = 1 )  
       BEGIN  
           Update Users     
             Set AccessFailedCount = 0,  
              LockoutEnabled = 0,  
              LockoutEnd = '1900-01-01'  
              Where EMAIL = @UserEmail  
         SET @IsAuthorisedCurrently = 1;  
         SET @IsCredentialValid = 1;            
         SET @IsLoginSuccessful = 1;             
         SET @AttemptDescription = @AttemptDescription + ', user''s password is valid and lockout period is ended so he is allowed to login';  
        END  
     END  
       
       
     /*Lock the user if too many invalid login attempts*/  
     if (@LockoutEnabled = 0 and (@LockoutEnd = '1900-01-01' OR @LockoutEnd is null))  
      BEGIN  
                            if (@IsPasswordValid = 0)  
                                   BEGIN  
            if (@AccessFailedCount < 3)  
            BEGIN  
             Update Users  
              Set AccessFailedCount = AccessFailedCount + 1  
              Where EMAIL = @UserEmail  
             SET @IsCredentialValid = 0;  
             SET @IsAuthorisedCurrently = 0;  
             SET @AttemptDescription = @AttemptDescription + ', user''s password is invalid for ' +CAST( @AccessFailedCount +1 AS VARCHAR(MAX))+ ' time(s) Not Authorised';  
             END    
            Else if (@AccessFailedCount >= 3)  
            BEGIN  
             Update Users     
              Set AccessFailedCount = AccessFailedCount + 1,  
               LockoutEnabled = 1,  
               LockoutEnd = DATEADD(MINUTE,10,SYSDATETIMEOFFSET())  
               Where EMAIL = @UserEmail  
             SET @IsCredentialValid = 0;  
             SET @IsAuthorisedCurrently = 0;  
             SET @AttemptDescription = @AttemptDescription + ', user''s password is invalid for ' +CAST( @AccessFailedCount +1 AS VARCHAR(MAX))+ ' time(s) Not Authorised And Locked for 10 minutes';  
             END  
           END  
        ELSE  
         BEGIN  
           SET @IsLoginSuccessful = 1  
           SET @AttemptDescription = @AttemptDescription + ', User''s password is valid and allowed to login';  
         END  
          END  
  
  
     /*Check if the user's password is expired*/  
     IF (@ExpirePassword <= GETDATE())  
     BEGIN  
      SET @IsPasswordExpired = 1  
     END  
  
     -- 5 = 'You are not authorized at this time please try again later'  
     -- 10 = 'Invalid Login or Password'  
     -- 15 = 'Login Successfully'  
     -- 20 = 'Incorrect Email'  
     -- 25 = Your password is expired please change it  
       
       
    END  
  
    ELSE  
    BEGIN  
     SET @IsEmailCorrect = 0;  
    END  
    Select @LockoutEnd = LockoutEnd, @LockoutEnabled = LockoutEnabled from  Users   Where EMAIL = @UserEmail  
  
    --insert data in userlog      
    Insert Into UserLog (UserID, AppID, DeviceTypeID, SessStart,  MachineIP, CreateDate,  Ip,  City, Region,  Region_code, Country, Country_name, Continent_code,  In_eu, Postal,  Latitude, Longitude, TimeZone, Utc_offset,  Country_calling_code, Currency,
 Languages, Asn, Org, IsAttemptSuccessful, AttemptDescription,  IsAuthorisedCurrently, IsCredentialValid, IsLoginSuccessful, IsEmailCorrect,  IsPasswordExpired, LockoutEnabled,  LockoutEnd,  browser, os,  device,  userAgent, os_version,  isMobile, isTablet, isDesktop, RememberUser, SessionToken, TokenExpirationDate)  
    Values    (@UserID, @AppID, @DeviceTypeID, @SessionDate, @ip,  @UserLocalDate, @ip, @City, @Region, @Region_code, @Country, @Country_name, @Continent_code, @In_eu, @Postal, @Latitude, @Longitude, @TimeZone, @Utc_offset, @Country_calling_code, @Currency, @Languages, @Asn, @Org, @IsLoginSuccessful,  @AttemptDescription, @IsAuthorisedCurrently, @IsCredentialValid, @IsLoginSuccessful, @IsEmailCorrect, @IsPasswordExpired, @LockoutEnabled, @LockoutEnd, @browser, @os, @device, @userAgent, @os_version, @isMobile, @isTablet, @isDesktop, @RememberUser, @SessionToken, @TokenExpirationDate)  
    -- return results  
    SELECT @IsAuthorisedCurrently AS IsAuthorisedCurrently, @IsCredentialValid AS IsCredentialValid,  
         @IsLoginSuccessful AS IsLoginSuccessful, @IsEmailCorrect AS IsEmailCorrect, @IsPasswordExpired AS IsPasswordExpired,  
         @LockoutEnabled AS IsLockoutEnabled,@LockoutEnd AS LockoutEndTime  
END  
  