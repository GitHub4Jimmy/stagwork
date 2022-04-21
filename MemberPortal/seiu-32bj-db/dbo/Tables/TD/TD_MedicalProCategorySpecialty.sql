CREATE TABLE [dbo].[TD_MedicalProCategorySpecialty](
	[Id] [int] NOT NULL,
	[MedicalProCategoryId] [int] NOT NULL,
	[SpecialtyId] [int] NOT NULL,
 CONSTRAINT [PK_MedicalProCategorySpecialtyId] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
