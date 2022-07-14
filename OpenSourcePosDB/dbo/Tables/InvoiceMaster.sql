﻿CREATE TABLE [dbo].[InvoiceMaster] (
    [InvoiceNo]       INT             NOT NULL,
    [InvoiceDate]     DATE            NOT NULL,
    [CustomerID]      INT             NULL,
    [CreateUser]      INT             NULL,
    [CreateDate]      DATETIME        NULL,
    [UpdateUser]      INT             NULL,
    [UpdateDate]      DATETIME        NULL,
    [CompanyID]       INT             NOT NULL,
    [ModuleID]        VARCHAR (3)     NULL,
    [TotalAmount]     DECIMAL (15, 2) NULL,
    [DiscountPercent] DECIMAL (10, 2) NULL,
    [DiscountAmount]  DECIMAL (10, 2) NULL,
    [SaleTaxPercent]  DECIMAL (10, 2) NULL,
    [SaleTaxAmount]   DECIMAL (15, 2) NULL,
    [NetAmount]       DECIMAL (15, 2) NULL,
    [ReceivedAmount]  DECIMAL (18, 2) NULL,
    [BalanceAmount]   DECIMAL (18, 2) NULL,
    [InvoiceType]     VARCHAR (3)     NULL,
    [FiscalYearID]    INT             NULL,
    [OtherTaxPercent] DECIMAL (6, 2)  NULL,
    [OtherTaxAmount]  DECIMAL (10, 2) NULL,
    [CreditLimit]     NUMERIC (18, 2) NULL,
    [ConsumedCredit]  NUMERIC (18, 2) NULL,
    [BalanceCredit]   NUMERIC (18, 2) NULL,
    [Status]          VARCHAR (15)    NULL,
    [BranchID]        INT             NULL,
    [Remarks]         VARCHAR (255)   NULL
);
