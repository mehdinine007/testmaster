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
        private readonly IRepository<SubmitedAnswers, int> _submitedAnswerRepository;
        private readonly ICommonAppService _commonAppService;

        public QuestionniareService(IRepository<Questionnaire, int> questionnaireRepository,
                                    IRepository<AnswerComponentType, int> answerComponentType,
                                    IRepository<QuestionnaireAnswer, int> questionnaireAnswerRepository,
                                    IRepository<SubmitedAnswers, int> submitedAnswerRepository,
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

        public async Task<SubmiteAnswerDto> SubmitAnswer(SubmiteAnswerDto submiteAnswerDto)
        {
            var userId = _commonAppService.GetUserId();
            var submitedAnswerQuery = await _submitedAnswerRepository.GetQueryableAsync();
            var submittedAnswer = submitedAnswerQuery.FirstOrDefault(x => x.UserId == userId && submiteAnswerDto.AnswerId == x.AnswerId);
            if (submittedAnswer != null)
                throw new UserFriendlyException("شما قبلا به این سوال پاسخ داده اید");

            var submitedAnswer = await _submitedAnswerRepository.InsertAsync(
                new SubmitedAnswers()
                {
                    UserId = userId,
                    AnswerId = submiteAnswerDto.AnswerId,
                    AnswerDescription = submiteAnswerDto.AnswerDescription,
                });
            return ObjectMapper.Map<SubmitedAnswers, SubmiteAnswerDto>(submitedAnswer);
        }
    }
}
