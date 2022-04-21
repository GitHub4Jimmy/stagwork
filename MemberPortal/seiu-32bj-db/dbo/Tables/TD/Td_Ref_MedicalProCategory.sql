CREATE TABLE [dbo].[Td_Ref_MedicalProCategory](
	[Id] [int] NOT NULL,
	[MedicalProCategory] [varchar](50) NULL,
	[IsSpecialist] [bit] NULL,
	[IsFacility] [bit] NULL,
	[ES_MedicalProCategory] [varchar](150) NULL,
	[parentid] [int] NULL,
	[isVisible] [bit] NULL,
	[benefitType] [varchar](50) NULL,
	[seqid] [int] NULL,
	[showDetail] [bit] NULL,
	[selectMode] [int] NULL,
 CONSTRAINT [PK_MedProId] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]