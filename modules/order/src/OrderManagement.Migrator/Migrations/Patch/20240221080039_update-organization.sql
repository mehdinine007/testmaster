exec(N'select * into TmpOrganization_BeforeAddCopmany from [Organization]')
exec(N'ALTER TABLE [Organization] ALTER COLUMN [Code] nvarchar(100) NOT NULL')
exec(N'ALTER TABLE [Organization] ADD [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit)')
exec(N'ALTER TABLE [Organization] ADD [Priority] int NOT NULL DEFAULT 0')
exec(N'ALTER TABLE [Organization] ADD [SupportingPhone] nvarchar(50) ')
exec(N'ALTER TABLE [Organization] ADD [UrlSite] nvarchar(300) ')

exec(N'truncate table [Organization]')
exec(N'
   SET IDENTITY_INSERT [dbo].[Organization] ON
       insert into [Organization] ([Id], [Code], [Title], [CreationTime],[IsDeleted], [IsActive], [Priority])
       select [Id], [Code], [Title], Getdate(),0, 1, convert(int,code)
	   from ProductAndCategory
	   where IsDeleted = 0 and ParentId is null and LevelId = 1
   SET IDENTITY_INSERT [dbo].[Organization] OFF 
')

update Attachments
set Entity = 9
where Entity = 1 and EntityId in (select o.Id from Organization o)



