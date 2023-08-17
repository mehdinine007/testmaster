using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using Esale.Share.Authorize;
using System.Threading.Tasks;
using OrderManagement.Application.Contracts.Dtos;
using System.Collections.Generic;
using OrderManagement.Application.Contracts;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/Questionnaire/[action]")]
[UserAuthorization]
public class QuestionnaireController : Controller, IQuestionnaireService
{
    private readonly IQuestionnaireService _questionnaireService;

    public QuestionnaireController(IQuestionnaireService questionnaireService)
    {
        _questionnaireService = questionnaireService;
    }

    [HttpGet]
    public async Task<QuestionnaireTreeDto> LoadQuestionnaireTree(int questionnaireId)
        => await _questionnaireService.LoadQuestionnaireTree(questionnaireId);

    [HttpPost]
    public async Task SubmitAnswer(List<SubmitAnswerDto> submitAnswerDtos)
        => await _questionnaireService.SubmitAnswer(submitAnswerDtos);
}
