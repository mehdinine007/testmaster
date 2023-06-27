﻿using Microsoft.AspNetCore.Server.IIS.Core;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class QuestionniareService : ApplicationService, IQuestionniareService
    {
        private readonly IRepository<Questionnaire, int> _questionnaireRepository;
        private readonly IRepository<QuestionnaireAnswer, int> _questionnaireAnswerRepository;
        private readonly IRepository<AnswerComponentType, int> _answerComponentType;
        private readonly IRepository<SubmittedAnswers, int> _submitedAnswerRepository;
        private readonly ICommonAppService _commonAppService;

        public QuestionniareService(IRepository<Questionnaire, int> questionnaireRepository,
                                    IRepository<AnswerComponentType, int> answerComponentType,
                                    IRepository<QuestionnaireAnswer, int> questionnaireAnswerRepository,
                                    IRepository<SubmittedAnswers, int> submitedAnswerRepository,
                                    ICommonAppService commonAppService
            )
        {
            _questionnaireRepository = questionnaireRepository;
            _answerComponentType = answerComponentType;
            _questionnaireAnswerRepository = questionnaireAnswerRepository;
            _submitedAnswerRepository = submitedAnswerRepository;
            _commonAppService = commonAppService;
        }

        public async Task<QuestionnaireDto> CreateQuestionnaire(QuestionnaireDto questionnaireDto)
        {
            var questionnaire = await _questionnaireRepository.InsertAsync(ObjectMapper.Map<QuestionnaireDto, Questionnaire>(questionnaireDto));
            return ObjectMapper.Map<Questionnaire, QuestionnaireDto>(questionnaire);
        }

        public async Task CreatQuestionnaireAnswers(List<QuestionnaireAnswerDto> questionnaireAnswers)
        {
            var questionnaireQuery = await _questionnaireRepository.GetQueryableAsync();
            var questionnaireIds = questionnaireAnswers.Select(x => x.QuestionnaireId).ToList();
            //if (questionnaireQuery.Any(x => questionnaireIds.Any(y => y != x.Id)))
            //    throw new UserFriendlyException("مغایرت در اطلاعات ورودی");

            await _questionnaireAnswerRepository.InsertManyAsync(
                ObjectMapper.Map<List<QuestionnaireAnswerDto>, List<QuestionnaireAnswer>>(questionnaireAnswers));
        }

        public async Task<List<AnswerComponentTypeDto>> GetAnswerComponentTypes()
        {
            var answerComponentTypeQuery = await _answerComponentType.GetQueryableAsync();
            return ObjectMapper.Map<List<AnswerComponentType>, List<AnswerComponentTypeDto>>(answerComponentTypeQuery.ToList());
        }

        private async Task<QuestionnaireTree> GetQuestionnaireTree(int questionnaireId)
        {
            var submitedAnswers = _submitedAnswerRepository.WithDetails(x => x.QuestionnaireAnswer)
                .Where(x => x.QuestionnaireAnswer.QuestionnaireId == questionnaireId && x.UserId == _commonAppService.GetUserId())
                .ToList();

            var questionnaire = _questionnaireRepository.WithDetails(x => x.QuestionnaireAnswers)
                .FirstOrDefault(x => x.Id == questionnaireId) ??
                throw new UserFriendlyException("پرسشنامه پیدا نشد");

            var questionnaireTree = new QuestionnaireTree()
            {
                Questionnaire = ObjectMapper.Map<Questionnaire, QuestionnaireDto>(questionnaire),
                QuestionnaireAnswers = ObjectMapper.Map<List<QuestionnaireAnswer>, List<QuestionnaireAnswerDto>>(questionnaire.QuestionnaireAnswers.ToList()),
                SubmitedAnswerIds = submitedAnswers.Select(x => x.Id).ToList()
            };

            return questionnaireTree;
        }

        public async Task<List<QuestionnaireTree>> GetQuestionnaireTrees()
        {
            var questionnaire = (await _questionnaireRepository.GetQueryableAsync()).ToList();
            var questionnaireList = new List<QuestionnaireTree>(questionnaire.Capacity);
            questionnaire.ForEach(async x => questionnaireList.Add(await GetQuestionnaireTree(x.Id)));
            return questionnaireList;
        }

        public async Task<SubmitteAnswerDto> SubmitAnswer(SubmitteAnswerDto submitteAnswerDto)
        {
            var userId = _commonAppService.GetUserId();
            var submittedAnswerQuery = await _submitedAnswerRepository.GetQueryableAsync();
            var answer = _questionnaireAnswerRepository.WithDetails(x => x.Questionnaire)
                .FirstOrDefault(x => x.Id == submitteAnswerDto.AnswerId)
                ?? throw new UserFriendlyException("جواب مورد نظر پیدا نشد");
            var questionId = answer.QuestionnaireId;
            var questionnaire = _questionnaireRepository.WithDetails(x => x.QuestionnaireAnswers).FirstOrDefault(x => x.Id == questionId)
                ?? throw new UserFriendlyException("سوال مورد نظر پیدا نشد");
            var answerIds = questionnaire.QuestionnaireAnswers.Select(x => x.Id).ToList();
            submittedAnswerQuery = submittedAnswerQuery.Where(x => x.UserId == userId && answerIds.Any(y => y == x.AnswerId));
            if (submittedAnswerQuery.FirstOrDefault() != null)
                throw new UserFriendlyException("شما قبلا به این سوال جواب داده اید");

            var submittedAnswer = await _submitedAnswerRepository.InsertAsync(
                new SubmittedAnswers()
                {
                    UserId = userId,
                    AnswerId = submitteAnswerDto.AnswerId,
                    AnswerDescription = submitteAnswerDto.AnswerDescription,
                });
            return ObjectMapper.Map<SubmittedAnswers, SubmitteAnswerDto>(submittedAnswer);
        }
    }
}
