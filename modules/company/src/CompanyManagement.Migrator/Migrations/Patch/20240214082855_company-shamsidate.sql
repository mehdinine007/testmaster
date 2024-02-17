exec(N'ALTER TABLE [CompanyPaypaidPrices] ADD [TranDay] int NOT NULL DEFAULT 0');
exec(N'ALTER TABLE [CompanyPaypaidPrices] ADD [TranMonth] int NOT NULL DEFAULT 0');
exec(N'ALTER TABLE [CompanyPaypaidPrices] ADD [TranYear] int NOT NULL DEFAULT 0');
exec(N'ALTER TABLE [ClientsOrderDetailByCompany] ADD [CompanyId] int NOT NULL DEFAULT 0');
exec(N'ALTER TABLE [ClientsOrderDetailByCompany] ADD [DeliveryDay] int NOT NULL DEFAULT 0');
exec(N'ALTER TABLE [ClientsOrderDetailByCompany] ADD [DeliveryMonth] int NOT NULL DEFAULT 0');
exec(N'ALTER TABLE [ClientsOrderDetailByCompany] ADD [DeliveryYear] int NOT NULL DEFAULT 0');
exec(N'ALTER TABLE [ClientsOrderDetailByCompany] ADD [FactorDay] int NOT NULL DEFAULT 0');
exec(N'ALTER TABLE [ClientsOrderDetailByCompany] ADD [FactorMonth] int NOT NULL DEFAULT 0');
exec(N'ALTER TABLE [ClientsOrderDetailByCompany] ADD [FactorYear] int NOT NULL DEFAULT 0');
exec(N'ALTER TABLE [ClientsOrderDetailByCompany] ADD [IntroductionDay] int NOT NULL DEFAULT 0');
exec(N'ALTER TABLE [ClientsOrderDetailByCompany] ADD [IntroductionMonth] int NOT NULL DEFAULT 0');
exec(N'ALTER TABLE [ClientsOrderDetailByCompany] ADD [IntroductionYear] int NOT NULL DEFAULT 0');
exec(N'CREATE INDEX [IX_ClientsOrderDetailByCompany_CompanyId] ON [ClientsOrderDetailByCompany] ([CompanyId])');


