exec(N'ALTER TABLE [Question] ADD [QuestionGroupId] int NULL');

exec(N'ALTER TABLE [Question] ADD CONSTRAINT [FK_Question_QuestionGroup_QuestionGroupId] FOREIGN KEY ([QuestionGroupId]) REFERENCES [QuestionGroup] ([Id])')

