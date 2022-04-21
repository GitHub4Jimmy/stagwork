CREATE TABLE [dbo].[ADDRESSES] (
    [KEY_VALUE]      NUMERIC (10)                                                    NOT NULL,
    [ENTITY_ID]      NUMERIC (10)                                                    NOT NULL,
    [ADDRESS_ID]     NUMERIC (10)                                                    NOT NULL,
    [ADDRESS_SOURCE] VARCHAR (2)                                                     NULL,
    [ADDRESS_TYPE]   VARCHAR (2)                                                     NOT NULL,
    [ADDRESS_1]      VARCHAR (250)                                                   NULL,
    [ADDRESS_2]      VARCHAR (250)                                                   NULL,
    [ADDRESS_3]      VARCHAR (250)                                                   NULL,
    [CITY]           VARCHAR (50)                                                    NULL,
    [COUNTRY]        VARCHAR (50)                                                    NULL,
    [COUNTY]         VARCHAR (10)                                                    NULL,
    [LATITUDE]       NUMERIC (13, 8)                                                 NULL,
    [LONGITUDE]      NUMERIC (13, 8)                                                 NULL,
    [POSTAL_CODE]    VARCHAR (10)                                                    NULL,
    [STATE]          VARCHAR (10)                                                    NULL,
    [ZIP]            NUMERIC (9)                                                     NULL,
    [START_DATE]     DATETIME2 (7)                                                   NOT NULL,
    [STOP_DATE]      DATETIME2 (7)                                                   NULL,
    [INSERTED_DATE]  DATETIME2 (7)                                                   NOT NULL,
    [UPDATED_DATE]   DATETIME2 (7)                                                   NULL,
    [IMPORT_DATE]    DATETIME                                                        NOT NULL,
    CONSTRAINT [PK_ADDRESSES] PRIMARY KEY CLUSTERED ([ADDRESS_ID] ASC),
    CONSTRAINT [FK_ADDRESSES_ENTITY_ID] FOREIGN KEY ([ENTITY_ID]) REFERENCES [dbo].[ENTITY] ([ENTITY_ID])
);


GO
ALTER TABLE [dbo].[ADDRESSES] NOCHECK CONSTRAINT [FK_ADDRESSES_ENTITY_ID];


GO
CREATE NONCLUSTERED INDEX [IX_ADDRESSES_KEY_VALUE]
    ON [dbo].[ADDRESSES]([KEY_VALUE] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ADDRESSES_ENTITY_ID]
    ON [dbo].[ADDRESSES]([ENTITY_ID] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ADDRESSES_ADDRES_ID]
    ON [dbo].[ADDRESSES]([ADDRESS_ID] ASC);

