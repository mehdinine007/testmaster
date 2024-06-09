if not exists(select 1 from sysObjects where Upper(Name)= 'Color' ) begin
	exec(N'CREATE TABLE [dbo].[Color](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ColorName] [nvarchar](max) NULL,
		[ColorCode] [int] NOT NULL,
		[CreationTime] [datetime2](7) NOT NULL,
		[CreatorId] [uniqueidentifier] NULL,
		[DeleterId] [uniqueidentifier] NULL,
		[DeletionTime] [datetime2](7) NULL,
		[IsDeleted] [bit] NOT NULL,
		[LastModificationTime] [datetime2](7) NULL,
		[LastModifierId] [uniqueidentifier] NULL,
	 CONSTRAINT [PK_Color] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]')

	exec(N'ALTER TABLE [dbo].[Color] ADD  DEFAULT (''0001-01-01T00:00:00.0000000'') FOR [CreationTime]')

	exec(N'ALTER TABLE [dbo].[Color] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]')
end

