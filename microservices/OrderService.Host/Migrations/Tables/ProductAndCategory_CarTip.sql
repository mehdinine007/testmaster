if not exists(select 1 from sysObjects where Upper(Name)= 'ProductAndCategory_CarTip' ) begin
	exec(N'CREATE TABLE [dbo].[ProductAndCategory_CarTip](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ProductId] [int] NOT NULL,
		[CarTipId] [int] NOT NULL,
		[CreationTime] [datetime2](7) NOT NULL,
		[CreatorUserId] [bigint] NULL,
		[LastModificationTime] [datetime2](7) NULL,
		[LastModifierUserId] [bigint] NULL,
		[IsDeleted] [bit] NOT NULL,
		[DeleterUserId] [bigint] NULL,
		[DeletionTime] [datetime2](7) NULL,
		[CreatorId] [uniqueidentifier] NULL,
		[LastModifierId] [uniqueidentifier] NULL,
		[DeleterId] [uniqueidentifier] NULL,
	 CONSTRAINT [PK_ProductAndCategory_CarTip] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]')
end


