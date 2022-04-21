CREATE TABLE [dbo].[EMPLOYER] (
    [EMPLOYER_ID]                    NUMERIC (10)                                                    NOT NULL,
    [EMPLOYER_NAME]                  VARCHAR (250)													 NULL,
    [EMPLOYER_CODE]                  VARCHAR (10)                                                    NOT NULL,
    [SIC_CODE_1]                     VARCHAR (10)                                                    NULL,
    [SIC_CODE_2]                     VARCHAR (10)                                                    NULL,
    [INSERTED_DATE]                  DATETIME2 (7)                                                   NOT NULL,
    [INSERTED_BY]                    VARCHAR (50)                                                    NOT NULL,
    [UPDATED_DATE]                   DATETIME2 (7)                                                   NULL,
    [UPDATED_BY]                     VARCHAR (50)                                                    NULL,
    [EMPLOYER_ALTERNATE_STATUS_CODE] VARCHAR (2)                                                     NULL,
    [IMPORT_DATE]                    DATETIME                                                        NOT NULL,
    CONSTRAINT [PK_EMPLOYER] PRIMARY KEY CLUSTERED ([EMPLOYER_ID] ASC)
);

