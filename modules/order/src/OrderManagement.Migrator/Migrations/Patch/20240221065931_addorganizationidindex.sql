if not exists(select 1 from sys.indexes where Upper(Name)= 'IX_ProductAndCategory_OrganizationId')
  exec(N'CREATE INDEX [IX_ProductAndCategory_OrganizationId] ON [ProductAndCategory] ([OrganizationId])');

exec(N'ALTER TABLE [ProductAndCategory] ADD CONSTRAINT [FK_ProductAndCategory_Organization_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organization] ([Id]) ON DELETE CASCADE');
