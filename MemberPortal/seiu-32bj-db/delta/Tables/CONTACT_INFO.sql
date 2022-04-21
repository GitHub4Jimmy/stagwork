CREATE TABLE [delta].[CONTACT_INFO] (
	[DELTA_GID]				   UNIQUEIDENTIFIER DEFAULT NEWID() NOT NULL, 
    [CONTACT_INFO_ID]          [varchar](250) NOT NULL,
    [ENTITY_ID]                NUMERIC (10)                                                    NULL,
    [KEY_VALUE]                [varchar](250)                                                  NOT NULL,
    [VAL_SEQ_NO]               NUMERIC (9, 4)                                                  NULL,
    [NVAL_01]                  NUMERIC (10)                                                    NULL,
    [NVAL_02]                  NUMERIC (10)                                                    NULL,
    [NVAL_03]                  NUMERIC (10)                                                    NULL,
    [CVAL_01]                  VARCHAR (50)                                                    NULL,
    [CVAL_02]                  VARCHAR (50)                                                    NULL,
    [CVAL_03]                  VARCHAR (250)                                                   NULL,
    [PRIMARY_FLAG]             VARCHAR (1)                                                     NULL,
    [INSERTED_BY]              VARCHAR (50)                                                    NULL,
    [INSERTED_DATE]            DATETIME2 (7)                                                   NULL,
    [UPDATED_BY]               VARCHAR (50)                                                    NULL,
    [UPDATED_DATE]             DATETIME2 (7)                                                   NULL,
    [CONTACT_INFO_SUBTYPE_ID]  NUMERIC (10)                                                    NULL,
    [CONTACT_SOURCE]           VARCHAR (2)                                                     NULL,
    [SECURITYUSER_ACTIVITY_ID] NUMERIC (10)                                                    NULL,
    [IMPORT_DATE]              DATETIME                                                        NULL,
	[DELTA_ACTION]			   [varchar](250) DEFAULT 'UPDATE',
	[DELTA_SENT]			   [datetime2] NULL,
    CONSTRAINT [PK_CONTACT_INFO] PRIMARY KEY CLUSTERED ([DELTA_GID] ASC),
);
GO
CREATE NONCLUSTERED INDEX [UNiQUE_IND]
    ON [delta].[CONTACT_INFO]([CONTACT_INFO_SUBTYPE_ID] ASC, [ENTITY_ID] ASC, [KEY_VALUE] ASC, [VAL_SEQ_NO] ASC);

