

exec(N'CREATE TABLE [AdvocacyUsersFromBank] (
    [Id] int NOT NULL IDENTITY,
    [nationalcode] nvarchar(max) NULL,
    [bankName] nvarchar(max) NULL,
    [price] decimal(18,2) NOT NULL,
    [dateTime] datetime2 NULL,
    [accountNumber] nvarchar(max) NULL,
    [shabaNumber] nvarchar(max) NULL,
    [UserId] bigint NOT NULL,
    [BanksId] int NULL,
    [CompanyId] int NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorId] uniqueidentifier NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierId] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit),
    [DeleterId] uniqueidentifier NULL,
    [DeletionTime] datetime2 NULL,
    CONSTRAINT [PK_AdvocacyUsersFromBank] PRIMARY KEY ([Id]))');


exec(N'CREATE TABLE [UserRejectionFromBank] (
    [Id] int NOT NULL IDENTITY,
    [nationalcode] nvarchar(max) NOT NULL,
    [bankName] nvarchar(max) NULL,
    [price] decimal(18,2) NOT NULL,
    [dateTime] datetime2 NULL,
    [accountNumber] nvarchar(max) NULL,
    [shabaNumber] nvarchar(max) NULL,
    [UserId] bigint NOT NULL,
    [BanksId] int NULL,
    [CarMaker] nvarchar(max) NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorId] uniqueidentifier NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierId] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit),
    [DeleterId] uniqueidentifier NULL,
    [DeletionTime] datetime2 NULL,
    CONSTRAINT [PK_UserRejectionFromBank] PRIMARY KEY ([Id]))');

