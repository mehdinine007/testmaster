if not exists(select 1 from sysObjects where Upper(Name)= 'OperatorEnumReadOnly' ) begin
	exec(N'CREATE TABLE [OperatorEnumReadOnly] (
		[Id] int NOT NULL IDENTITY,
		[Title_En] nvarchar(250) NULL,
		[Title] nvarchar(250) NULL,
		[Code] int NOT NULL,
		CONSTRAINT [PK_OperatorEnumReadOnly] PRIMARY KEY ([Id])
	)')
end
if not exists(select 1 from sysObjects where Upper(Name)= 'QuestionRelationship' ) begin
	exec(N'CREATE TABLE [QuestionRelationship] (
		[Id] int NOT NULL IDENTITY,
		[QuestionRelationId] int NOT NULL,
		[OperationType] int NOT NULL,
		[QuestionAnswerId] bigint NOT NULL,
		[QuestionId] int NOT NULL,
		[CreationTime] datetime2 NOT NULL,
		[CreatorId] uniqueidentifier NULL,
		[LastModificationTime] datetime2 NULL,
		[LastModifierId] uniqueidentifier NULL,
		[IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit),
		[DeleterId] uniqueidentifier NULL,
		[DeletionTime] datetime2 NULL,
		CONSTRAINT [PK_QuestionRelationship] PRIMARY KEY ([Id]),
		CONSTRAINT [FK_QuestionRelationship_QuestionAnswer_QuestionAnswerId] FOREIGN KEY ([QuestionAnswerId]) REFERENCES [QuestionAnswer] ([Id]) ON DELETE CASCADE,
		CONSTRAINT [FK_QuestionRelationship_Question_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [Question] ([Id])
	)')
end
SET IDENTITY_INSERT [OperatorEnumReadOnly] ON
INSERT INTO [OperatorEnumReadOnly] ([Id], [Code], [Title], [Title_En])
VALUES (1, 1, N'Equal', N'Equal'),
(2, 2, N'EqualOpposite', N'EqualOpposite'),
(3, 3, N'Bigger', N'Bigger'),
(4, 4, N'Smaller', N'Smaller'),
(5, 5, N'Like', N'Like'),
(6, 6, N'StartWith', N'StartWith'),
(7, 7, N'EndWith', N'EndWith');
SET IDENTITY_INSERT [OperatorEnumReadOnly] OFF;
