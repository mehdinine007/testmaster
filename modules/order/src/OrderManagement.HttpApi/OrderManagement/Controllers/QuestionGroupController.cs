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
using IFG.Core.Utility.Tools;
using OrderManagement.Application.OrderManagement.Implementations;
using Volo.Abp.AspNetCore.Mvc;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/QuestionGroup/[action]")]
public class QuestionGroupController : AbpController, IQuestionGroupService
{
    private readonly IQuestionGroupService _questionGroupService;

    public QuestionGroupController(IQuestionGroupService questionGroupService)
    {
        _questionGroupService = questionGroupService;
    }
    [HttpGet]
    public async Task<List<QuestionGroupDto>> GetAll() => await _questionGroupService.GetAll();

    [HttpGet]
    public async Task<QuestionGroupDto> GetById(int Id) => await _questionGroupService.GetById(Id);

    [HttpPost]
    public async Task<QuestionGroupDto> Add(QuestionGroupDto questionGroup) => await _questionGroupService.Add(questionGroup);

    [HttpPut]
    public async Task<QuestionGroupDto> Update(QuestionGroupDto questionGroup) => await _questionGroupService.Update(questionGroup);
    [HttpDelete]
    public async Task<bool> Delete(int Id) => await _questionGroupService.Delete(Id);
}
