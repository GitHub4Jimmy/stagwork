﻿CREATE TABLE [delta].[MEMBER] (
	[DELTA_GID] UNIQUEIDENTIFIER DEFAULT NEWID() NOT NULL, 
    [MEMBER_ID]          [varchar](250) NOT NULL,
    [PERSON_ID]          [varchar](250)  NOT NULL,
    [ELIG_ACTIVITY_DATE] DATETIME2 (7) NULL,
    [FREEZE_WORK]        CHAR (1)      NULL,
    [INSERTED_BY]        VARCHAR (50)  NULL,
    [INSERTED_DATE]      DATETIME2 (7) NULL,
    [UPDATED_BY]         VARCHAR (50)  NULL,
    [UPDATED_DATE]       DATETIME2 (7) NULL,
    [IMPORT_DATE]        DATETIME      NULL,
	[DELTA_ACTION] [varchar](250) DEFAULT 'UPDATE',
	[DELTA_SENT] [datetime2] NULL,
    CONSTRAINT [PK_MEMBER] PRIMARY KEY CLUSTERED ([DELTA_GID] ASC)
);

