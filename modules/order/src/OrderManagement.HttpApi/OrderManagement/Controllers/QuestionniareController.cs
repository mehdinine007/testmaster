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
    public class QuestionniareController : AbpController, IQuestionniareService
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
        public async Task CreatQuestionnaireAnswers(List<QuestionnaireAnswerDto> questionnaireAnswers)
            => await _questionnaireService.CreatQuestionnaireAnswers(questionnaireAnswers);


        [HttpGet]
        [UserAuthorization]
        public async Task<List<AnswerComponentTypeDto>> GetAnswerComponentTypes()
            => await _questionnaireService.GetAnswerComponentTypes();


        [HttpGet]
        [UserAuthorization]
        [RemoteService(isEnabled: false)]
        public async Task<List<QuestionnaireTree>> GetQuestionnaireTrees()
            => await _questionnaireService.GetQuestionnaireTrees();

        [HttpPost]
        [UserAuthorization]
        public async Task<SubmiteAnswerDto> SubmitAnswer(SubmiteAnswerDto submiteAnswerDto)
            => await _questionnaireService.SubmitAnswer(submiteAnswerDto);

    }
}
