CREATE PROCEDURE [dbo].[usp_Users_GetDataBy_UserID]  
 @UserID BIGINT  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
      
  select UserID, AppID, AppRoleID, EMAIL AS UserEmail, FirstName,  MiddleName, LastName, PasswordHash, PasswordSalt, CREATE_DATE AS CreateDate, PhoneNumber   ,COMPANY_ID AS CompanyID, BRANCH_ID AS BranchID  from ITP_USERS_ST  
   
  where UserID = @UserID  
   
END