CREATE TABLE [dbo].[TD_Provider](
	[Id] [int] NOT NULL,
	[FullName] [varchar](80) NULL,
	[LastName] [varchar](20) NULL,
	[FirstName] [varchar](20) NULL,
	[MiddleInit] [char](1) NULL,
	[Suffix] [char](3) NULL,
	[Sex] [char](1) NULL,
	[PcpEnrollId] [varchar](12) NULL,
	[ProviderTypeId] [int] NULL,
	[ProviderRoleId] [int] NULL,
	[NETWORK] [varchar](20) NULL,
	[CCN] [char](1) NULL,
	[ProviderUniqueId] [varchar](25) NOT NULL,
 CONSTRAINT [PK_ProviderId] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]