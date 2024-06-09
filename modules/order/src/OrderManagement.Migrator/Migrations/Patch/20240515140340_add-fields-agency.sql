



exec(N'ALTER TABLE [Agency] ADD [Address] nvarchar(max) NULL');


exec(N'ALTER TABLE [Agency] ADD [AgencyType] int NOT NULL DEFAULT 0');


exec(N'ALTER TABLE [Agency] ADD [CityId] int NULL ');


exec(N'ALTER TABLE [Agency] ADD [Code] nvarchar(max) NULL');


exec(N'ALTER TABLE [Agency] ADD [Latitude] decimal(18,2) NULL');


exec(N'ALTER TABLE [Agency] ADD [Longitude] decimal(18,2) NULL');


exec(N'ALTER TABLE [Agency] ADD [PhoneNumber] nvarchar(max) NULL');


exec(N'ALTER TABLE [Agency] ADD [Visible] bit NOT NULL DEFAULT CAST(0 AS bit)');


exec(N'CREATE INDEX [IX_Agency_CityId] ON [Agency] ([CityId])');


exec(N'ALTER TABLE [Agency] ADD CONSTRAINT [FK_Agency_City_CityId] FOREIGN KEY ([CityId]) REFERENCES [aucbase].[City] ([Id])');























