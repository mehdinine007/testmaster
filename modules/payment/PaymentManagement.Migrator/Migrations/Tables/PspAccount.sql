if not exists(select 1 from sysObjects where Upper(Name)= 'PspAccount' ) begin
	exec(N'CREATE TABLE [dbo].[PspAccount](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[PspId] [int] NOT NULL,
			[AccountId] [int] NOT NULL,
			[IsActive] [bit] NOT NULL,
			[JsonProps] [varchar](500) NOT NULL,
			[Logo] [varchar](200) NULL,
			[CreationTime] [datetime2](7) NOT NULL,
			[CreatorId] [uniqueidentifier] NULL,
			[LastModificationTime] [datetime2](7) NULL,
			[LastModifierId] [uniqueidentifier] NULL,
			[IsDeleted] [bit] NOT NULL,
			[DeleterId] [uniqueidentifier] NULL,
			[DeletionTime] [datetime2](7) NULL,
		 CONSTRAINT [PK_PspAccount] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY]')

    exec(N'ALTER TABLE [dbo].[PspAccount] ADD  CONSTRAINT [DF__PspAccoun__JsonP__6477ECF3]  DEFAULT ('''') FOR [JsonProps]')
    exec(N'ALTER TABLE [dbo].[PspAccount] ADD  CONSTRAINT [DF__PspAccoun__IsDel__44FF419A]  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]')
    exec(N'ALTER TABLE [dbo].[PspAccount]  WITH CHECK ADD  CONSTRAINT [FK_PspAccount_Account_AccountId] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
ON DELETE CASCADE')
    exec(N'ALTER TABLE [dbo].[PspAccount] CHECK CONSTRAINT [FK_PspAccount_Account_AccountId]')
    exec(N'ALTER TABLE [dbo].[PspAccount]  WITH CHECK ADD  CONSTRAINT [FK_PspAccount_Psp_PspId] FOREIGN KEY([PspId])
REFERENCES [dbo].[Psp] ([Id])
ON DELETE CASCADE')
    exec(N'ALTER TABLE [dbo].[PspAccount] CHECK CONSTRAINT [FK_PspAccount_Psp_PspId]')

end


