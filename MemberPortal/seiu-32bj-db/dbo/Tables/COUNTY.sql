CREATE TABLE [dbo].[COUNTY] (
    [COUNTY_ID]   NUMERIC (10) NOT NULL,
    [COUNTY_NAME] VARCHAR (50) NOT NULL,
    [COUNTY_CODE] VARCHAR (10) NOT NULL,
    [STATE_ID]    NUMERIC (10) NOT NULL,
    CONSTRAINT [PK_COUNTY] PRIMARY KEY CLUSTERED ([COUNTY_ID] ASC),
    CONSTRAINT [FK_COUNTY_STATE_ID] FOREIGN KEY ([STATE_ID]) REFERENCES [dbo].[STATE] ([STATE_ID]) NOT FOR REPLICATION
);


GO
ALTER TABLE [dbo].[COUNTY] NOCHECK CONSTRAINT [FK_COUNTY_STATE_ID];

