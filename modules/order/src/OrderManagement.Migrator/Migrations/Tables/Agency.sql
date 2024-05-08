if not exists(select 1 from sysObjects where Upper(Name)= 'Agency' ) begin
	exec(N'CREATE TABLE [dbo].[Agency](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](max) NULL,
		[ProvinceId] [int] NOT NULL,
		[CreationTime] [datetime2](7) NOT NULL,
		[CreatorId] [uniqueidentifier] NULL,
		[LastModificationTime] [datetime2](7) NULL,
		[LastModifierId] [uniqueidentifier] NULL,
		[IsDeleted] [bit] NOT NULL,
		[DeleterId] [uniqueidentifier] NULL,
		[DeletionTime] [datetime2](7) NULL,
	 CONSTRAINT [PK_Agency] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]')

	exec(N'ALTER TABLE [dbo].[Agency] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]')

	exec(N'ALTER TABLE [dbo].[Agency]  WITH CHECK ADD  CONSTRAINT [FK_Agency_Province_ProvinceId] FOREIGN KEY([ProvinceId])
	REFERENCES [aucbase].[Province] ([Id])
	ON DELETE CASCADE')

	exec(N'ALTER TABLE [dbo].[Agency] CHECK CONSTRAINT [FK_Agency_Province_ProvinceId]')
end

