-- =============================================  
-- Author:      Ali Shan  
-- Create date: 20-Nov-2018  
-- Description: Used for loging out user and update UserLog and end current session  
-- =============================================  
CREATE  PROCEDURE [dbo].[usp_Users_LogOutUser]   
 @UserEmail VARCHAR(100), @UserID bigint,   
 @SessionToken VARCHAR(MAX),   
 @SessionEndDateTime DatetimeOffset(2)  
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
     UPDATE [dbo].[UserLog]  
        SET  SessEnd = @SessionEndDateTime,  
       RememberUser = 0  
      WHERE SessionToken = @SessionToken and UserID = @UserID  
  
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
  