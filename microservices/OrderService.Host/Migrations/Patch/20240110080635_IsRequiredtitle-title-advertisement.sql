
DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AdvertisementDetail]') AND [c].[name] = N'Title');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [AdvertisementDetail] DROP CONSTRAINT [' + @var0 + '];');
UPDATE [AdvertisementDetail] SET [Title] = N'' WHERE [Title] IS NULL;
ALTER TABLE [AdvertisementDetail] ALTER COLUMN [Title] nvarchar(100) NOT NULL;
ALTER TABLE [AdvertisementDetail] ADD DEFAULT N'' FOR [Title];


DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Advertisement]') AND [c].[name] = N'Title');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Advertisement] DROP CONSTRAINT [' + @var1 + '];');
UPDATE [Advertisement] SET [Title] = N'' WHERE [Title] IS NULL;
ALTER TABLE [Advertisement] ALTER COLUMN [Title] nvarchar(100) NOT NULL;
ALTER TABLE [Advertisement] ADD DEFAULT N'' FOR [Title];





