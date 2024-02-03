if not exists(select 1 from sysObjects where Upper(Name)= 'PaymentLog' ) begin
	exec(N'	CREATE TABLE [dbo].[PaymentLog](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[PaymentId] [int] NOT NULL,
			[Psp] [nvarchar](100) NOT NULL,
			[Message] [nvarchar](100) NOT NULL,
			[Parameter] [nvarchar](max) NULL,
			[CreationTime] [datetime2](7) NOT NULL,
			[CreatorId] [uniqueidentifier] NULL,
			[LastModificationTime] [datetime2](7) NULL,
			[LastModifierId] [uniqueidentifier] NULL,
			[IsDeleted] [bit] NOT NULL,
			[DeleterId] [uniqueidentifier] NULL,
			[DeletionTime] [datetime2](7) NULL,
		 CONSTRAINT [PK_PaymentLog] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]')

    exec(N'ALTER TABLE [dbo].[PaymentLog] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]')
    exec(N'ALTER TABLE [dbo].[PaymentLog]  WITH CHECK ADD  CONSTRAINT [FK_PaymentLog_Payment_PaymentId] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([Id])
ON DELETE CASCADE')
    exec(N'ALTER TABLE [dbo].[PaymentLog] CHECK CONSTRAINT [FK_PaymentLog_Payment_PaymentId]')
end


