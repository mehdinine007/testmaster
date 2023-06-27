using Microsoft.AspNetCore.Server.IIS.Core;
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
using Volo.Abp.ObjectMapping;

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

        public async Task CreatQuestionnaireAnswers(CreateQuestionnaireAnswerDto questionnaireAnswers)
        {
            var questionnaireQuery = await _questionnaireRepository.GetQueryableAsync();
            var questionnaire = questionnaireQuery.FirstOrDefault(x => x.Id == questionnaireAnswers.QuestionnaireId)
                ?? throw new UserFriendlyException("پرسشنامه پیدا نشد");
            //TODO : add enum and replace this
            if (questionnaire.AnswerComponentId != 1)
                throw new UserFriendlyException("تنها برای سوال های گزینه ای امکان تعریف جواب وجود دارد");

            if (questionnaireAnswers.AnswerDescriptions.Any())
            {
                await _questionnaireAnswerRepository.InsertManyAsync(
                    questionnaireAnswers.AnswerDescriptions.Select(x => new QuestionnaireAnswer
                    {
                        Description = x,
                        QuestionnaireId = questionnaireAnswers.QuestionnaireId
                    }).ToList());
            }
        }

        public async Task<List<AnswerComponentTypeDto>> GetAnswerComponentTypes()
        {
            var answerComponentTypeQuery = await _answerComponentType.GetQueryableAsync();
            return ObjectMapper.Map<List<AnswerComponentType>, List<AnswerComponentTypeDto>>(answerComponentTypeQuery.ToList());
        }

        private async Task<QuestionnaireTree> GetQuestionnaireTree(int questionnaireId)
        {

            var questionnaire = _questionnaireRepository.WithDetails(x => x.QuestionnaireAnswers)
                .FirstOrDefault(x => x.Id == questionnaireId) ??
                throw new UserFriendlyException("پرسشنامه پیدا نشد");
            var submitedAnswer = _submitedAnswerRepository.WithDetails(x => x.QuestionnaireAnswer)
                .FirstOrDefault(x => x.QuestionnaireAnswer.QuestionnaireId == questionnaireId && x.UserId == _commonAppService.GetUserId());

            var questionnaireTree = new QuestionnaireTree()
            {
                Questionnaire = ObjectMapper.Map<Questionnaire, QuestionnaireDto>(questionnaire),
                QuestionnaireAnswers = ObjectMapper.Map<List<QuestionnaireAnswer>, List<QuestionnaireAnswerDto>>(questionnaire.QuestionnaireAnswers.ToList()),
                //TODO : add enum and replace here
                SubmitedAnswerId = questionnaire.AnswerComponentId == 1 && submitedAnswer != null
                    ? submitedAnswer.AnswerId
                    : null,
                AnswerContent = questionnaire.AnswerComponentId == 2 && submitedAnswer != null
                    ? submitedAnswer.AnswerDescription
                    : null
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
            Questionnaire questionnaire;
            SubmittedAnswers submittedAnswer;
            var submittedAnswerQuery = await _submitedAnswerRepository.GetQueryableAsync();
            if (submitteAnswerDto.QuestionnaireId.HasValue)
            {
                questionnaire = _questionnaireRepository.WithDetails().FirstOrDefault(x => x.Id == submitteAnswerDto.QuestionnaireId.Value);
                //TODO : Add enum and replace here
                if (questionnaire.AnswerComponentId != 2)
                    throw new UserFriendlyException("فقط برای سوال های تشریحی میتوان کد پرسشنامه را ارسال کرد");
                submittedAnswerQuery.Where(x => x.UserId == _commonAppService.GetUserId() && x.QuestionnaireId == submitteAnswerDto.QuestionnaireId.Value);
                if (submittedAnswerQuery.FirstOrDefault() != null)
                    throw new UserFriendlyException("شما قبلا به این سوال جواب داده اید");
                submittedAnswer = await _submitedAnswerRepository.InsertAsync(
                new SubmittedAnswers()
                {
                    UserId = _commonAppService.GetUserId(),
                    AnswerId = null,
                    AnswerDescription = submitteAnswerDto.AnswerDescription,
                    QuestionnaireId = submitteAnswerDto.QuestionnaireId.Value
                });
                return ObjectMapper.Map<SubmittedAnswers, SubmitteAnswerDto>(submittedAnswer);
            }
            var userId = _commonAppService.GetUserId();

            var answer = _questionnaireAnswerRepository.WithDetails(x => x.Questionnaire)
                .FirstOrDefault(x => x.Id == submitteAnswerDto.AnswerId)
                ?? throw new UserFriendlyException("جواب مورد نظر پیدا نشد");
            var questionId = answer.QuestionnaireId;
            questionnaire = _questionnaireRepository.WithDetails(x => x.QuestionnaireAnswers).FirstOrDefault(x => x.Id == questionId)
                ?? throw new UserFriendlyException("سوال مورد نظر پیدا نشد");
            if (questionnaire.AnswerComponentId == 2)
                throw new UserFriendlyException("نوع سوال و پاسخ همخوانی ندارد");

            var answerIds = questionnaire.QuestionnaireAnswers.Select(x => x.Id).ToList();
            submittedAnswerQuery = submittedAnswerQuery.Where(x => x.UserId == userId && answerIds.Any(y => y == x.AnswerId.Value));
            var submitted = submittedAnswerQuery.FirstOrDefault();
            if (submitted != null)
                throw new UserFriendlyException($"شما قبلا به این سوال جواب داده اید");

            submittedAnswer = await _submitedAnswerRepository.InsertAsync(
                new SubmittedAnswers()
                {
                    UserId = userId,
                    AnswerId = submitteAnswerDto.AnswerId,
                    AnswerDescription = submitteAnswerDto.AnswerDescription,
                    QuestionnaireId = questionId
                });
            return ObjectMapper.Map<SubmittedAnswers, SubmitteAnswerDto>(submittedAnswer);
        }

        public async Task SubmitAnswers(List<int> answerIds)
        {
            //TODO : add enum and replace it here
            var incomingQuestionnaireIds = _questionnaireRepository.WithDetails(x => x.QuestionnaireAnswers).Where(x => x.AnswerComponentId == 1).Select(x => x.Id);
            var questionnaireIds = (await _questionnaireAnswerRepository.GetQueryableAsync()).Select(x => x.QuestionnaireId);
            var unAnsweredQuestions = questionnaireIds.Any(x => incomingQuestionnaireIds.Any(y => y != x));
            if (unAnsweredQuestions)
                throw new UserFriendlyException("لطفا به تمام سوالات پاسخ دهید");

            answerIds.ForEach(async x => await SubmitAnswer(new SubmitteAnswerDto()
            {
                AnswerId = x
            }));
        }
    }
}
