if not exists(select 1 from sysObjects where Upper(Name)= 'Widgets' ) begin
	exec(N'CREATE TABLE [dbo].[Widgets](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Title] [nvarchar](100) NULL,
		[Type] [int] NOT NULL,
		[Command] [nvarchar](max) NULL,
		[Fields] [nvarchar](max) NULL,
		[CreationTime] [datetime2](7) NOT NULL,
		[CreatorId] [uniqueidentifier] NULL,
		[LastModificationTime] [datetime2](7) NULL,
		[LastModifierId] [uniqueidentifier] NULL,
		[IsDeleted] [bit] NOT NULL,
		[DeleterId] [uniqueidentifier] NULL,
		[DeletionTime] [datetime2](7) NULL,
		[Condition] [nvarchar](max) NULL,
		[OutPutType] [int] NOT NULL,
		[Roles] [nvarchar](max) NULL,
		[ReportId] [int] NULL,
	 CONSTRAINT [PK_Widgets] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]')

	exec(N'ALTER TABLE [dbo].[Widgets] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]')

	exec(N'ALTER TABLE [dbo].[Widgets] ADD  DEFAULT ((0)) FOR [OutPutType]')
end


