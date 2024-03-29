﻿CREATE TABLE [dbo].[Users] (
    [Email]                VARCHAR (50)       NOT NULL,
    [CompanyID]           INT                NOT NULL DEFAULT 1,
    [UserID]               BIGINT             IDENTITY (1, 1) NOT NULL,
    [AppID]                BIGINT             NULL,
    [AppRoleID]            BIGINT             NULL,
    [FirstName]            VARCHAR (15)       NOT NULL,
    [MiddleName]           VARCHAR (1)        NULL,
    [LastName]             VARCHAR (15)       NOT NULL,
    [PhoneNumber]          VARCHAR (15)       NULL,
    [StartDate]            DATETIME           NULL,
    [EndDate]              DATETIME           NULL,
    [IsActive]             BIT                NULL,
    [IsSpecialOffer]       BIT                NULL,
    [IsCustomer]           BIT                NULL,
    [ModifyDate]           DATETIME           NULL,
    [AccessFailedCount]    INT                NULL,
    [ConcurrencyStamp]     NVARCHAR (MAX)     NULL,
    [EmailConfirmed]       BIT                NULL,
    [LockoutEnabled]       BIT                NULL,
    [LockoutEnd]           DATETIMEOFFSET (7) NULL,
    [PasswordHash]         NVARCHAR (MAX)     NULL,
    [PhoneNumberConfirmed] BIT                NULL,
    [SecurityStamp]        NVARCHAR (MAX)     NULL,
    [TwoFactorEnabled]     BIT                NULL,
    [IsAdmin]              BIT                NULL,
    [DataEventRecordsRole] NVARCHAR (256)     NULL,
    [SecuredFilesRole]     NVARCHAR (256)     NULL,
    [IsDeleted]            BIT                NULL,
    [Primary]              BIT                NULL,
    [Q1Id]                 INT                NULL,
    [Q1Answer]             NVARCHAR (100)     NULL,
    [Q2Id]                 INT                NULL,
    [Q2Answer]             NVARCHAR (100)     NULL,
    [Q3Id]                 INT                NULL,
    [Q3Answer]             NVARCHAR (100)     NULL,
    [PasswordSalt]         VARCHAR (MAX)      NULL,
    [IsTemp]               BIT                NULL,
    [UserPhoto]            VARCHAR (100)      NULL,
    [PreviousPassword]     NVARCHAR (MAX)     NULL,
    [ExpirePassword]       DATETIME           NULL,
    [IsForgetPassword]     BIT                NULL, 
    [CreateDate] DATETIME NULL, 
    [CreateUser] INT NULL, 
    [BranchID] INT NULL, 
    CONSTRAINT [PK_ITP_USERS_ST] PRIMARY KEY ([UserID]), 
    CONSTRAINT [AK_ITP_USERS_ST_Email] UNIQUE ([Email])
);

