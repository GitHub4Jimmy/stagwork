﻿CREATE TABLE [delta].[ADDRESSES] (
	[DELTA_GID] UNIQUEIDENTIFIER DEFAULT NEWID() NOT NULL, 
	[KEY_VALUE] [varchar](250) NULL,
	[ENTITY_ID] [numeric](10, 0) NOT NULL,
	[ADDRESS_ID] [varchar](250) NOT NULL,
	[ADDRESS_SOURCE] [varchar](2) NULL,
	[ADDRESS_TYPE] [varchar](2) NULL,
	[ADDRESS_1] [varchar](250) NULL,
	[ADDRESS_2] [varchar](250) NULL,
	[ADDRESS_3] [varchar](250) NULL,
	[CITY] [varchar](250) NULL,
	[COUNTRY] [varchar](250) NULL,
	[COUNTY] [varchar](250) NULL,
	[LATITUDE] [numeric](13, 8) NULL,
	[LONGITUDE] [numeric](13, 8) NULL,
	[POSTAL_CODE] [varchar](10) NULL,
	[STATE] [varchar](250) NULL,
	[ZIP] [numeric](9, 0) NULL,
	[START_DATE] [datetime2](7) NULL,
	[STOP_DATE] [datetime2](7) NULL,
	[INSERTED_DATE] [datetime2](7) NULL,
	[UPDATED_DATE] [datetime2](7) NULL,
	[IMPORT_DATE] [datetime] NULL,
	[DELTA_ACTION] [varchar](250) DEFAULT 'UPDATE',
	[DELTA_SENT] [datetime2] NULL,
 CONSTRAINT [PK_DELTA_ADDRESSES] PRIMARY KEY CLUSTERED 
(
	[DELTA_GID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


