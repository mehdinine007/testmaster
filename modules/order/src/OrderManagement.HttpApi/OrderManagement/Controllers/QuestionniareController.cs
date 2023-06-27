using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Esale.Share.Authorize;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Application.Contracts;

namespace OrderManagement.HttpApi.OrderManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/SaleDetailService/[action]")]
    public class QuestionniareController : AbpController
    {
        private readonly IQuestionniareService _questionnaireService;

        public QuestionniareController(IQuestionniareService questionnaireService)
            => _questionnaireService = questionnaireService;


        [HttpPost]
        [UserAuthorization]
        public async Task<QuestionnaireDto> CreateQuestionnaire(QuestionnaireDto questionnaireDto)
            => await _questionnaireService.CreateQuestionnaire(questionnaireDto);

        [HttpPost]
        [UserAuthorization]
        public async Task<bool> CreatQuestionnaireAnswers(CreateQuestionnaireAnswerDto questionnaireAnswers)
        {
            await _questionnaireService.CreatQuestionnaireAnswers(questionnaireAnswers);
            return true;
        }

        [HttpGet]
        public async Task<List<AnswerComponentTypeDto>> GetAnswerComponentTypes()
            => await _questionnaireService.GetAnswerComponentTypes();


        [HttpGet]
        [UserAuthorization]
        public async Task<List<QuestionnaireTree>> GetQuestionnaireTrees()
            => await _questionnaireService.GetQuestionnaireTrees();

        [HttpPost]
        [UserAuthorization]
        public async Task<SubmitteAnswerDto> SubmitAnswer(SubmitteAnswerDto submitteAnswerDto)
            => await _questionnaireService.SubmitAnswer(submitteAnswerDto);

        [UserAuthorization]
        [HttpPost]
        public async Task<bool> SubmitAnswers(List<int> answerIds)
        {
            await _questionnaireService.SubmitAnswers(answerIds);
            return true;
        }
    }
}
