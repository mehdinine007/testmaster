﻿using EasyCaching.Core;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Dtos;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Application.OrderManagement.Constants;
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
    private readonly IHybridCachingProvider _hybridCachingProvider;
    private readonly IRepository<Question, int> _questionRepository;
    private readonly IRepository<QuestionAnswer, long> _answerRepository;

    public QuestionnaireService(IRepository<Questionnaire, int> questionnaireRepository,
                                IBaseInformationService baseInformationService,
                                ICommonAppService commonAppService,
                                IRepository<SubmittedAnswer, long> submittedAnswerRepository,
                                IHybridCachingProvider hybridCachingProvider,
                                IRepository<Question, int> questionRepository,
                                IRepository<QuestionAnswer, long> answerRepository
        )
    {
        _questionnaireRepository = questionnaireRepository;
        _baseInformationService = baseInformationService;
        _commonAppService = commonAppService;
        _submittedAnswerRepository = submittedAnswerRepository;
        _hybridCachingProvider = hybridCachingProvider;
        _questionRepository = questionRepository;
        _answerRepository = answerRepository;
    }

    public async Task<QuestionnaireTreeDto> LoadQuestionnaireTree(int questionnaireId)
    {
        var questionnaireWhitListType = (await _questionnaireRepository.GetQueryableAsync())
            .Select(x => new
            {
                x.Id,
                x.WhitListRequirement
            })
            .FirstOrDefault(x => x.Id == questionnaireId)
            ?? throw new UserFriendlyException("پرسشنامه مورد نظر پیدا نشد");

        var currentUserId = _commonAppService.GetUserId();
        var currentUserNationalCode = _commonAppService.GetNationalCode();

        if (questionnaireWhitListType.WhitListRequirement.HasValue)
        {
            var requiredWhitListType = (WhiteListEnumType)questionnaireWhitListType.WhitListRequirement.Value;
            _baseInformationService.CheckWhiteList(requiredWhitListType, currentUserNationalCode);
        }
        var questionnaireQuery = await _questionnaireRepository.GetQueryableAsync();
        questionnaireQuery = questionnaireQuery.Include(x => x.Questions)
            .ThenInclude(x => x.Answers);
        questionnaireQuery = questionnaireQuery.Include(x => x.Questions)
            .ThenInclude(x => x.SubmittedAnswers.Where(y => y.UserId == currentUserId));

        var questionnaireResult = questionnaireQuery.FirstOrDefault(x => x.Id == questionnaireId)
            ?? throw new UserFriendlyException("پرسشنامه مورد نظر یافت نشد");
        var resultDto = ObjectMapper.Map<Questionnaire, QuestionnaireTreeDto>(questionnaireResult);
        resultDto.QuestionnaireAnalysis = await GetQuestionnaireReport(questionnaireId);
        return resultDto;
    }

    public async Task<List<QuestionnaireAnalysisDto>> GetQuestionnaireReport(int questionnaireId)
    {
        var questionnaire = (await _questionnaireRepository.GetQueryableAsync())
            .Select(x => new { x.Id })
            .FirstOrDefault(x => x.Id == questionnaireId);
        if (questionnaire == null)
            throw new UserFriendlyException("پرسشنامه پیدا نشد");
        var cacheKey = string.Format(RedisConstants.QuestionnaireSurveyReport, questionnaireId);
        var result = await _hybridCachingProvider.GetAsync<List<QuestionnaireAnalysisDto>>(cacheKey);
        if (result.HasValue)
            return result.Value;

        var ls = new List<int>() { (int)QuestionType.Optional, (int)QuestionType.Range };
        var questions = (await _questionRepository.GetQueryableAsync())
            .Where(x => x.QuestionnaireId == questionnaireId && ls.Any(y => (int)x.QuestionType == y));
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
            })
            .Where(x => questionIds.Any(y => y == x.QuestionId))
            .GroupBy(x => x.QuestionId)
            .Select(x => new QuestionnaireAnalysisDto
            {
                Rate = x.Average(y => y.Value),
                QuestionId = x.Key
            })
            .ToList();

        await _hybridCachingProvider.SetAsync(cacheKey, surveyReport, new TimeSpan(4, 0, 0));
        return surveyReport;
    }

    public async Task SubmitAnswer(SubmitAnswerTreeDto submitAnswerTreeDto)
    {
        var questionnaire = await LoadQuestionnaireTree(submitAnswerTreeDto.QuestionnaireId);
        var currentUserUserId = _commonAppService.GetUserId();
        //check quesionnaire has not beeing completed by user
        //var answerSubmitted = questionnaire.Questions.Any(x => x.SubmittedAnswers.Any());
        var answerSubmitted = (await _submittedAnswerRepository.GetQueryableAsync())
            .Include(x => x.Question)
            .Select(x => new
            {
                x.UserId,
                x.Question.QuestionnaireId
            })
            .FirstOrDefault(x => x.UserId == currentUserUserId && x.QuestionnaireId == submitAnswerTreeDto.QuestionnaireId);
        if (answerSubmitted != null)
            throw new UserFriendlyException("این پرسشنامه قبلا توسط شما تکمیل شده است");

        //check all available questions in questionnaire being completed
        var questionIds = questionnaire.Questions.Select(x => x.Id).ToList();
        var missedQuestionExists = submitAnswerTreeDto.SubmitAnswerDto.Any(x => !questionIds.Any(y => y == x.QuestionId));
        if (missedQuestionExists)
            throw new UserFriendlyException("لطفا به تمام سوالات پاسخ دهید");

        //submit answer base on question type
        var qeustionAnswers = questionnaire.Questions.SelectMany(x => x.Answers).ToList();
        List<SubmittedAnswer> submitAnswerList = new(questionIds.Count);
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
                        UserId = currentUserUserId,
                        QuestionId = x.QuestionId,
                        QuestionAnswerId = x.QuestionAnswerId
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
                        UserId = currentUserUserId
                    }).ToList());
                    break;
                case QuestionType.Descriptional:
                    if (submitAnswerList.Any(y => y.QuestionId == x.QuestionId))
                        throw new UserFriendlyException("لطفا به سوالات تشریحی بیش از یک پاسخ ندهید");
                    submitAnswerList.Add(new SubmittedAnswer
                    {
                        CustomAnswerValue = x.CustomAnswerValue,
                        QuestionId = x.QuestionId,
                        UserId = currentUserUserId
                    });
                    break;
                default:
                    break;
            }
        });
        //add to database
        await _submittedAnswerRepository.InsertManyAsync(submitAnswerList);
    }
}
