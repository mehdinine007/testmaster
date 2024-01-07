if not exists(select 1 from sysObjects where Upper(Name)= 'CustomerPriority' ) begin
	exec(N'CREATE TABLE [CustomerPriority] (
		[Id] bigint NOT NULL IDENTITY,
		[Uid] uniqueidentifier NOT NULL,
		[ApproximatePriority] int NOT NULL,
		[SaleId] int NOT NULL,
		[ChosenPriorityByCustomer] int NOT NULL,
		[CreationTime] datetime2 NOT NULL,
		[CreatorId] uniqueidentifier NULL,
		[LastModificationTime] datetime2 NULL,
		[LastModifierId] uniqueidentifier NULL,
		[IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit),
		[DeleterId] uniqueidentifier NULL,
		[DeletionTime] datetime2 NULL,
		CONSTRAINT [PK_CustomerPriority] PRIMARY KEY ([Id])
	)')
end