CREATE TABLE [dbo].[DATASHEET_VALUE] (
    [ATTRIBUTE_ID]  NUMERIC (10)   NOT NULL,
    [DATASHEET_ID]  NUMERIC (10)   NOT NULL,
    [DS_VALUE_ID]   NUMERIC (10)   NOT NULL,
    [NUM_KEY_VALUE] NUMERIC (10)   NOT NULL,
    [KEY_VALUE]     VARCHAR (50)   NOT NULL,
    [OWNER_ID]      NUMERIC (10)   NOT NULL,
    [SEQ_NO]        NUMERIC (10)   NULL,
    [VALUE]         VARCHAR (4000) NULL,
    [UPDATED_DATE]  DATETIME2 (7)  NULL,
    [INSERTED_DATE] DATETIME2 (7)  NOT NULL,
    [IMPORT_DATE]   DATETIME       NULL,
    [INSERTED_BY]   VARCHAR (50)   NULL,
    [UPDATED_BY]    VARCHAR (50)   NULL,
    CONSTRAINT [PK_DATASHEET_VALUE] PRIMARY KEY NONCLUSTERED ([DS_VALUE_ID] ASC),
    CONSTRAINT [FK_DATASHEET_VALUE_ATTRIBUTE_ID] FOREIGN KEY ([ATTRIBUTE_ID]) REFERENCES [dbo].[ATTRIBUTE] ([ATTRIBUTE_ID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_DATASHEET_VALUE_DATASHEET_ID] FOREIGN KEY ([DATASHEET_ID]) REFERENCES [dbo].[DATASHEET_DEFINITION] ([DATASHEET_ID]) NOT FOR REPLICATION
);


GO
ALTER TABLE [dbo].[DATASHEET_VALUE] NOCHECK CONSTRAINT [FK_DATASHEET_VALUE_ATTRIBUTE_ID];


GO
ALTER TABLE [dbo].[DATASHEET_VALUE] NOCHECK CONSTRAINT [FK_DATASHEET_VALUE_DATASHEET_ID];


GO
CREATE NONCLUSTERED INDEX [IX_DATASHEET_VALUE_KEY_VALUE]
    ON [dbo].[DATASHEET_VALUE]([KEY_VALUE] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DATASHEET_VALUE_DATASHEET_ID_KEY_VALUE_SEQ_NO]
    ON [dbo].[DATASHEET_VALUE]([DATASHEET_ID] ASC, [KEY_VALUE] ASC, [SEQ_NO] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DATASHEET_VALUE_DATASHEET_ID]
    ON [dbo].[DATASHEET_VALUE]([DATASHEET_ID] ASC);


GO
CREATE CLUSTERED INDEX [IX_DATASHEET_VALUE_ATTRIBUTE_ID_DATASHEET_ID]
    ON [dbo].[DATASHEET_VALUE]([ATTRIBUTE_ID] ASC, [DATASHEET_ID] ASC);

