  
-- =============================================  
-- Author:      Ali Shan  
-- Create date: 15-May-2018  
-- Description: user for user login  
-- =============================================  
CREATE PROCEDURE [dbo].[usp_UserLog_GetData_BySessionToken]  
 @UserID bigint,  
 @SessionToken varchar (MAX)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
      
  SELECT UserLogID, UserID, AppID, DeviceTypeID, SessStart, SessEnd, MachineIP, CreateDate, Ip, City, Region, Region_code, Country,  
    Country_name, Continent_code, In_eu, Postal, Latitude, Longitude, TimeZone, Utc_offset, Country_calling_code, Currency, Languages,  
    Asn, Org, IsAttemptSuccessful, AttemptDescription, IsAuthorisedCurrently, IsCredentialValid, IsLoginSuccessful, IsEmailCorrect,   
    IsPasswordExpired, LockoutEnabled, LockoutEnd, browser, os, device, userAgent, os_version, isMobile, isTablet, isDesktop, RememberUser,   
    SessionToken, TokenExpirationDate  
  FROM UserLog   
  WHERE UserID =@UserID AND SessionToken = @SessionToken;  
   
END