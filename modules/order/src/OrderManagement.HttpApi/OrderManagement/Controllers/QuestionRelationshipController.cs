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

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/QuestionRelationship/[action]")]
public class QuestionRelationshipController : Controller 
{
    private readonly IQuestionRelationshipService _questionRelationshipService;

    public QuestionRelationshipController(IQuestionRelationshipService questionRelationshipService)
    {
        _questionRelationshipService = questionRelationshipService;
    }
    /// <summary class="header">
    /// لیست ارتباطات سوال ها 
    /// </summary>
    /// <param name="questionId">hi babe</param>
    /// <remarks>bye babe</remarks>
    [HttpGet]
    public async Task<List<QuestionRelationshipDto>> GetListByQuestionId(int questionId)
   => await _questionRelationshipService.GetList(questionId);

    [HttpPost]   
    public async Task<int> Add(QuestionRelationshipDto questionRelationshipDto)
        => await _questionRelationshipService.Add(questionRelationshipDto);

    [HttpGet]
    public async Task<QuestionRelationshipDto> GetById(int id)
        => await (_questionRelationshipService.GetById(id));

    [HttpPut]
    public async Task<int> Update(QuestionRelationshipDto questionRelationshipDto)
    => await (_questionRelationshipService.Update(questionRelationshipDto));

    [HttpDelete]
    public async Task<bool> Delete(int id)
    => await (_questionRelationshipService.Delete(id));

}
