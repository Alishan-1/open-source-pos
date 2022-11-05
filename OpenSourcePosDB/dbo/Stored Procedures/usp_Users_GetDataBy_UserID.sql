CREATE PROCEDURE [dbo].[usp_Users_GetDataBy_UserID]  
 @UserID BIGINT  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
      
  select UserID, AppID, AppRoleID, EMAIL AS UserEmail, FirstName,  MiddleName, LastName, PasswordHash, PasswordSalt,  CreateDate, PhoneNumber   , CompanyID,  BranchID  from Users  
   
  where UserID = @UserID  
   
END