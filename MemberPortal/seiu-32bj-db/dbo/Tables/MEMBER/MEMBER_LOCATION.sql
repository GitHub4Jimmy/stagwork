CREATE TABLE [dbo].[MEMBER_LOCATION]
(
	[MEMBER_LOCATION_ID] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[MEMBER_ID] NUMERIC(10) NOT NULL,
    [LOCATION_TYPE] INT NOT NULL,
	[ADDRESS_1]      VARCHAR (250)                                                   NULL,
    [ADDRESS_2]      VARCHAR (250)                                                   NULL,
    [ADDRESS_3]      VARCHAR (250)                                                   NULL,
    [CITY]           VARCHAR (50)                                                    NULL,
    [COUNTRY]        VARCHAR (50)                                                    NULL,
    [LATITUDE]       NUMERIC (13, 8)                                                 NULL,
    [LONGITUDE]      NUMERIC (13, 8)                                                 NULL,
    [POSTAL_CODE]    VARCHAR (10)                                                    NULL,
    [STATE]          VARCHAR (10)                                                    NULL,
    [ZIP]            NUMERIC (9)                                                     NULL,
    [START_DATE]     DATETIME2 (7)                                                   NOT NULL,
    [STOP_DATE]      DATETIME2 (7)                                                   NULL,
    [INSERTED_BY] VARCHAR(100) NULL, 
    [INSERTED_DATE] DATETIME2 NULL DEFAULT GETDATE(), 
    [UPDATED_BY] VARCHAR(100) NULL, 
    [UPDATED_DATE] DATETIME2 NULL DEFAULT GETDATE(), 
    CONSTRAINT [FK__MEMBER_LOCATION__MEMBER] FOREIGN KEY ([MEMBER_ID]) REFERENCES [MEMBER]([MEMBER_ID])
)
