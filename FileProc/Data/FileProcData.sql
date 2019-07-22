IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FileProcData]') AND type in (N'U'))
DROP TABLE [dbo].[FileProcData]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FileProcData] (
	[Id] [int] NOT NULL,
	[CustomerNumber] [int] NULL,
	[CustomerName] [varchar](30) NULL,
	[CustomerSurname] [varchar](30) NULL,
	[AddressStreet1] [varchar](30) NULL,
	[AddressStreet2] [varchar](30) NULL,
	[AddressStreet3] [varchar](30) NULL,
	[AddressState] [varchar](10) NULL,
	[AddressPostcode] [varchar](5) NULL,
	[AccountNumber] [varchar](20) NULL,
	[AccountBalance] [decimal](19,4) NULL,
	[BankNumber] [varchar](10) NULL,
	[DateOfBirth] [datetime] NULL,
	[Message] [varchar](255) NULL,
	[SourceCode] [varchar](20) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING ON
GO
