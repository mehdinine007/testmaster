exec(N'ALTER TABLE [Organization] ALTER COLUMN [Code] nvarchar(100) NOT NULL')
exec(N'ALTER TABLE [Organization] ADD [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit)')
exec(N'ALTER TABLE [Organization] ADD [Priority] int NOT NULL DEFAULT 0')
exec(N'ALTER TABLE [Organization] ADD [SupportingPhone] nvarchar(50) ')
exec(N'ALTER TABLE [Organization] ADD [UrlSite] nvarchar(300) ')

exec(N'update [Organization] set Priority = convert(int,code) where Priority = 0')

update [Organization]
set [Code] = right('000' + [Code],4)
where len(Code) < 4


