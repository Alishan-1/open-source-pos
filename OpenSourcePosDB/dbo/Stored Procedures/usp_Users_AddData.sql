-- =============================================  
-- Author:      Ali Shan  
-- Create date: 15-May-2018  
-- Description: used for User registration  
-- =============================================  
CREATE PROCEDURE [dbo].[usp_Users_AddData]  
 @AppID Bigint , @AppRoleID Bigint, @UserEmail VARCHAR(100), @FirstName VARCHAR(100),  
 @MiddleName VARCHAR(100), @LastName VARCHAR(100), @PasswordHash VARCHAR(MAX),   
 @PasswordSalt VARCHAR(MAX), @CreateDate datetime, @PhoneNumber VARCHAR(100),  
 @ExpirePassword datetime, @IsTemp bit, @IsDeleted bit, @IsAdmin bit,  
    @IsCustomer bit, @EmailConfirmed bit, @LockoutEnabled bit, @AccessFailedCount bigint, @IsActive bit  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 if exists (select * from ITP_USERS_ST where EMAIL = @UserEmail)  
  BEGIN return -1 END  
  --select top 1 * from users  
  declare @LOGIN_ID int =  (select ISNULL(MAX( LOGIN_ID),0) +1 from ITP_USERS_ST)
  declare @COMPANY_ID int = (select ISNULL( MAX(  COMPANY_ID), 0) +1 from ITP_USERS_ST)

  Insert into ITP_USERS_ST (AppID, AppRoleID, EMAIL, FirstName,  MiddleName, LastName, PasswordHash, PasswordSalt, CREATE_DATE, PhoneNumber,ExpirePassword,  
  IsTemp, IsDeleted, IsAdmin, IsCustomer, EmailConfirmed, LockoutEnabled, AccessFailedCount, IsActive, LOGIN_ID, [USER_NAME], USER_STATUS, COMPANY_ID, BRANCH_ID, PASSWORD, EMP_CODE, DAYLIMIT, FAQ1, ANS1, FAQ2, ANS2, USER_LOGIN_STATUS, CREATE_USER)  
  VALUES   (@AppID, @AppRoleID, @UserEmail, @FirstName, @MiddleName, @LastName, @PasswordHash, @PasswordSalt, @CreateDate, @PhoneNumber, @ExpirePassword, @IsTemp, @IsDeleted , @IsAdmin,  
   @IsCustomer ,@EmailConfirmed, @LockoutEnabled, @AccessFailedCount, @IsActive, @LOGIN_ID, @FirstName + CAST(@LOGIN_ID AS VARCHAR(MAX)), 'Y', @COMPANY_ID, 1, 'uui345', 1, 999, 'Who m i?', 'admin', 'Who m i?', 'admin', 'N', 1)  
  
  Select @@IDENTITY  
END