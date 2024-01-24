if not exists(select 1 from sysObjects where Upper(Name)= 'DashboardWidgets' ) begin
	exec(N'CREATE TABLE [dbo].[DashboardWidgets](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[DashboardId] [int] NOT NULL,
		[WidgetId] [int] NOT NULL,
		[CreationTime] [datetime2](7) NOT NULL,
		[CreatorId] [uniqueidentifier] NULL,
		[LastModificationTime] [datetime2](7) NULL,
		[LastModifierId] [uniqueidentifier] NULL,
		[IsDeleted] [bit] NOT NULL,
		[DeleterId] [uniqueidentifier] NULL,
		[DeletionTime] [datetime2](7) NULL,
	 CONSTRAINT [PK_DashboardWidgets] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]')

	exec(N'ALTER TABLE [dbo].[DashboardWidgets] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]')

	exec(N'ALTER TABLE [dbo].[DashboardWidgets]  WITH CHECK ADD  CONSTRAINT [FK_DashboardWidgets_Dashboards_DashboardId] FOREIGN KEY([DashboardId])
	REFERENCES [dbo].[Dashboards] ([Id])
	ON DELETE CASCADE')

	exec(N'ALTER TABLE [dbo].[DashboardWidgets] CHECK CONSTRAINT [FK_DashboardWidgets_Dashboards_DashboardId]')

	exec(N'ALTER TABLE [dbo].[DashboardWidgets]  WITH CHECK ADD  CONSTRAINT [FK_DashboardWidgets_Widgets_WidgetId] FOREIGN KEY([WidgetId])
	REFERENCES [dbo].[Widgets] ([Id])
	ON DELETE CASCADE')

	exec(N'ALTER TABLE [dbo].[DashboardWidgets] CHECK CONSTRAINT [FK_DashboardWidgets_Widgets_WidgetId]')
end


