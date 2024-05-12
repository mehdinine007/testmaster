
exec(N'ALTER TABLE [SaleDetailAllocation] ADD [SeasonAllocationId] int NULL');


exec(N'CREATE INDEX [IX_SaleDetailAllocation_SeasonAllocationId] ON [SaleDetailAllocation] ([SeasonAllocationId])');


exec(N'ALTER TABLE [SaleDetailAllocation] ADD CONSTRAINT [FK_SaleDetailAllocation_SeasonAllocation_SeasonAllocationId] FOREIGN KEY ([SeasonAllocationId]) REFERENCES [SeasonAllocation] ([Id])');


