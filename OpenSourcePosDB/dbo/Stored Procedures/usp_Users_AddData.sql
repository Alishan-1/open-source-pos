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
 if exists (select * from Users where EMAIL = @UserEmail)  
  BEGIN return -1 END  
  --select top 1 * from users  
  
  declare @COMPANY_ID int = (select ISNULL( MAX(  CompanyID), 0) +1 from Users)

  Insert into Users (AppID, AppRoleID, Email, FirstName,  MiddleName, LastName, PasswordHash, PasswordSalt, CreateDate, PhoneNumber,ExpirePassword,  
  IsTemp, IsDeleted, IsAdmin, IsCustomer, EmailConfirmed, LockoutEnabled, AccessFailedCount, IsActive,  CompanyID)  
  VALUES   (@AppID, @AppRoleID, @UserEmail, @FirstName, @MiddleName, @LastName, @PasswordHash, @PasswordSalt, @CreateDate, @PhoneNumber, @ExpirePassword, @IsTemp, @IsDeleted , @IsAdmin,  
   @IsCustomer ,@EmailConfirmed, @LockoutEnabled, @AccessFailedCount, @IsActive, @COMPANY_ID)  
  
  Select @@IDENTITY  
END