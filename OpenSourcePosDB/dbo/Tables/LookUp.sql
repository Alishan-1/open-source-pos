CREATE TABLE [dbo].[LookUp] (
    [LookUpID]     BIGINT         NOT NULL,
    [UserID]       BIGINT         NULL,
    [Category]     NVARCHAR (10)  NULL,
    [SubCategory]  NVARCHAR (10)  NULL,
    [OptionOrders] INT            NULL,
    [Description]  NVARCHAR (100) NULL,
    [Screen]       NVARCHAR (10)  NULL,
    [Value]        NVARCHAR (35)  NULL,
    [IsValid]      BIT            NULL,
    [CreateDate]   DATETIME       NULL,
    [CreateID]     BIGINT         NULL,
    [ModifyDate]   DATETIME       NULL
);

