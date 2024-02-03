CREATE TABLE [QuestionGroup] (
    [Id] int NOT NULL IDENTITY,
    [Code] int NOT NULL,
    [Title] nvarchar(50) NULL,
    [QuestionnaireId] int NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorId] uniqueidentifier NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierId] uniqueidentifier NULL,
    [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit),
    [DeleterId] uniqueidentifier NULL,
    [DeletionTime] datetime2 NULL,
    CONSTRAINT [PK_QuestionGroup] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_QuestionGroup_Questionnaire_QuestionnaireId] FOREIGN KEY ([QuestionnaireId]) REFERENCES [Questionnaire] ([Id])
)
