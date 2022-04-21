CREATE TABLE [dbo].[CONTACT_INFO] (
    [CONTACT_INFO_ID]          NUMERIC (10)                                                    NOT NULL,
    [ENTITY_ID]                NUMERIC (10)                                                    NOT NULL,
    [KEY_VALUE]                NUMERIC (10)                                                    NOT NULL,
    [VAL_SEQ_NO]               NUMERIC (9, 4)                                                  NOT NULL,
    [NVAL_01]                  NUMERIC (10)                                                    NULL,
    [NVAL_02]                  NUMERIC (10)                                                    NULL,
    [NVAL_03]                  NUMERIC (10)                                                    NULL,
    [CVAL_01]                  VARCHAR (50)                                                     NULL,
    [CVAL_02]                  VARCHAR (50)                                                     NULL,
    [CVAL_03]                  VARCHAR (250)                                                    NULL,
    [PRIMARY_FLAG]             VARCHAR (1)                                                     NOT NULL,
    [INSERTED_BY]              VARCHAR (50)                                                    NOT NULL,
    [INSERTED_DATE]            DATETIME2 (7)                                                   NOT NULL,
    [UPDATED_BY]               VARCHAR (50)                                                    NULL,
    [UPDATED_DATE]             DATETIME2 (7)                                                   NULL,
    [CONTACT_INFO_SUBTYPE_ID]  NUMERIC (10)                                                    NOT NULL,
    [CONTACT_SOURCE]           VARCHAR (2)                                                     NULL,
    [SECURITYUSER_ACTIVITY_ID] NUMERIC (10)                                                    NULL,
    [IMPORT_DATE]              DATETIME                                                        NOT NULL,
    CONSTRAINT [PK_CONTACT_INFO] PRIMARY KEY CLUSTERED ([CONTACT_INFO_ID] ASC),
    CONSTRAINT [FK_CONTACT_INFO_CONTACT_INFO_SUBTYPE_ID] FOREIGN KEY ([CONTACT_INFO_SUBTYPE_ID]) REFERENCES [dbo].[CONTACT_INFO_SUBTYPE] ([CONTACT_INFO_SUBTYPE_ID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_CONTACT_INFO_ENTITY_ID] FOREIGN KEY ([ENTITY_ID]) REFERENCES [dbo].[ENTITY] ([ENTITY_ID]) NOT FOR REPLICATION
);


GO
ALTER TABLE [dbo].[CONTACT_INFO] NOCHECK CONSTRAINT [FK_CONTACT_INFO_CONTACT_INFO_SUBTYPE_ID];


GO
ALTER TABLE [dbo].[CONTACT_INFO] NOCHECK CONSTRAINT [FK_CONTACT_INFO_ENTITY_ID];


GO
CREATE NONCLUSTERED INDEX [UNiQUE_IND]
    ON [dbo].[CONTACT_INFO]([CONTACT_INFO_SUBTYPE_ID] ASC, [ENTITY_ID] ASC, [KEY_VALUE] ASC, [VAL_SEQ_NO] ASC);

