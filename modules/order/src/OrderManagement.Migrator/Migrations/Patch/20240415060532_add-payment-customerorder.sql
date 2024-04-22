exec (N'ALTER TABLE [CustomerOrder] ADD [PaymentPrice] bigint  NULL');
exec (N'ALTER TABLE [CustomerOrder] ADD [TransactionCommitDate] datetime2 NULL');
exec (N'ALTER TABLE [CustomerOrder] ADD [TransactionId] nvarchar(max) NULL');