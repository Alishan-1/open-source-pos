﻿CREATE TABLE [dbo].[ITP_USERS_ST] (
    [LOGIN_ID]             VARCHAR (7)        NOT NULL,
    [USER_NAME]            VARCHAR (50)       NOT NULL,
    [PASSWORD]             VARCHAR (50)       NULL,
    [EMP_CODE]             INT                NULL,
    [EMAIL]                VARCHAR (50)       NULL,
    [DAYLIMIT]             INT                NULL,
    [FAQ1]                 VARCHAR (50)       NULL,
    [ANS1]                 VARCHAR (50)       NULL,
    [FAQ2]                 VARCHAR (50)       NULL,
    [ANS2]                 VARCHAR (50)       NULL,
    [USER_STATUS]          VARCHAR (1)        NOT NULL,
    [USER_LOGIN_STATUS]    VARCHAR (1)        NULL,
    [USER_LOGIN_DATE]      DATE               NULL,
    [CREATE_USER]          INT                NULL,
    [CREATE_DATE]          DATETIME           NULL,
    [COMPANY_ID]           INT                NOT NULL,
    [EPASSWORD]            VARCHAR (50)       NULL,
    [CELL]                 VARCHAR (50)       NULL,
    [PYear]                INT                NULL,
    [BRANCH_ID]            INT                NULL,
    [UserID]               BIGINT             IDENTITY (1, 1) NOT NULL,
    [AppID]                BIGINT             NULL,
    [AppRoleID]            BIGINT             NULL,
    [FirstName]            VARCHAR (15)       NULL,
    [MiddleName]           VARCHAR (1)        NULL,
    [LastName]             VARCHAR (15)       NULL,
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
    [IS_SYSTEM_ADMIN]      BIT                NULL
);
