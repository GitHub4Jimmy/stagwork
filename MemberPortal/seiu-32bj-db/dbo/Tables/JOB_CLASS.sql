﻿CREATE TABLE [dbo].[JOB_CLASS] (
    [JOB_CATEGORY]    VARCHAR (25) NULL,
    [JOB_CLASS]       VARCHAR (10) NOT NULL,
    [JOB_CLASS_ID]    NUMERIC (10) NOT NULL,
    [JOB_DESCRIPTION] VARCHAR (50) NULL,
    CONSTRAINT [PK_JOB_CLASS] PRIMARY KEY CLUSTERED ([JOB_CLASS_ID] ASC),
    CONSTRAINT [FK_JOB_CLASS_JOB_CATEGORY] FOREIGN KEY ([JOB_CATEGORY]) REFERENCES [dbo].[JOB_CATEGORY] ([JOB_CATEGORY]) NOT FOR REPLICATION
);


GO
ALTER TABLE [dbo].[JOB_CLASS] NOCHECK CONSTRAINT [FK_JOB_CLASS_JOB_CATEGORY];
