﻿using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts.Dtos;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.Shared;
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

    public QuestionnaireService(IRepository<Questionnaire, int> questionnaireRepository,
                                IBaseInformationService baseInformationService,
                                ICommonAppService commonAppService
        )
    {
        _questionnaireRepository = questionnaireRepository;
        _baseInformationService = baseInformationService;
        _commonAppService = commonAppService;
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
        //questionnaireQuery = questionnaireQuery.Include(x => x.Questions)
        //    .ThenInclude(x => x.SubmittedAnswers);
        var qeustionnaireQueryResult = questionnaireQuery.FirstOrDefault(x => x.Id == questionnaireId);
        //var submittedAnswers = await 
        throw new System.NotImplementedException();
    }

    public Task SubmitAnswer(List<SubmitAnswerDto> submitAnswerDtos)
    {
        throw new System.NotImplementedException();
    }
}
