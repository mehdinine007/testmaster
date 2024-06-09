if not exists(select 1 from sysObjects where Upper(Name)= 'SaleDetailAllocation' ) begin
	exec(N'CREATE TABLE [dbo].[SaleDetailAllocation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SaleDetailId] [int] NOT NULL,
	[SeasonAllocationId] [int] NULL,
	[Count] [int] NOT NULL,
	[IsComplete] [bit] NOT NULL,
	[TotalCount] [int] NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[CreatorId] [uniqueidentifier] NULL,
	[LastModificationTime] [datetime2](7) NULL,
	[LastModifierId] [uniqueidentifier] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeleterId] [uniqueidentifier] NULL,
	[DeletionTime] [datetime2](7) NULL,
 CONSTRAINT [PK_SeasonCompanyProduct] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
')

exec(N'ALTER TABLE [dbo].[SaleDetailAllocation] ADD  CONSTRAINT [DF__SeasonCom__SaleD__2AEB3533]  DEFAULT ((0)) FOR [SaleDetailId]')

exec(N'ALTER TABLE [dbo].[SaleDetailAllocation] ADD  CONSTRAINT [DF__SeasonCom__IsDel__10966653]  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]')

exec(N'ALTER TABLE [dbo].[SaleDetailAllocation]  WITH CHECK ADD  CONSTRAINT [FK__SeasonCom__SaleD__37510C18] FOREIGN KEY([SaleDetailId])
REFERENCES [dbo].[SaleDetail] ([Id])')

exec(N'ALTER TABLE [dbo].[SaleDetailAllocation]  WITH CHECK ADD  CONSTRAINT [FK_SaleDetailAllocation_SeasonAllocation_SeasonAllocationId] FOREIGN KEY([SeasonAllocationId])
REFERENCES [dbo].[SeasonAllocation] ([Id])')

exec(N'ALTER TABLE [dbo].[SaleDetailAllocation] CHECK CONSTRAINT [FK_SaleDetailAllocation_SeasonAllocation_SeasonAllocationId]')

end

