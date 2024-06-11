if not exists(select 1 from sysObjects where Upper(Name)= 'SeasonAllocation' ) begin
	exec(N'CREATE TABLE [SeasonAllocation] (
    [Id] int NOT NULL IDENTITY,
    [Code] int NOT NULL,
    [Title] nvarchar(max) NULL,
    [SeasonId] int NOT NULL,
    [Year] int NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorId] uniqueidentifier NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierId] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit),
    [DeleterId] uniqueidentifier NULL,
    [DeletionTime] datetime2 NULL,
    CONSTRAINT [PK_SeasonAllocation] PRIMARY KEY ([Id])
)
')


end

