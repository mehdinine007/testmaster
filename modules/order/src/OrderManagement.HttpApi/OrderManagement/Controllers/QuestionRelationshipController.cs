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
    //--------------------------------------------------
    /// <summary class="header">
    ///گرفتن تمام گروهبندی سوالات 
    /// </summary>
    /// <remarks>
    /// Sample value of message
    /// 
    ///     پارامترها 
    ///     questionId: int : پرسشنامه
    /// </remarks>
    [HttpGet]
    public async Task<List<QuestionRelationshipDto>> GetListByQuestionId(int questionId)
   => await _questionRelationshipService.GetList(questionId);
    //--------------------------------------------------
    /// <summary class="header">
    ///گرفتن گروهبندی سوالات با شناسه 
    /// </summary>
    /// <remarks>
    /// Sample value of message
    /// 
    ///     پارامترها 
    ///     Id: int : گروهبندی سوالات
    /// </remarks>
    [HttpGet]
    public async Task<QuestionRelationshipDto> GetById(int id)
        => await (_questionRelationshipService.GetById(id));
    //--------------------------------------------------
    /// <summary class="header">
    ///اضافه کردن گروهبندی سوالات 
    /// </summary>
    /// <remarks>
    /// Sample value of message
    /// 
    ///    تمام پارامترها اجباری است
    ///    
    /// </remarks>
    [HttpPost]
    public async Task<int> Add(QuestionRelationshipDto questionRelationshipDto)
       => await _questionRelationshipService.Add(questionRelationshipDto);
    //--------------------------------------------------
    /// <summary class="header">
    ///ویرایش ارتباط سوالات 
    /// </summary>
    /// <remarks>
    /// Sample value of message
    /// 
    ///     تمام پارامترها اجباری است ،QuestionAnswer نیاز خالی باید پاک شود
    ///     
    /// </remarks>
    [HttpPut]
    public async Task<int> Update(QuestionRelationshipDto questionRelationshipDto)
    => await (_questionRelationshipService.Update(questionRelationshipDto));
    //--------------------------------------------------
    /// <summary class="header">
    ///حذف ارتباط سوالات  
    /// </summary>
    /// <remarks>
    /// Sample value of message
    /// 
    ///     پارامتر زیر اجباری است 
    ///     Id: int : شناسه گروهبندی سوالات
    ///     
    /// </remarks>
    [HttpDelete]
    public async Task<bool> Delete(int id)
    => await (_questionRelationshipService.Delete(id));

}
