exec(N'CREATE TABLE [SeasonCompanyProduct] (
    [Id] int NOT NULL IDENTITY,
    [CompanyId] int NOT NULL,
    [SeasonId] int NOT NULL,
    [CarTipId] int NULL,
    [count] int NOT NULL,
    [CreatorUserId] bigint NULL,
    [DeleterUserId] bigint NULL,
    [EsaleTypeId] int NULL,
    [IsComplete] int NOT NULL,
    [YearId] int NULL,
    [TotalCount] int NULL,
    [CategoryId] int NULL,
    [ProductId] int NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorId] uniqueidentifier NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierId] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit),
    [DeleterId] uniqueidentifier NULL,
    [DeletionTime] datetime2 NULL,
    CONSTRAINT [PK_SeasonCompanyProduct] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SeasonCompanyProduct_ESaleType_EsaleTypeId] FOREIGN KEY ([EsaleTypeId]) REFERENCES [ESaleType] ([Id]),
    CONSTRAINT [FK_SeasonCompanyProduct_ProductAndCategory_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [ProductAndCategory] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_SeasonCompanyProduct_ProductAndCategory_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [ProductAndCategory] ([Id]),
    CONSTRAINT [FK_SeasonCompanyProduct_Year_YearId] FOREIGN KEY ([YearId]) REFERENCES [Year] ([Id])
)')


exec(N'CREATE INDEX [IX_SeasonCompanyProduct_CompanyId] ON [SeasonCompanyProduct] ([CompanyId])')

exec(N'CREATE INDEX [IX_SeasonCompanyProduct_EsaleTypeId] ON [SeasonCompanyProduct] ([EsaleTypeId])')


exec(N'CREATE INDEX [IX_SeasonCompanyProduct_ProductId] ON [SeasonCompanyProduct] ([ProductId])')


exec(N'CREATE INDEX [IX_SeasonCompanyProduct_YearId] ON [SeasonCompanyProduct] ([YearId])')

