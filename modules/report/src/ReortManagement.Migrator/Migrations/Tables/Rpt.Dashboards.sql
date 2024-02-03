if not exists(select 1 from sysObjects where Upper(Name)= 'Dashboards' ) begin
	exec(N'CREATE TABLE [dbo].[Dashboards](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Title] [nvarchar](100) NULL,
		[Priority] [int] NOT NULL,
		[CreationTime] [datetime2](7) NOT NULL,
		[CreatorId] [uniqueidentifier] NULL,
		[LastModificationTime] [datetime2](7) NULL,
		[LastModifierId] [uniqueidentifier] NULL,
		[IsDeleted] [bit] NOT NULL,
		[DeleterId] [uniqueidentifier] NULL,
		[DeletionTime] [datetime2](7) NULL,
		[Roles] [nvarchar](max) NULL,
	 CONSTRAINT [PK_Dashboards] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]')

    exec(N'ALTER TABLE [dbo].[Dashboards] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]')
end


