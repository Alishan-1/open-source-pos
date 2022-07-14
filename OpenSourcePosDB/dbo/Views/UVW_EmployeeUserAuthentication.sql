-- ==================================================  
-- Author:      ali shah  
-- Create date: 18-OCT-2018  
-- Description: Checking Employee User Authentication  
--Used in admin side   
-- ==================================================  
  
CREATE VIEW [dbo].[UVW_EmployeeUserAuthentication]  
AS  
 SELECT U.UserID,U.AppID,U.AppRoleID,U.FirstName, U.MiddleName,U.LastName,U.StartDate,U.EndDate,U.AccessFailedCount,  
 U.EMAIL  as UserEmail, U.EmailConfirmed,U.LockoutEnabled,U.LockoutEnd,  
 U.PasswordHash,U.IsTemp,U.PreviousPassword,U.ExpirePassword, U.IsDeleted, IsActive  
 --, CASE  
  --  WHEN U.LockoutEnd <= getdate() THEN 1  
      
   -- ELSE 0  
--END as LockoutEnd ,  
 -- CASE  
 --   WHEN U.ExpirePassword <= getdate() THEN 1  
      
   -- ELSE 0  
--END as PasswordExpired  
  
  from ITP_USERS_ST U   
  --where u.IsActive = 1 and u.EmailConfirmed = 1 and u.LockoutEnabled = 0  and u.IsTemp = 0  