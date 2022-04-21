CREATE TABLE [dbo].[PERSON] (
    [ALT_IDENTIFIER_4]      VARCHAR (10)                                                        NULL,
    [BIC_CODE]              VARCHAR (25)                                                        NULL,
    [BIRTH_DATE]            DATETIME2 (7)                 NULL,
    [DEATH_DATE]            DATETIME2 (7)                                                       NULL,
    [EMPLOYMENT_STATUS]     VARCHAR (2)                                                         NULL,
    [FIRST_NAME]            VARCHAR (50)     NULL,
    [INSERTED_DATE]         DATETIME2 (7)                                                       NOT NULL,
    [LAST_NAME]             VARCHAR (50)    NULL,
    [MIDDLE_NAME]           VARCHAR (50)   NULL,
    [PERSON_ID]             NUMERIC (10)                                                        NOT NULL,
    [PRIMARY_LANGUAGE_CODE] VARCHAR (2)                                                         NULL,
    [SEX]                   CHAR (1)                        NULL,
    [SORT_NAME]             VARCHAR (250)     NULL,
    [SSN]                   NUMERIC (9) NULL,
    [TYPE]                  VARCHAR (2)                                                         NULL,
    [SUFFIX]                VARCHAR (2)                                                         NULL,
    [OIP_TYPE_ID]           NUMERIC (10)                                                        NULL,
    [UPDATED_DATE]          DATETIME2 (7)                                                       NULL,
    [IMPORT_DATE]           DATETIME                                                            NOT NULL,
	[MARITAL_STATUS]        VARCHAR (2)                                                         NULL,
    [MARRIAGE_DATE]         DATETIME2 (7)                                                       NULL,
    [DIVORCE_DATE]          DATETIME2 (7)                                                       NULL,
    [INSERTED_BY]           VARCHAR (50)                                                        NULL,
    [UPDATED_BY]            VARCHAR (50)                                                        NULL,
    CONSTRAINT [PK_PERSON] PRIMARY KEY CLUSTERED ([PERSON_ID] ASC),
    CONSTRAINT [FK_PERSON_OIP_TYPE_ID] FOREIGN KEY ([OIP_TYPE_ID]) REFERENCES [dbo].[OIP_TYPE] ([OIP_TYPE_ID]) NOT FOR REPLICATION
);


GO
ALTER TABLE [dbo].[PERSON] NOCHECK CONSTRAINT [FK_PERSON_OIP_TYPE_ID];

