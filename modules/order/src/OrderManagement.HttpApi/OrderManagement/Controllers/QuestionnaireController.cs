using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using System.Threading.Tasks;
using OrderManagement.Application.Contracts.Dtos;
using OrderManagement.Application.Contracts;
using Esale.Share.Authorize;
using OrderManagement.Application.Contracts.OrderManagement;
using System.Collections.Generic;
using OrderManagement.Domain.Shared;
using Esale.Core.Utility.Tools;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/Questionnaire/[action]")]
public class QuestionnaireController : Controller //, IQuestionnaireService
{
    private readonly IQuestionnaireService _questionnaireService;

    public QuestionnaireController(IQuestionnaireService questionnaireService)
    {
        _questionnaireService = questionnaireService;
    }

    [UserAuthorization]
    [HttpGet]
    public async Task<QuestionnaireTreeDto> LoadQuestionnaireTree(int questionnaireId)
        => await _questionnaireService.LoadQuestionnaireTree(questionnaireId);

    [UserAuthorization]
    [HttpPost]
    public async Task<bool> SubmitAnswer(SubmitAnswerTreeDto submitAnswerTreeDto)
    {
        await _questionnaireService.SubmitAnswer(submitAnswerTreeDto);
        return true;
    }

    [HttpPost]
    public async Task<bool> UploadFile([FromForm] UploadFileDto uploadFile)
        => await _questionnaireService.UploadFile(uploadFile);

    [HttpGet]
    public async Task<List<QuestionnaireDto>> LoadQuestionnaireList(string attachmentEntityTypeEnums)
        => await _questionnaireService.LoadQuestionnaireList(EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentEntityTypeEnums));

    [HttpGet]
    public async Task<List<QuestionnaireAnalysisDto>> GetQuestionnaireReport(int questionnaireId, long? relatedEntityId)
        => await _questionnaireService.GetQuestionnaireReport(questionnaireId, relatedEntityId);
}
