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
    //--------------------------------------------------
    /// <summary class="header">
    ///گرفتن تمام گروهبندی سوالات 
    /// </summary>
    /// <remarks>
    /// Sample value of message
    /// 
    ///     پارامترها 
    ///     QuestionnaireId: int : پرسشنامه
    /// </remarks>
    [HttpGet]
    public async Task<List<QuestionGroupDto>> GetAll(int QuestionnaireId) => await _questionGroupService.GetAll(QuestionnaireId);
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
    public async Task<QuestionGroupDto> GetById(int Id) => await _questionGroupService.GetById(Id);
    //--------------------------------------------------
    /// <summary class="header">
    ///اضافه کردن گروهبندی سوالات 
    /// </summary>
    /// <remarks>
    /// Sample value of message
    /// 
    ///     پارامترها زیر اجباری است 
    ///     Title: int : گروهبندی سوالات
    ///     QuestionnaireId: int : پرسشنامه
    /// </remarks>
    [HttpPost]
    public async Task<QuestionGroupDto> Add(QuestionGroupDto questionGroup) => await _questionGroupService.Add(questionGroup);
    //--------------------------------------------------
    /// <summary class="header">
    ///ویرایش گروهبندی سوالات 
    /// </summary>
    /// <remarks>
    /// Sample value of message
    /// 
    ///     پارامترها زیر اجباری است و فقط اجازه ی ویرایش عنوان وجود دارد 
    ///     Title: int : گروهبندی سوالات
    ///     
    /// </remarks>
    [HttpPut]
    public async Task<QuestionGroupDto> Update(QuestionGroupDto questionGroup) => await _questionGroupService.Update(questionGroup);
    //--------------------------------------------------
    /// <summary class="header">
    ///حذف گروهبندی  
    /// </summary>
    /// <remarks>
    /// Sample value of message
    /// 
    ///     پارامتر زیر اجباری است 
    ///     Id: int : شناسه گروهبندی سوالات
    ///     
    /// </remarks>
    [HttpDelete]
    public async Task<bool> Delete(int Id) => await _questionGroupService.Delete(Id);
}
