CREATE TABLE [dbo].[TD_Address](
	[Id] [int] NOT NULL,
	[Street] [varchar](60) NOT NULL,
	[City] [varchar](40) NOT NULL,
	[StateId] [int] NOT NULL,
	[PostalCode] [varchar](5) NOT NULL,
	[PostalCodeSuffix] [varchar](4) NULL,
	[County] [varchar](25) NULL,
	[AddressLineOther] [varchar](35) NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[FiveStarCenter] [bit] NULL,
 CONSTRAINT [PK_AddressId] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]