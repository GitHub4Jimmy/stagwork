CREATE TABLE [dbo].[BILLING_ENTITY] (
    [BILLING_ID]                 NUMERIC (10)                                                    NOT NULL,
    [BILLING_ENTITY_NAME]        VARCHAR (250) NULL,
    [WORK_LOCATION_ID]           NUMERIC (10)                                                    NULL,
    [EMPLOYER_ID]                NUMERIC (10)                                                    NOT NULL,
    [REF_BILLING_ID]             NUMERIC (10)                                                    NULL,
    [BILLING_ENTITY_CODE]        VARCHAR (25)                                                    NOT NULL,
    [ENABLE_SELF_SERVICE_ACCESS] VARCHAR (2)                                                     NOT NULL,
    [ALT_IDENTIFIER]             VARCHAR (250)                                                   NULL,
    [IMPORT_DATE]                DATETIME                                                        NOT NULL,
    CONSTRAINT [PK_BILLING_ENTITY] PRIMARY KEY CLUSTERED ([BILLING_ID] ASC),
    CONSTRAINT [FK_BILLING_ENTITY_EMPLOYER_ID] FOREIGN KEY ([EMPLOYER_ID]) REFERENCES [dbo].[EMPLOYER] ([EMPLOYER_ID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_BILLING_ENTITY_REF_BILLING_ID] FOREIGN KEY ([REF_BILLING_ID]) REFERENCES [dbo].[BILLING_ENTITY] ([BILLING_ID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_BILLING_ENTITY_WORK_LOCATION_ID] FOREIGN KEY ([WORK_LOCATION_ID]) REFERENCES [dbo].[WORK_LOCATION] ([WORK_LOCATION_ID]) ON DELETE CASCADE NOT FOR REPLICATION
);


GO
ALTER TABLE [dbo].[BILLING_ENTITY] NOCHECK CONSTRAINT [FK_BILLING_ENTITY_EMPLOYER_ID];


GO
ALTER TABLE [dbo].[BILLING_ENTITY] NOCHECK CONSTRAINT [FK_BILLING_ENTITY_REF_BILLING_ID];

