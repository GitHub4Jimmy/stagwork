CREATE TABLE [dbo].[BARG_UNIT] (
    [BARG_UNIT_ID]     NUMERIC (10)                                                    NOT NULL,
    [BARG_UNIT_NAME]   VARCHAR (100)  NULL,
    [BARG_UNIT_TYPE]   VARCHAR (2)                                                     NULL,
    [BARG_UNIT_CODE]   VARCHAR (25)                                                    NOT NULL,
    [REF_BARG_UNIT_ID] NUMERIC (10)                                                    NULL,
    [INSERTED_BY]      VARCHAR (50)                                                    NOT NULL,
    [INSERTED_DATE]    DATETIME2 (7)                                                   NOT NULL,
    CONSTRAINT [PK_BARG_UNIT] PRIMARY KEY CLUSTERED ([BARG_UNIT_ID] ASC),
    CONSTRAINT [FK_BARG_UNIT_REF_BARG_UNIT_ID] FOREIGN KEY ([REF_BARG_UNIT_ID]) REFERENCES [dbo].[BARG_UNIT] ([BARG_UNIT_ID]) NOT FOR REPLICATION
);


GO
ALTER TABLE [dbo].[BARG_UNIT] NOCHECK CONSTRAINT [FK_BARG_UNIT_REF_BARG_UNIT_ID];

