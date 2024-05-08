if not exists(select 1 from sysObjects where Upper(Name)= 'SaleDetailCarColor' ) begin
	exec(N'CREATE TABLE [dbo].[SaleDetailCarColor](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[SaleDetailId] [int] NOT NULL,
		[ColorId] [int] NOT NULL,
		[CreationTime] [datetime2](7) NOT NULL,
		[CreatorId] [uniqueidentifier] NULL,
		[LastModificationTime] [datetime2](7) NULL,
		[LastModifierId] [uniqueidentifier] NULL,
		[IsDeleted] [bit] NOT NULL,
		[DeleterId] [uniqueidentifier] NULL,
		[DeletionTime] [datetime2](7) NULL,
	 CONSTRAINT [PK_SaleDetailCarColor] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]')

	exec(N'ALTER TABLE [dbo].[SaleDetailCarColor] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]')

	exec(N'ALTER TABLE [dbo].[SaleDetailCarColor]  WITH CHECK ADD  CONSTRAINT [FK_SaleDetailCarColor_Color_ColorId] FOREIGN KEY([ColorId])
	REFERENCES [dbo].[Color] ([Id])
	ON DELETE CASCADE')

	exec(N'ALTER TABLE [dbo].[SaleDetailCarColor] CHECK CONSTRAINT [FK_SaleDetailCarColor_Color_ColorId]')

	exec(N'ALTER TABLE [dbo].[SaleDetailCarColor]  WITH CHECK ADD  CONSTRAINT [FK_SaleDetailCarColor_SaleDetail_SaleDetailId] FOREIGN KEY([SaleDetailId])
	REFERENCES [dbo].[SaleDetail] ([Id])
	ON DELETE CASCADE')

	exec(N'ALTER TABLE [dbo].[SaleDetailCarColor] CHECK CONSTRAINT [FK_SaleDetailCarColor_SaleDetail_SaleDetailId]')
end

