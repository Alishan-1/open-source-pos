-- =============================================  
-- Author:      Ali Shan  
-- Create date: 15-May-2018  
-- Description: user for user login  
-- =============================================  
CREATE PROCEDURE [dbo].[usp_Users_GetDataBy_Email]  
 @UserEmail varchar (50)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
      
  select [UserID], [AppID], [AppRoleID], EMAIL AS UserEmail, [FirstName], [MiddleName], [LastName], [PhoneNumber], [StartDate], [EndDate], [IsActive], [IsSpecialOffer],  
   [IsCustomer], CREATE_USER AS [CreateID], CREATE_DATE AS [CreateDate], [ModifyDate], [AccessFailedCount], [ConcurrencyStamp], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [PasswordHash],  
    [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [IsAdmin], [DataEventRecordsRole], [SecuredFilesRole], [IsDeleted], [Primary], [Q1Id], [Q1Answer], [Q2Id],  
     [Q2Answer], [Q3Id], [Q3Answer], [PasswordSalt], [IsTemp], [UserPhoto], [PreviousPassword], [ExpirePassword], [IsForgetPassword]   
  -------------------------------------------  
  -------------------------------------------  
   ,COMPANY_ID AS CompanyID, BRANCH_ID AS BranchID  from ITP_USERS_ST   
  -------------------------------------------  
  ---------------------------------------------  
  where EMAIL =@UserEmail  
   
END