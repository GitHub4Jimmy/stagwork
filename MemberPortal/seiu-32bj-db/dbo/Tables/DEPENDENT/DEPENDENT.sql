CREATE TABLE [dbo].[DEPENDENT] (
    [PERSON_ID]            NUMERIC (10)                                  NOT NULL,
    [DEPENDENT_ID]         NUMERIC (10)                                  NOT NULL,
    [QMCSO_FLAG]           CHAR (1)                                      NOT NULL,
    [QMCSO_FROM_DATE]      DATETIME2 (7)                                 NULL,
    [RELATION]             VARCHAR (2)                                   NULL,
    [PRIMARY_DEPENDENT_ID] NUMERIC (10)                                  NOT NULL,
    [DISABLED]             CHAR (1)										 NULL,
    [INSERTED_BY]          VARCHAR (50)                                  NOT NULL,
    [INSERTED_DATE]        DATETIME2 (7)                                 NOT NULL,
    [UPDATED_BY]           VARCHAR (50)                                  NULL,
    [UPDATED_DATE]         DATETIME2 (7)                                 NULL,
    [SEQ_NO]               NUMERIC (5)                                   NULL,
    [QMCSO_UNTIL_DATE]     DATETIME2 (7)                                 NULL,
    [MARRIAGE_DATE]        DATETIME2 (7)                                 NULL,
    [DIVORCE_DATE]         DATETIME2 (7)                                 NULL,
    [SEPARATION_DATE]      DATETIME2 (7)                                 NULL,
    [IMPORT_DATE]          DATETIME                                      NOT NULL,
    CONSTRAINT [PK_DEPENDENT] PRIMARY KEY CLUSTERED ([PRIMARY_DEPENDENT_ID] ASC),
    CONSTRAINT [FK_DEPENDENT_DEPENDENT_ID] FOREIGN KEY ([DEPENDENT_ID]) REFERENCES [dbo].[PERSON] ([PERSON_ID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_DEPENDENT_PERSON_ID] FOREIGN KEY ([PERSON_ID]) REFERENCES [dbo].[PERSON] ([PERSON_ID]) NOT FOR REPLICATION
);


GO
ALTER TABLE [dbo].[DEPENDENT] NOCHECK CONSTRAINT [FK_DEPENDENT_DEPENDENT_ID];


GO
ALTER TABLE [dbo].[DEPENDENT] NOCHECK CONSTRAINT [FK_DEPENDENT_PERSON_ID];


