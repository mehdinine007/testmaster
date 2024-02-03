

CREATE TABLE [OldCars] (
    [Id] int NOT NULL IDENTITY,
    [Vehicle] nvarchar(max) NULL,
    [Nationalcode] nvarchar(max) NULL,
    [Vin] nvarchar(max) NULL,
    [ChassiNo] nvarchar(max) NULL,
    [EngineNo] nvarchar(max) NULL,
    [BatchNo] int NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorId] uniqueidentifier NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierId] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit),
    [DeleterId] uniqueidentifier NULL,
    [DeletionTime] datetime2 NULL,
    CONSTRAINT [PK_OldCars] PRIMARY KEY ([Id])
);


