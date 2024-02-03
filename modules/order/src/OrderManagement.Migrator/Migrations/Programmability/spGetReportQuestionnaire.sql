CREATE PROCEDURE [dbo].[spGetReportQuestionnaire]
    @userId nvarchar(36)  null,
    @Type int,
	@maxResultCount int = 5000,
	@skipCount int
AS
BEGIN
    IF (@Type = 1)
    BEGIN

        SELECT SubmittedAnswer.Id
		,UnAuthorizedUser.Age
		,UnAuthorizedUser.EducationLevel
		,UnAuthorizedUser.FirstName
		,UnAuthorizedUser.gender
		,UnAuthorizedUser.LastName
		,UnAuthorizedUser.ManufactureDate
		,UnAuthorizedUser.MobileNumber
		,UnAuthorizedUser.NationalCode
		,UnAuthorizedUser.VehicleName
		,UnAuthorizedUser.Vin
		,Question.Title AS QuestionTitle,Questionnaire.Title AS  QuestionnaireTitle,
		Questionnaire.Id as QuestionnaireId  , ISNULL(SubmittedAnswer.CustomAnswerValue, '') AS CustomAnswerValue, QuestionAnswer.Description
        FROM SubmittedAnswer 
        INNER JOIN Question ON SubmittedAnswer.QuestionId = Question.id
        INNER JOIN QuestionAnswer ON SubmittedAnswer.QuestionAnswerId = QuestionAnswer.Id 
		inner join Questionnaire on Question.QuestionnaireId=Questionnaire.Id
		inner join UnAuthorizedUser on UnAuthorizedUser.QuestionnaireId=Questionnaire.Id
        WHERE UserId = TRY_CAST(@userId AS uniqueidentifier) and Questionnaire.QuestionnaireType=1
    END

    IF (@Type = 2)
    BEGIN
    	declare @cnt  int = 0;
		set @cnt = (@skipCount - 1) * 5000;

        SELECT SubmittedAnswer.Id
		,UnAuthorizedUser.Age
		,UnAuthorizedUser.EducationLevel
		,UnAuthorizedUser.FirstName
		,UnAuthorizedUser.gender
		,UnAuthorizedUser.LastName
		,UnAuthorizedUser.ManufactureDate
		,UnAuthorizedUser.MobileNumber
		,UnAuthorizedUser.NationalCode
		,UnAuthorizedUser.VehicleName
		,UnAuthorizedUser.Vin
		,Question.Title AS QuestionTitle,Questionnaire.Title AS  QuestionnaireTitle,Questionnaire.Id as QuestionnaireId , ISNULL(SubmittedAnswer.CustomAnswerValue, '') AS CustomAnswerValue, QuestionAnswer.Description
        FROM SubmittedAnswer 
        INNER JOIN Question ON SubmittedAnswer.QuestionId = Question.id
		inner join Questionnaire on Question.QuestionnaireId=Questionnaire.Id
		inner join UnAuthorizedUser on UnAuthorizedUser.QuestionnaireId=Questionnaire.Id
        INNER JOIN QuestionAnswer ON SubmittedAnswer.QuestionAnswerId = QuestionAnswer.Id 
		INNER JOIN AbpUsers ON SubmittedAnswer.UserId=AbpUsers.UID
		where Questionnaire.QuestionnaireType=1
	    ORDER BY AbpUsers.Id
        OFFSET @cnt ROWS
        FETCH NEXT 5000 ROWS ONLY
  --       SELECT SubmittedAnswer.Id
		--,UnAuthorizedUser.Age
		--,UnAuthorizedUser.EducationLevel
		--,UnAuthorizedUser.FirstName
		--,UnAuthorizedUser.gender
		--,UnAuthorizedUser.LastName
		--,UnAuthorizedUser.ManufactureDate
		--,UnAuthorizedUser.MobileNumber
		--,UnAuthorizedUser.NationalCode
		--,UnAuthorizedUser.VehicleName
		--,UnAuthorizedUser.Vin
		--,Question.Title AS QuestionTitle,Questionnaire.Title AS  QuestionnaireTitle,
		--Questionnaire.Id as QuestionnaireId  , ISNULL(SubmittedAnswer.CustomAnswerValue, '') AS CustomAnswerValue, QuestionAnswer.Description
  --      FROM SubmittedAnswer 
  --      INNER JOIN Question ON SubmittedAnswer.QuestionId = Question.id
  --      INNER JOIN QuestionAnswer ON SubmittedAnswer.QuestionAnswerId = QuestionAnswer.Id 
		--inner join Questionnaire on Question.QuestionnaireId=Questionnaire.Id
		--inner join UnAuthorizedUser on UnAuthorizedUser.QuestionnaireId=Questionnaire.Id
  --      WHERE  Questionnaire.QuestionnaireType=1
		--ORDER BY Question.Id
  --      OFFSET (@skipCount - 1) * @maxResultCount ROWS
  --      FETCH NEXT @maxResultCount ROWS ONLY
    END
END
