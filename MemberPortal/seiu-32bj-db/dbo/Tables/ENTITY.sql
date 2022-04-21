CREATE TABLE [dbo].[ENTITY] (
    [ENTITY_ID]                   NUMERIC (10)   NOT NULL,
    [ENTITY_NAME]                 VARCHAR (50)   NULL,
    [QUERY_VISIBLE]               CHAR (1)       NOT NULL,
    [DISPLAY_NAME]                VARCHAR (50)   NULL,
    [TABLE_ENTITY]                CHAR (1)       NOT NULL,
    [BYPASS_DERIVED]              CHAR (1)       NOT NULL,
    [ACCOUNT_ENTITY]              CHAR (1)       NOT NULL,
    [ADDRESS_ENTITY]              CHAR (1)       NOT NULL,
    [MODULE_ENTITY]               CHAR (1)       NOT NULL,
    [RECORD_ENTITY]               CHAR (1)       NOT NULL,
    [NOTES]                       VARCHAR (250)  NULL,
    [INSERTED_BY]                 VARCHAR (50)   NOT NULL,
    [INSERTED_DATE]               DATETIME2 (7)  NOT NULL,
    [UPDATED_BY]                  VARCHAR (50)   NULL,
    [UPDATED_DATE]                DATETIME2 (7)  NULL,
    [AUDIT_HISTORY_SEARCH_SQL]    VARCHAR (2000) NULL,
    [ALLOW_HISTORY]               CHAR (1)       NOT NULL,
    [HISTORY_ON_INSERT]           CHAR (1)       NOT NULL,
    [HISTORY_ON_DELETE]           CHAR (1)       NOT NULL,
    [IDENTIFIER_BUSINESS_RULE_ID] FLOAT (53)     NULL,
    [AUDIT_TRAIL_ENTITY_ID]       NUMERIC (10)   NULL,
    [HISTORY_ON_UPDATE]           VARCHAR (1)    NOT NULL,
    [TABLESPACE_NAME]             VARCHAR (50)   NULL,
    [DOCUMENT_TYPE_ENTITY]        CHAR (1)       NULL,
    [TABLE_TYPE]                  VARCHAR (1)    NULL,
    [TRACK_CHANGES_FOR_ELIG_CALC] VARCHAR (1)    NOT NULL,
    CONSTRAINT [PK_ENTITY] PRIMARY KEY CLUSTERED ([ENTITY_ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ENTITY_ENTITY_ID]
    ON [dbo].[ENTITY]([ENTITY_ID] ASC);

