DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SeasonCompanyProduct]') AND [c].[name] = N'CreatorUserId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [SeasonCompanyProduct] DROP CONSTRAINT [' + @var2 + '];');
exec ('ALTER TABLE [SeasonCompanyProduct] DROP COLUMN [CreatorUserId]');

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SeasonCompanyProduct]') AND [c].[name] = N'DeleterUserId');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [SeasonCompanyProduct] DROP CONSTRAINT [' + @var3 + '];');
exec ('ALTER TABLE [SeasonCompanyProduct] DROP COLUMN [DeleterUserId]');

EXEC('ALTER TABLE [SeasonCompanyProduct] ALTER COLUMN CompanyId int null');

EXEC('ALTER TABLE [SeasonCompanyProduct] ALTER COLUMN ProductId int null');

EXEC('ALTER TABLE [SeasonCompanyProduct] ALTER COLUMN EsaleTypeId int null');

EXEC('ALTER TABLE [SeasonCompanyProduct] ADD SaleDetailId int null');

EXEC ('CREATE INDEX [IX_SeasonCompanyProduct_SaleDetailId] ON [SeasonCompanyProduct] ([SaleDetailId])');

EXEC('ALTER TABLE [dbo].[SeasonCompanyProduct]  WITH CHECK ADD  CONSTRAINT [FK_SeasonCompanyProduct_SaleDetail] FOREIGN KEY([SaleDetailId])
REFERENCES [dbo].[SaleDetail] ([Id])');

EXEC('ALTER TABLE [dbo].[SeasonCompanyProduct] CHECK CONSTRAINT [FK_SeasonCompanyProduct_SaleDetail]')



