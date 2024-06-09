exec(N'ALTER TABLE [CustomerOrder] ADD [SeasonAllocationId] int NULL')

exec(N'CREATE INDEX [IX_CustomerOrder_SeasonAllocationId] ON [CustomerOrder] ([SeasonAllocationId])')

exec(N'ALTER TABLE [CustomerOrder] ADD CONSTRAINT [FK_CustomerOrder_SeasonAllocation_SeasonAllocationId] FOREIGN KEY ([SeasonAllocationId]) REFERENCES [SeasonAllocation] ([Id])')





