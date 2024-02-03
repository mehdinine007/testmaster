if not exists(select 1 from sysObjects where Upper(Name)= 'Account' ) begin
	exec(N'
	CREATE TABLE [dbo].[Account](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[CustomerId] [int] NOT NULL,
		[AccountName] [nvarchar](100) NOT NULL,
		[Branch] [nvarchar](100) NULL,
		[AccountNumber] [nvarchar](100) NULL,
		[IBAN] [nvarchar](100) NULL,
		[IsActive] [bit] NOT NULL,
		[CreationTime] [datetime2](7) NOT NULL,
		[CreatorId] [uniqueidentifier] NULL,
		[LastModificationTime] [datetime2](7) NULL,
		[LastModifierId] [uniqueidentifier] NULL,
		[IsDeleted] [bit] NOT NULL,
		[DeleterId] [uniqueidentifier] NULL,
		[DeletionTime] [datetime2](7) NULL,
	 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
	')

    exec(N'ALTER TABLE [dbo].[Account] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]')

	exec(N'ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Customer_CustomerId] FOREIGN KEY([CustomerId])
	REFERENCES [dbo].[Customer] ([Id])
	ON DELETE CASCADE')

	exec(N'ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Customer_CustomerId]')

end


