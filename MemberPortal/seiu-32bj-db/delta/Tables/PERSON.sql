﻿CREATE TABLE [delta].[PERSON](
	[DELTA_GID] UNIQUEIDENTIFIER DEFAULT NEWID() NOT NULL, 
	[ALT_IDENTIFIER_4] [varchar](10) NULL,
	[BIC_CODE] [varchar](25) NULL,
	[BIRTH_DATE] [datetime2](7) NULL,
	[DEATH_DATE] [datetime2](7) NULL,
	[EMPLOYMENT_STATUS] [varchar](2) NULL,
	[FIRST_NAME] [varchar](50) NULL,
	[INSERTED_DATE] [datetime2](7) NULL,
	[LAST_NAME] [varchar](50) NULL,
	[MIDDLE_NAME] [varchar](50) NULL,
	[PERSON_ID] [varchar](250) NOT NULL,
	[PRIMARY_LANGUAGE_CODE] [varchar](2) NULL,
	[SEX] [char](1) NULL,
	[SORT_NAME] [varchar](250) NULL,
	[SSN] [numeric](9, 0) NULL,
	[TYPE] [varchar](2) NULL,
	[SUFFIX] [varchar](2) NULL,
	[OIP_TYPE_ID] [numeric](10, 0) NULL,
	[UPDATED_DATE] [datetime2](7) NULL,
	[IMPORT_DATE] [datetime] NULL,
	[MARITAL_STATUS] [varchar](2) NULL,
	[MARRIAGE_DATE] [datetime2](7) NULL,
	[DIVORCE_DATE] [datetime2](7) NULL,
	[INSERTED_BY] [varchar](50) NULL,
	[UPDATED_BY] [varchar](50) NULL,
	[DELTA_ACTION] [varchar](250) DEFAULT 'UPDATE',
	[DELTA_SENT] [datetime2] NULL,
    CONSTRAINT [PK_PERSON] PRIMARY KEY CLUSTERED 
(
	[DELTA_GID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [delta].[PERSON]  WITH NOCHECK ADD  CONSTRAINT [FK_PERSON_OIP_TYPE_ID] FOREIGN KEY([OIP_TYPE_ID])
REFERENCES [dbo].[OIP_TYPE] ([OIP_TYPE_ID])
GO

ALTER TABLE [delta].[PERSON] NOCHECK CONSTRAINT [FK_PERSON_OIP_TYPE_ID]
GO