if not exists(select 1 from sysObjects where Upper(Name)= '__MigrationsHistory' ) begin
	exec(N'CREATE TABLE [dbo].[__MigrationsHistory](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[MigrationId] [nvarchar](200) NOT NULL,
		[Version] [nvarchar](10) NOT NULL,
		[StateName] [nvarchar](20) NOT NULL,
		[Created] [datetime2](7) NOT NULL,
	 CONSTRAINT [PK___MigrationsHistory] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]')

	exec(N'ALTER TABLE [dbo].[__MigrationsHistory] ADD  CONSTRAINT [DF___MigrationsHistory_Created]  DEFAULT (getdate()) FOR [Created]')
end


