if not exists(select 1 from sysObjects where Upper(Name)= 'AgencySaleDetail' ) begin
	exec(N'CREATE TABLE [dbo].[AgencySaleDetail](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[DistributionCapacity] [int] NOT NULL,
		[AgencyId] [int] NOT NULL,
		[SaleDetailId] [int] NOT NULL,
		[CreationTime] [datetime2](7) NOT NULL,
		[CreatorId] [uniqueidentifier] NULL,
		[LastModificationTime] [datetime2](7) NULL,
		[LastModifierId] [uniqueidentifier] NULL,
		[IsDeleted] [bit] NOT NULL,
		[DeleterId] [uniqueidentifier] NULL,
		[DeletionTime] [datetime2](7) NULL,
		[ReserveCount] [int] NOT NULL,
	 CONSTRAINT [PK_AgencySaleDetail] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]')

	exec(N'ALTER TABLE [dbo].[AgencySaleDetail] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]')

	exec(N'ALTER TABLE [dbo].[AgencySaleDetail] ADD  DEFAULT ((0)) FOR [ReserveCount]')

	exec(N'ALTER TABLE [dbo].[AgencySaleDetail]  WITH CHECK ADD  CONSTRAINT [FK_AgencySaleDetail_Agency_AgencyId] FOREIGN KEY([AgencyId])
	REFERENCES [dbo].[Agency] ([Id])
	ON DELETE CASCADE')

	exec(N'ALTER TABLE [dbo].[AgencySaleDetail] CHECK CONSTRAINT [FK_AgencySaleDetail_Agency_AgencyId]')

	exec(N'ALTER TABLE [dbo].[AgencySaleDetail]  WITH CHECK ADD  CONSTRAINT [FK_AgencySaleDetail_SaleDetail_SaleDetailId] FOREIGN KEY([SaleDetailId])
	REFERENCES [dbo].[SaleDetail] ([Id])
	ON DELETE CASCADE')

	exec(N'ALTER TABLE [dbo].[AgencySaleDetail] CHECK CONSTRAINT [FK_AgencySaleDetail_SaleDetail_SaleDetailId]')
end

