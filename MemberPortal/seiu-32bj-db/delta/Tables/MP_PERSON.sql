﻿CREATE TABLE [delta].[MP_PERSON](
	[DELTA_GID] [uniqueidentifier] NOT NULL,
	[PERSON_ID] [numeric](10, 0) NOT NULL,
	[MEMBER_ID] [varchar](250) NULL,
	[PATIENT_ID] [varchar](5) NULL,
	[FIRST_NAME] [varchar](50) NULL,
	[MIDDLE_NAME] [varchar](50) NULL,
	[LAST_NAME] [varchar](50) NULL,
	[BIRTH_DATE] [datetime2](7) NULL,
	[GENDER] [varchar](50) NULL,
	[SSN] [varchar](50) NULL,
	[MARITAL_STATUS] [varchar](50) NULL,
	[MARRIAGE_DATE] [datetime2](7) NULL,
	[DIVORCE_DATE] [datetime2](7) NULL,
	[DEATH_DATE] [datetime2](7) NULL,
	[LANGUAGE] [varchar](50) NULL,
	[DATE_OF_DEATH_INSERTED] [datetime2](7) NULL,
	[PERSON_TYPE] [varchar](9) NOT NULL,
	[D1_PRIMARY_DEPENDENT_ID] [numeric](10, 0) NULL,
	[D1_PRIMARY_PERSON_ID] [numeric](10, 0) NULL,
	[D1_DEPENDENT_SEQ_NO] [numeric](5, 0) NULL,
	[D1_QMCSO_FLAG] [char](1) NULL,
	[D1_QMCSO_FROM_DATE] [datetime2](7) NULL,
	[D1_QMCSO_UNTIL_DATE] [datetime2](7) NULL,
	[D1_RELATION] [varchar](2) NULL,
	[D1_RELATION_NAME] [varchar](250) NULL,
	[D1_DISABILITY] [char](1) NULL,
	[D1_MARRIAGE_DATE] [datetime2](7) NULL,
	[D1_DIVORCE_DATE] [datetime2](7) NULL,
	[D1_DIVORCE_NOTIFICATION_DATE] [datetime2](7) NULL,
	[D2_PRIMARY_DEPENDENT_ID] [numeric](10, 0) NULL,
	[D2_PRIMARY_PERSON_ID] [numeric](10, 0) NULL,
	[D2_DEPENDENT_SEQ_NO] [numeric](5, 0) NULL,
	[D2_QMCSO_FLAG] [char](1) NULL,
	[D2_QMCSO_FROM_DATE] [datetime2](7) NULL,
	[D2_QMCSO_UNTIL_DATE] [datetime2](7) NULL,
	[D2_RELATION] [varchar](2) NULL,
	[D2_RELATION_NAME] [varchar](250) NULL,
	[D2_DISABILITY] [char](1) NULL,
	[D2_MARRIAGE_DATE] [datetime2](7) NULL,
	[D2_DIVORCE_DATE] [datetime2](7) NULL,
	[D2_DIVORCE_NOTIFICATION_DATE] [datetime2](7) NULL,
	[RETIREMENT_STATUS] [varchar](200) NULL,
	[CURRENT_EMPLOYMENT_STATUS] [varchar](200) NULL,
	[JOB_COUNT] [int] NULL,
	[SECURITY_TYPE] [varchar](50) NULL,
	[ADDRESS_1] [varchar](250) NULL,
	[ADDRESS_2] [varchar](250) NULL,
	[ADDRESS_3] [varchar](250) NULL,
	[CITY] [varchar](50) NULL,
	[STATE] [varchar](10) NULL,
	[ZIP] [varchar](5) NULL,
	[COUNTY_NAME] [varchar](50) NULL,
	[COUNTRY] [varchar](50) NULL,
	[ADDRESS_FLAG] [char](1) NULL,
	[ADDRESS_SOURCE] [varchar](250) NULL,
	[ADDRESS_LONGITUDE] [numeric](13, 8) NULL,
	[ADDRESS_LATITUDE] [numeric](13, 8) NULL,
	[POSTAL_CODE] [varchar](10) NULL,
	[ADDRESS_ID] [varchar](250) NULL,
	[ADDRESS_START_DATE] [datetime2](7) NULL,
	[ADDRESS_STOP_DATE] [datetime2](7) NULL,
	[CUSTODIAL_PERSON_ID] [numeric](10, 0) NULL,
	[ALT_ADDRESS_ID] [varchar](250) NULL,
	[ALT_ADDRESS_1] [varchar](250) NULL,
	[ALT_ADDRESS_2] [varchar](250) NULL,
	[ALT_ADDRESS_3] [varchar](250) NULL,
	[ALT_CITY] [varchar](50) NULL,
	[ALT_STATE] [varchar](10) NULL,
	[ALT_ZIP] [varchar](5) NULL,
	[ALT_COUNTRY] [varchar](50) NULL,
	[EMAIL_CONTACT_INFO_ID] [varchar](250) NULL,
	[EMAIL] [varchar](50) NULL,
	[HOME_PHONE_CONTACT_INFO_ID] [varchar](250) NULL,
	[HOME_PHONE] [varchar](20) NULL,
	[MOBILE_PHONE_CONTACT_INFO_ID] [varchar](250) NULL,
	[MOBILE_PHONE] [varchar](20) NULL,
	[BUSINESS_PHONE_CONTACT_INFO_ID] [varchar](250) NULL,
	[BUSINESS_PHONE] [varchar](20) NULL,
	[BUSINESS_PHONE_EXT] [varchar](50) NULL,
	[VESTED_STATUS] [varchar](2) NULL,
	[E_DELIVERY_OPTION] [bit] NULL,
	[FUTURE_HEALTH] [date] NULL,
	[FUTURE_LEGAL] [date] NULL,
	[FUTURE_TRAINING] [date] NULL,
	[FUTURE_401K] [date] NULL,
	[FUTURE_PENSION] [date] NULL,
	[FUTURE_PROFIT_SHARING] [date] NULL,
	[INSERTED_DATE] [datetime2](7) NOT NULL,
	[UPDATED_DATE] [datetime2](7) NULL,
	[DELTA_ACTION] [varchar](250) NULL,
	[DELTA_SENT] [datetime2](7) NULL,
 CONSTRAINT [PK_MP_PERSON] PRIMARY KEY CLUSTERED 
(
	[DELTA_GID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [delta].[MP_PERSON] ADD  CONSTRAINT [DF__MP_PERSON__DELTA__4E3E9311]  DEFAULT (newid()) FOR [DELTA_GID]
GO

ALTER TABLE [delta].[MP_PERSON] ADD  CONSTRAINT [DF__MP_PERSON__DELTA__4F32B74A]  DEFAULT ('UPDATE') FOR [DELTA_ACTION]
GO
