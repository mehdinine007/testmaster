﻿using EasyCaching.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Dtos;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class QuestionnaireService : ApplicationService, IQuestionnaireService
{
    private readonly IRepository<Questionnaire, int> _questionnaireRepository;
    private readonly IBaseInformationService _baseInformationService;
    private readonly ICommonAppService _commonAppService;
    private readonly IRepository<SubmittedAnswer, long> _submittedAnswerRepository;
    private readonly IRepository<Question, int> _questionRepository;
    private readonly IRepository<QuestionAnswer, long> _answerRepository;
    private readonly IAttachmentService _attachmentService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<UnAuthorizedUser, long> _unAuthorizedUserRepository;

    public QuestionnaireService(IRepository<Questionnaire, int> questionnaireRepository,
                                IBaseInformationService baseInformationService,
                                ICommonAppService commonAppService,
                                IRepository<SubmittedAnswer, long> submittedAnswerRepository,
                                IRepository<Question, int> questionRepository,
                                IRepository<QuestionAnswer, long> answerRepository,
                                IAttachmentService attachmentService,
                                IHttpContextAccessor httpContextAccessor,
                                IRepository<UnAuthorizedUser, long> unAuthorizedUserRepository
        )
    {
        _questionnaireRepository = questionnaireRepository;
        _baseInformationService = baseInformationService;
        _commonAppService = commonAppService;
        _submittedAnswerRepository = submittedAnswerRepository;
        _questionRepository = questionRepository;
        _answerRepository = answerRepository;
        _attachmentService = attachmentService;
        _httpContextAccessor = httpContextAccessor;
        _unAuthorizedUserRepository = unAuthorizedUserRepository;
    }

    //[SecuredOperation(QuestionnaireServicePermissionconstants.LoadQuestionnaireTree)]
    public async Task<QuestionnaireTreeDto> LoadQuestionnaireTree(int questionnaireId, long? relatedEntityId = null)
    {
        var questionnaireWhitListType = (await _questionnaireRepository.GetQueryableAsync())
            .Select(x => new
            {
                x.Id,
                x.WhitListRequirement,
                x.QuestionnaireType
            })
            .FirstOrDefault(x => x.Id == questionnaireId)
            ?? throw new UserFriendlyException("پرسشنامه مورد نظر پیدا نشد");
        var currentUserId = _commonAppService.SoftGetUserId();
        if (questionnaireWhitListType.QuestionnaireType == QuestionnaireType.AuthorizedOnly || questionnaireWhitListType.WhitListRequirement.HasValue)
            if (!currentUserId.HasValue)
                throw new UserFriendlyException("لطفا لاگین کنید");

        var currentUserNationalCode = questionnaireWhitListType.WhitListRequirement.HasValue ? _commonAppService.GetNationalCode() : string.Empty;

        if (questionnaireWhitListType.WhitListRequirement.HasValue)
            _baseInformationService.CheckWhiteList((WhiteListEnumType)questionnaireWhitListType.WhitListRequirement.Value, currentUserNationalCode);

        var questionnaireQuery =( await _questionnaireRepository.GetQueryableAsync()).AsNoTracking();
        questionnaireQuery = questionnaireQuery.Include(x => x.Questions)
            .ThenInclude(x => x.Answers)
            .Include(x => x.Questions).ThenInclude(x => x.QuestionGroup)
            .Include(x => x.Questions).ThenInclude(x => x.QuestionRelationships)
            .ThenInclude(x => x.QuestionAnswer);
        if (currentUserId.HasValue)
        {
            if (!relatedEntityId.HasValue)
            {
                questionnaireQuery = questionnaireQuery.Include(x => x.Questions)
                    .ThenInclude(x => x.SubmittedAnswers.Where(y => y.UserId.Value == currentUserId.Value))
                     .Include(x => x.Questions).ThenInclude(x => x.QuestionRelationships)
                    .ThenInclude(x => x.QuestionAnswer)
                    .ThenInclude(x => x.SubmittedAnswers.Where(y => y.UserId.Value == currentUserId.Value));
                  
            }
            else
            {

                questionnaireQuery = questionnaireQuery.Include(x => x.Questions)
                    .ThenInclude(x => x.SubmittedAnswers.Where(y => y.UserId.Value == currentUserId.Value && y.RelatedEntityId.Value == relatedEntityId.Value))
                    .Include(x => x.Questions).ThenInclude(x => x.QuestionRelationships)
                    .ThenInclude(x => x.QuestionAnswer)
                    .ThenInclude(x => x.SubmittedAnswers.Where(y => y.UserId.Value == currentUserId.Value && y.RelatedEntityId.Value == relatedEntityId.Value));
                    
            }
        }

        var questionnaireResult = questionnaireQuery.FirstOrDefault(x => x.Id == questionnaireId)
            ?? throw new UserFriendlyException("پرسشنامه مورد نظر یافت نشد");
        var resultDto = ObjectMapper.Map<Questionnaire, QuestionnaireTreeDto>(questionnaireResult);
        return resultDto;
    }

    public async Task<List<QuestionnaireAnalysisDto>> GetQuestionnaireReport(int questionnaireId, long? relatedEntityId)
    {
        var questionnaire = (await _questionnaireRepository.GetQueryableAsync())
            .Select(x => new { x.Id })
            .FirstOrDefault(x => x.Id == questionnaireId);
        if (questionnaire == null)
            throw new UserFriendlyException("پرسشنامه پیدا نشد");
        var cacheKey = relatedEntityId.HasValue
            ? string.Format(RedisConstants.QuestionnaireSurveyReportWithRelatedEntity, questionnaireId, relatedEntityId.Value)
            : string.Format(RedisConstants.QuestionnaireSurveyReport, questionnaireId);
        //  var result = await _hybridCachingProvider.GetAsync<List<QuestionnaireAnalysisDto>>(cacheKey);
        //   if (result.HasValue)
        //       return result.Value;

        var ls = new List<int>() { (int)QuestionType.Optional, (int)QuestionType.Range };
        var questions = (await _questionRepository.GetQueryableAsync())
            .Where(x => x.QuestionnaireId == questionnaireId && ls.Any(y => (int)x.QuestionType == y))
            .Select(x => new { x.Id, x.Title })
            .ToList();
        var questionIds = questions.Select(x => x.Id).ToList();
        var answersQuery = (await _answerRepository.GetQueryableAsync())
            .Where(x => questionIds.Any(y => y == x.QuestionId));

        var surveyReport = (await _submittedAnswerRepository.GetQueryableAsync()).Join(answersQuery,
            x => x.QuestionAnswerId,
            x => x.Id,
            (y, x) => new
            {
                x.QuestionId,
                x.Value,
                QuestionAnswerId = x.Id,
                SubmittedAnswerId = y.Id,
                y.RelatedEntityId
            })
            .Where(x => questionIds.Any(y => y == x.QuestionId) && x.RelatedEntityId == relatedEntityId)
            .GroupBy(x => x.QuestionId)
            .Select(x => new QuestionnaireAnalysisDto
            {
                Rate = x.Average(y => y.Value),
                QuestionId = x.Key
            })
            .ToList();

        surveyReport.ForEach(x =>
        {
            var question = questions.FirstOrDefault(y => y.Id == x.QuestionId);
            if (question != null)
                x.QuestionTitle = question.Title;
        });

        //   await _hybridCachingProvider.SetAsync(cacheKey, surveyReport, new TimeSpan(4, 0, 0));
        return surveyReport;
    }

    //[SecuredOperation(QuestionnaireServicePermissionconstants.SubmitAnswer)]
    public async Task SubmitAnswer(SubmitAnswerTreeDto submitAnswerTreeDto)
    {
        if (submitAnswerTreeDto.SubmitAnswerDto == null || !submitAnswerTreeDto.SubmitAnswerDto.Any())
            throw new UserFriendlyException("به هیچ سوالی پاسخ داده نشده است");
        var questionnaire = await LoadQuestionnaireTree(submitAnswerTreeDto.QuestionnaireId);
        if (!questionnaire.Questions.Any())
            throw new UserFriendlyException("برای این پرسشنامه سوالی تعریف نشده است");

        if (questionnaire.QuestionnaireType == QuestionnaireType.AnonymousAllowed)
        {
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {

                var smsCodeIsValid = await _commonAppService.ValidateSMS(submitAnswerTreeDto?.UnregisteredUserInformation?.MobileNumber,
                    submitAnswerTreeDto?.UnregisteredUserInformation?.NationalCode,
                    submitAnswerTreeDto?.UnregisteredUserInformation?.SmsCode,
                    SMSType.AnonymousQuestionnaireSubmitation);
                if (!smsCodeIsValid)
                    throw new UserFriendlyException("کد ارسالی صحیح نیست");
            }
            //else
            //{
            //    await ControlUserWotnSpamAnswer(_commonAppService.GetUserId(), submitAnswerTreeDto.QuestionnaireId, submitAnswerTreeDto.RelatedEntity);
            //}
            if (!string.IsNullOrWhiteSpace(submitAnswerTreeDto?.UnregisteredUserInformation?.Vin))
                await ControlAnonymousUserWontSpamAnswer(submitAnswerTreeDto.UnregisteredUserInformation.Vin, questionnaire.Id);

            if (string.IsNullOrWhiteSpace(submitAnswerTreeDto?.UnregisteredUserInformation?.Vin) &&
                string.IsNullOrWhiteSpace(submitAnswerTreeDto?.UnregisteredUserInformation?.FirstName) &&
                string.IsNullOrWhiteSpace(submitAnswerTreeDto?.UnregisteredUserInformation?.ManufactureDate) &&
                string.IsNullOrWhiteSpace(submitAnswerTreeDto?.UnregisteredUserInformation?.LastName) &&
                string.IsNullOrWhiteSpace(submitAnswerTreeDto?.UnregisteredUserInformation?.VehicleName) &&
                string.IsNullOrWhiteSpace(submitAnswerTreeDto?.UnregisteredUserInformation?.MobileNumber) &&
                string.IsNullOrWhiteSpace(submitAnswerTreeDto?.UnregisteredUserInformation?.SmsCode) &&
                string.IsNullOrWhiteSpace(submitAnswerTreeDto?.UnregisteredUserInformation?.EducationLevel) &&
                string.IsNullOrWhiteSpace(submitAnswerTreeDto?.UnregisteredUserInformation?.Occupation) &&
                string.IsNullOrWhiteSpace(submitAnswerTreeDto?.UnregisteredUserInformation?.NationalCode))
                throw new UserFriendlyException("لطفا نمام فیلد ها را پر کنید");

            submitAnswerTreeDto.UnregisteredUserInformation.QuestionnaireId = questionnaire.Id;
        }
        else if (questionnaire.QuestionnaireType == QuestionnaireType.AuthorizedOnly)
        {
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                throw new UserFriendlyException("لطفا لاگین کنید");

            await ControlUserWotnSpamAnswer(_commonAppService.GetUserId(), submitAnswerTreeDto.QuestionnaireId, submitAnswerTreeDto.RelatedEntity);
        }
        else
            throw new InvalidOperationException();


        //check all available questions in questionnaire being completed
        var questionIds = questionnaire.Questions.Select(x => x.Id).OrderBy(x => x).ToList();
        var noDescriptionalquestionId = questionnaire.Questions.Where(x => x.QuestionType != QuestionType.Descriptional).Select(x => x.Id).OrderBy(x => x).ToList();
        var incomigAnswerIds = submitAnswerTreeDto.SubmitAnswerDto.Where(x => noDescriptionalquestionId.Contains(x.QuestionId)).Select(x => x.QuestionId).OrderBy(x => x).ToList();
        var missedQuestionExists = !noDescriptionalquestionId.SequenceEqual(incomigAnswerIds);
        if (missedQuestionExists)
            throw new UserFriendlyException("لطفا به تمام سوالات پاسخ دهید");

        //submit answer base on question type
        var qeustionAnswers = questionnaire.Questions.SelectMany(x => x.Answers).ToList();
        List<SubmittedAnswer> submitAnswerList = new(questionIds.Count);
        var userAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        Guid? currrentUserId = userAuthenticated ? _commonAppService.GetUserId() : null;
        submitAnswerTreeDto.SubmitAnswerDto.ForEach(x =>
        {
            var question = questionnaire.Questions.First(y => y.Id == x.QuestionId);
            switch (question.QuestionType)
            {
                case QuestionType.Optional:
                case QuestionType.Range:
                    if (!question.Answers.Any(y => y.Id == x.QuestionAnswerId))
                        throw new UserFriendlyException($"پاسخ انتخاب شده مربوط به این سوال نمیباشد : {question.Title}");
                    if (submitAnswerList.Any(y => y.QuestionId == x.QuestionId))
                        throw new UserFriendlyException($"لطفا برای این سوال یک گزینه را انتخاب کنید : {question.Title}");
                    submitAnswerList.Add(new SubmittedAnswer()
                    {
                        UserId = questionnaire.QuestionnaireType == QuestionnaireType.AuthorizedOnly && userAuthenticated ? currrentUserId.Value : null,
                        QuestionId = x.QuestionId,
                        QuestionAnswerId = x.QuestionAnswerId,
                        RelatedEntityId = submitAnswerTreeDto.RelatedEntity.HasValue
                            ? submitAnswerTreeDto.RelatedEntity.Value
                            : null
                    });
                    break;
                case QuestionType.MultiSelectOptional:
                    var answerIds = x.CustomAnswerValue.Split(',').Select(y => int.Parse(y)).ToList();
                    if (!question.Answers.Any(questionAnswer => !answerIds.Any(incomigAnswerId => incomigAnswerId == questionAnswer.Id)))
                        throw new UserFriendlyException($"پاسخ انتخاب شده مربوط به این سوال نمیباشد : {question.Title}");
                    var selectedAnswers = question.Answers.Where(y => answerIds.Any(z => z == y.Id)).ToList();
                    submitAnswerList.AddRange(selectedAnswers.Select(y => new SubmittedAnswer()
                    {
                        QuestionAnswerId = y.Id,
                        QuestionId = x.QuestionId,
                        UserId = questionnaire.QuestionnaireType == QuestionnaireType.AuthorizedOnly && userAuthenticated ? currrentUserId.Value : null,
                        RelatedEntityId = submitAnswerTreeDto.RelatedEntity.HasValue
                            ? submitAnswerTreeDto.RelatedEntity.Value
                            : null
                    }).ToList());
                    break;
                case QuestionType.Descriptional:
                    if (submitAnswerList.Any(y => y.QuestionId == x.QuestionId))
                        throw new UserFriendlyException("لطفا به سوالات تشریحی بیش از یک پاسخ ندهید");
                    submitAnswerList.Add(new SubmittedAnswer
                    {
                        CustomAnswerValue = x.CustomAnswerValue,
                        QuestionId = x.QuestionId,
                        UserId = questionnaire.QuestionnaireType == QuestionnaireType.AuthorizedOnly && userAuthenticated ? currrentUserId.Value : null,
                        RelatedEntityId = submitAnswerTreeDto.RelatedEntity.HasValue
                            ? submitAnswerTreeDto.RelatedEntity.Value
                            : null
                    });
                    break;
                default:
                    break;
            }
        });
        //add to database
        await _submittedAnswerRepository.InsertManyAsync(submitAnswerList);
        if (questionnaire.QuestionnaireType == QuestionnaireType.AnonymousAllowed)
        {
            await _unAuthorizedUserRepository.InsertAsync(
             ObjectMapper.Map<UnregisteredUserInformation, UnAuthorizedUser>(submitAnswerTreeDto.UnregisteredUserInformation));
        }
    }

    public async Task<bool> UploadFile(UploadFileDto uploadFile)
    {
        var questionnaire = (await _questionnaireRepository.GetQueryableAsync())
            .Select(x => new { x.Id })
            .Where(x => x.Id == uploadFile.Id)
            ?? throw new UserFriendlyException("پرسشنامه یافت نشد");
        await _attachmentService.UploadFile(AttachmentEntityEnum.Questionnaire, uploadFile);
        return true;
    }

    public async Task<List<QuestionnaireDto>> LoadQuestionnaireList(List<AttachmentEntityTypeEnum> attachmentEntityTypeEnums = null, List<AttachmentLocationEnum> attachmentlocation = null)
    {
        var questionnaireList = (await _questionnaireRepository.GetQueryableAsync()).Where(x => x.Id != 0).ToList();
        var questionnaireIds = questionnaireList.Select(x => x.Id).ToList();
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Questionnaire, questionnaireIds, attachmentEntityTypeEnums, attachmentlocation);
        var questionnaireDtoList = ObjectMapper.Map<List<Questionnaire>, List<QuestionnaireDto>>(questionnaireList);
        questionnaireDtoList.ForEach(x =>
        {
            var crrentAttachments = attachments.Where(y => y.EntityId == x.Id).ToList();
            if (crrentAttachments.Any())
                x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(crrentAttachments);
        });
        return questionnaireDtoList;
    }


    #region Utilities
    private async Task ControlUserWotnSpamAnswer(Guid userId, int questionnaireId, long? relatedEntity)
    {
        var answerSubmittedQuery = (await _submittedAnswerRepository.GetQueryableAsync())
            .Include(x => x.Question)
            .Select(x => new
            {
                x.UserId,
                x.Question.QuestionnaireId,
                x.RelatedEntityId
            })
            .Where(x => x.UserId.Value == userId && x.QuestionnaireId == questionnaireId);

        var submittedAnswer = relatedEntity.HasValue
            ? answerSubmittedQuery.FirstOrDefault(x => x.RelatedEntityId.Value == relatedEntity.Value)
            : answerSubmittedQuery.FirstOrDefault();

        if (submittedAnswer != null)
            throw new UserFriendlyException("این پرسشنامه قبلا توسط شما تکمیل شده است");
    }

    private async Task ControlAnonymousUserWontSpamAnswer(string vin, int questionnaireId, long? relatedEntity = null)
    {
        var questionnaire = (await _questionnaireRepository
            .GetQueryableAsync())
            .Include(x => x.UnAuthorizedUsers)
            .FirstOrDefault(x => x.Id == questionnaireId)
            ?? throw new UserFriendlyException("پرسشنامه یافت نشد");
        if (questionnaire.QuestionnaireType == QuestionnaireType.AuthorizedOnly)
            throw new InvalidOperationException();

        var answerForVin = questionnaire.UnAuthorizedUsers.FirstOrDefault(x => x.Vin == vin);
        if (answerForVin != null)
            throw new UserFriendlyException("قبلا برای این VIN نظر ثبت شده است");
    }

    #endregion
}
