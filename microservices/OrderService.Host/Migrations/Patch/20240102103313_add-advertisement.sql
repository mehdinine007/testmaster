exec(N'CREATE TABLE [Advertisement] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorId] uniqueidentifier NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierId] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit),
    [DeleterId] uniqueidentifier NULL,
    [DeletionTime] datetime2 NULL,
    CONSTRAINT [PK_Advertisement] PRIMARY KEY ([Id])
)')

exec(N'CREATE TABLE [AdvertisementDetail] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NULL,
    [Url] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [AdvertisementId] int NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorId] uniqueidentifier NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierId] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit),
    [DeleterId] uniqueidentifier NULL,
    [DeletionTime] datetime2 NULL,
    CONSTRAINT [PK_AdvertisementDetail] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AdvertisementDetail_Advertisement_AdvertisementId] FOREIGN KEY ([AdvertisementId]) REFERENCES [Advertisement] ([Id]) ON DELETE CASCADE
)')

exec(N'CREATE INDEX [IX_AdvertisementDetail_AdvertisementId] ON [AdvertisementDetail] ([AdvertisementId])')
