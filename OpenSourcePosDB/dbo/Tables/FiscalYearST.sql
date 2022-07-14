CREATE TABLE [dbo].[FiscalYearST] (
    [FiscalYearID]        INT      NULL,
    [FiscalYearFrom] DATE     NULL,
	[FiscalYearTo]   DATE     NULL,
    [IsCurrentYear]     CHAR (1) NULL,
    [CreateUser]      INT      NULL,
    [CreateDate]      DATETIME NULL,
    [UpdateUser]      INT      NULL,
    [UpdateDate]      DATETIME NULL,
    [CompanyID]       INT      NULL
);

