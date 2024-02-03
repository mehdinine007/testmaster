if not exists(select 1 from sysObjects where Upper(Name)= 'Payment' ) begin
	exec(N'CREATE TABLE [dbo].[Payment](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[PspAccountId] [int] NOT NULL,
			[PaymentStatusId] [int] NOT NULL,
			[Amount] [decimal](18, 0) NOT NULL,
			[Token] [varchar](100) NULL,
			[TransactionCode] [varchar](100) NULL,
			[TransactionDate] [datetime2](7) NOT NULL,
			[TransactionPersianDate] [varchar](10) NOT NULL,
			[TraceNo] [varchar](50) NULL,
			[NationalCode] [varchar](10) NULL,
			[Mobile] [varchar](20) NULL,
			[AdditionalData] [varchar](1000) NULL,
			[CallBackUrl] [varchar](200) NOT NULL,
			[RetryCount] [int] NOT NULL,
			[CreationTime] [datetime2](7) NOT NULL,
			[CreatorId] [uniqueidentifier] NULL,
			[LastModificationTime] [datetime2](7) NULL,
			[LastModifierId] [uniqueidentifier] NULL,
			[IsDeleted] [bit] NOT NULL,
			[DeleterId] [uniqueidentifier] NULL,
			[DeletionTime] [datetime2](7) NULL,
			[FilterParam1] [int] NULL,
			[FilterParam2] [int] NULL,
			[FilterParam3] [int] NULL,
			[FilterParam4] [int] NULL,
		 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY]')

    exec(N'ALTER TABLE [dbo].[Payment] ADD  CONSTRAINT [DF__Payment__RetryCo__656C112C]  DEFAULT ((0)) FOR [RetryCount]')
    exec(N'ALTER TABLE [dbo].[Payment] ADD  CONSTRAINT [DF__Payments__IsDele__49C3F6B7]  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]')
    exec(N'ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_PaymentStatus_PaymentStatusId] FOREIGN KEY([PaymentStatusId])
REFERENCES [dbo].[PaymentStatus] ([Id])
ON DELETE CASCADE')
    exec(N'ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_PaymentStatus_PaymentStatusId]')
    exec(N'ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_PspAccount_PspAccountId] FOREIGN KEY([PspAccountId])
REFERENCES [dbo].[PspAccount] ([Id])
ON DELETE CASCADE')
    exec(N'ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_PspAccount_PspAccountId]')
end
