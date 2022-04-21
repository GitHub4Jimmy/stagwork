CREATE TABLE [dbo].[TD_Ref_Specialty](
	[Id] [int] NOT NULL,
	[SpecialtyDescription] [varchar](75) NULL,
	[SpecialtyAbbrev] [varchar](3) NULL,
	[ProviderType] [int] NULL,
	[ES_SpecialtyDescription] [varchar](150) NULL,
	[SSA_Specialty] [varchar](2) NULL,
	[CommonTerm] [varchar](1000) NULL,
 CONSTRAINT [PK_SpecialtyId1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
