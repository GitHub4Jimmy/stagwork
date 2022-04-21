CREATE TABLE [dbo].[WORK_LOCATION] (
    [WORK_LOCATION_ID]              NUMERIC (10)                                                    NOT NULL,
    [WORK_LOCATION_CODE]            VARCHAR (25)                                                    NULL,
    [ALT_IDENTIFIER]                VARCHAR (25)                                                    NULL,
    [ALT_IDENTIFIER_2]              VARCHAR (25)                                                    NULL,
    [ALT_IDENTIFIER_3]              VARCHAR (25)                                                    NULL,
    [TENANT_NAME]                   VARCHAR (250)                                                   NULL,
    [WORK_LOCATION_DISTRICT]        VARCHAR (2)                                                     NULL,
    [WORK_LOCATION_GROUP]           VARCHAR (2)                                                     NULL,
    [WORK_LOCATION_GROUP_SUB_GROUP] VARCHAR (2)                                                     NULL,
    [WORK_LOCATION_NAME]            VARCHAR (250)                                                   NULL,
    [WORK_LOCATION_TYPE]            VARCHAR (2)                                                     NULL,
    [PARENT_WORK_LOCATION_ID]       NUMERIC (10)                                                    NULL,
    [UPDATED_DATE]                  DATETIME2 (7)                                                   NULL,
    [INSERTED_DATE]                 DATETIME2 (7)                                                   NOT NULL,
    CONSTRAINT [PK_WORK_LOCATION] PRIMARY KEY CLUSTERED ([WORK_LOCATION_ID] ASC),
    CONSTRAINT [FK_WORK_LOCATION_PARENT_WORK_LOCATION_ID] FOREIGN KEY ([PARENT_WORK_LOCATION_ID]) REFERENCES [dbo].[WORK_LOCATION] ([WORK_LOCATION_ID]) NOT FOR REPLICATION
);


GO
ALTER TABLE [dbo].[WORK_LOCATION] NOCHECK CONSTRAINT [FK_WORK_LOCATION_PARENT_WORK_LOCATION_ID];

