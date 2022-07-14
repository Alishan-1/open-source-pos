-- =============================================  
-- Author:      Ali Shan  
-- Create date: 2018/06/10  
-- Description: UsersBy Email Update  
--in usp_Users_UpdDataBy_Email UserEmail and Userid both are requied   
--all other parameters are optional and the column value will not be changed if the parameter value is null  
-- =============================================  
CREATE  PROCEDURE [dbo].[usp_Users_UpdDataBy_Email]   
 @UserEmail VARCHAR(100),@UserID bigint, @AppID Bigint = NULL , @AppRoleID Bigint = NULL,  @FirstName VARCHAR(100) = NULL,  
 @MiddleName VARCHAR(100) = NULL, @LastName VARCHAR(100) = NULL, @PasswordHash VARCHAR(MAX) = NULL,   
 @PasswordSalt VARCHAR(MAX) = NULL, @PhoneNumber VARCHAR(100) = NULL, @ExpirePassword datetime = NULL, @IsTemp bit = NULL, @IsDeleted bit = NULL,  
 @IsAdmin bit = NULL, @IsCustomer bit = NULL, @EmailConfirmed bit = NULL, @LockoutEnabled bit = NULL, @AccessFailedCount bit = NULL, @IsActive bit = NULL,  
 @PreviousPassword VARCHAR(MAX) = NULL, @IsForgetPassword bit = NULL, @UserPhoto varchar(max) = null  
AS  
BEGIN   
 SET NOCOUNT ON;  
    SET XACT_ABORT,  
    QUOTED_IDENTIFIER,  
    ARITHABORT,  
          ANSI_NULLS,  
    ANSI_PADDING,  
    ANSI_WARNINGS,  
    CONCAT_NULL_YIELDS_NULL ON;  
    SET NUMERIC_ROUNDABORT OFF;  
   
 DECLARE @transactionName VARCHAR(32) = REPLACE((CAST(NEWID() AS VARCHAR(36))),'-','')  
   
 BEGIN TRY  
    
  DECLARE @TranCounter INT;  
  SET @TranCounter = @@TRANCOUNT;  
  IF @TranCounter > 0  
   -- Procedure called when there is an active transaction. Create a savepoint to be able to roll back only the work done in the procedure if there is an error.  
   SAVE TRANSACTION @transactionName;  
  ELSE  
   -- Procedure must start its own transaction.  
   BEGIN TRANSACTION  
    -- Do stuff here  
     UPDATE [dbo].[ITP_USERS_ST]  
        SET  [AppID] =ISNULL(@AppID, AppID)  
        , [AppRoleID] = ISNULL(@AppRoleID,AppRoleID)  
        , [ModifyDate] = GETDATE()  
        , [FirstName] = ISNULL(@FirstName,FirstName)  
        , [MiddleName] = ISNULL(@MiddleName,MiddleName)  
        , [LastName] = ISNULL(@LastName,LastName)  
        , [PasswordHash] = ISNULL(@PasswordHash,PasswordHash)  
        , [PasswordSalt] = ISNULL(@PasswordSalt,PasswordSalt)  
        , [PhoneNumber] = ISNULL(@PhoneNumber,PhoneNumber)          
        , ExpirePassword = ISNULL(@ExpirePassword,ExpirePassword)  
        , IsTemp = ISNULL(@IsTemp,IsTemp)  
        , IsDeleted = ISNULL(@IsDeleted,IsDeleted)  
        , IsAdmin = ISNULL(@IsAdmin,IsAdmin)  
        , IsCustomer = ISNULL(@IsCustomer,IsCustomer)  
        , EmailConfirmed = ISNULL(@EmailConfirmed,EmailConfirmed)  
        , LockoutEnabled = ISNULL(@LockoutEnabled,LockoutEnabled)  
        , AccessFailedCount = ISNULL(@AccessFailedCount,AccessFailedCount)  
        , IsActive = ISNULL(@IsActive,IsActive)  
        , PreviousPassword = ISNULL(@PreviousPassword,PreviousPassword)  
        , IsForgetPassword = ISNULL(@IsForgetPassword,IsForgetPassword)  
        -- New Change - Fazi  
        , UserPhoto = ISNULL(@UserPhoto,UserPhoto)  
      WHERE EMAIL=@UserEmail and UserID = @UserID  
  
     SELECT @@ROWCOUNT;   
  
  IF @TranCounter = 0 AND @@TRANCOUNT >= 1   
   COMMIT  
  
  RETURN 0  
  
 END TRY  
  
 BEGIN CATCH      
  DECLARE @errorNumber INT = ERROR_NUMBER(),  
    @errorProcedure NVARCHAR(4000) = ERROR_PROCEDURE(),  
    @errorLine INT = ERROR_LINE(),  
    @errorMessage NVARCHAR(4000) = ERROR_MESSAGE(),  
    @errorSeverity INT = ERROR_SEVERITY(),  
    @errorState INT = ERROR_STATE(),  
    @rolledBack NVARCHAR(5) = 'False';  
       
  IF @TranCounter = 0 AND @@TRANCOUNT >= 1  
            -- Transaction started in procedure. Roll back complete transaction.  
            ROLLBACK TRANSACTION;  
        ELSE  
            -- Transaction started before procedure called, do not roll back modifications made before the procedure was called.  
            IF XACT_STATE() = 1 AND @@TRANCOUNT >= 1  
                -- If the transaction is still valid, just roll back to the savepoint set at the start of the stored procedure.  
                ROLLBACK TRANSACTION @transactionName;  
            
  RAISERROR   
  (  
   @errorMessage,   
   @errorSeverity,   
   @errorState,                 
   @errorNumber,    -- parameter: original error number.  
   @errorSeverity,  -- parameter: original error severity.  
   @errorState,     -- parameter: original error state.  
   @errorProcedure, -- parameter: original error procedure name.  
   @errorLine,      -- parameter: original error line number.  
   @rolledBack   -- parameter: indicates if transactions have been rolled back  
  );  
    
 END CATCH   
  
END  