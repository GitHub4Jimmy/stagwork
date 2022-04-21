CREATE TABLE [dbo].[OIP_TYPE] (
    [OIP_TYPE_ID]              NUMERIC (10)  NOT NULL,
    [OIP_TYPE]                 VARCHAR (25)  NOT NULL,
    [INTERNAL_NAME]            VARCHAR (50)  NULL,
    [DESCRIPTION]              VARCHAR (50)  NULL,
    [INSERTED_BY]              VARCHAR (50)  NULL,
    [INSERTED_DATE]            DATETIME2 (7) NULL,
    [UPDATED_BY]               VARCHAR (50)  NULL,
    [UPDATED_DATE]             DATETIME2 (7) NULL,
    [DEFAULT_FLAG]             CHAR (1)      NOT NULL,
    [CONFERENCE_LOCATION_FLAG] CHAR (1)      NOT NULL,
    [SEQ_NO]                   NUMERIC (5)   NULL,
    [IMPORT_DATE]              DATETIME      NOT NULL,
    CONSTRAINT [PK_OIP_TYPE] PRIMARY KEY CLUSTERED ([OIP_TYPE_ID] ASC)
);

