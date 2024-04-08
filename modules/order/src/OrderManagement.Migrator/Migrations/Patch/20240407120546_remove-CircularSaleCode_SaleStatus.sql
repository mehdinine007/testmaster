if  exists(select 1 from INFORMATION_SCHEMA.COLUMNS where Upper(TABLE_NAME)= 'SaleDetail' and Upper(COLUMN_NAME)='CircularSaleCode' ) begin
DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SaleDetail]') AND [c].[name] = N'CircularSaleCode');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [SaleDetail] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [SaleDetail] DROP COLUMN [CircularSaleCode];
end
if  exists(select 1 from INFORMATION_SCHEMA.COLUMNS where Upper(TABLE_NAME)= 'SaleSchema' and Upper(COLUMN_NAME)='SaleStatus' ) begin
     ALTER TABLE SaleSchema DROP COLUMN SaleStatus;
end

exec(N'ALTER TABLE SaleSchema
ADD Code int  NOT NULL DEFAULT 0');