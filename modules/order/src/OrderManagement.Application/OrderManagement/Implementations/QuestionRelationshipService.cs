using EasyCaching.Core;
using Esale.Share.Authorize;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Nest;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Dtos;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Constants;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Application.OrderManagement.FluentValidation;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;
using Permission.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class QuestionRelationshipService : ApplicationService, IQuestionRelationshipService
{
    private readonly IRepository<QuestionRelationship, int> _questionRelationshipRepository;

    private readonly IValidator<QuestionRelationshipDto> _questionRelationshipValidator;



    public QuestionRelationshipService(
        IRepository<QuestionRelationship, int> questionRelationshipRepository, IValidator<QuestionRelationshipDto> questionRelationshipValidator)
    {
        _questionRelationshipRepository = questionRelationshipRepository;
        _questionRelationshipValidator = questionRelationshipValidator;

    }

    [SecuredOperation(QuestionRelationshipServicePermissionConstants.GetById)]
    public async Task<QuestionRelationshipDto> GetById(int id)
    {
        var validationResult = await _questionRelationshipValidator.ValidateAsync(new QuestionRelationshipDto { Id = id }, options => options.IncludeRuleSets(RuleSets.GetById));

        if (!validationResult.IsValid)
        {
            var ex = new ValidationException(validationResult.Errors);
            throw new UserFriendlyException(ex.Message, ValidationConstant.ItemNotFound);
        }

        var questionRelationship = await _questionRelationshipRepository.GetAsync(x => x.Id == id);
        var questionRelationshipDto = ObjectMapper.Map<QuestionRelationship, QuestionRelationshipDto>(questionRelationship);
        return questionRelationshipDto;

    }

    [SecuredOperation(QuestionRelationshipServicePermissionConstants.GetList)]
    public async Task<List<QuestionRelationshipDto>> GetList(int questionId)
    {
        var validationResult = await _questionRelationshipValidator.ValidateAsync(new QuestionRelationshipDto { QuestionId = questionId }, options => options.IncludeRuleSets(RuleSets.GetListByQuestionId));

        if (!validationResult.IsValid)
        {
            var ex = new ValidationException(validationResult.Errors);
            throw new UserFriendlyException(ex.Message, ValidationConstant.ItemNotFound);
        }

        var qrs = await _questionRelationshipRepository.GetQueryableAsync();
        var questionRel = qrs
                        .AsNoTracking()
                       .Where(x => x.QuestionId == questionId)
                       .ToList();
        var questionRelationshipDto = ObjectMapper.Map<List<QuestionRelationship>, List<QuestionRelationshipDto>>(questionRel);
        return questionRelationshipDto;
    }

    [SecuredOperation(QuestionRelationshipServicePermissionConstants.Delete)]
    public async Task<bool> Delete(int id)
    {
        var validationResult = await _questionRelationshipValidator.ValidateAsync(new QuestionRelationshipDto { Id = id }, options => options.IncludeRuleSets(RuleSets.Delete));

        if (!validationResult.IsValid)
        {
            var ex = new ValidationException(validationResult.Errors);
            throw new UserFriendlyException(ex.Message, ValidationConstant.ItemNotFound);
        }

        await _questionRelationshipRepository.DeleteAsync(x => x.Id == id);
        await CurrentUnitOfWork.SaveChangesAsync();
        return true;
    }

    [SecuredOperation(QuestionRelationshipServicePermissionConstants.Add)]
    public async Task<int> Add(QuestionRelationshipDto questionRelationshipDto)
    {

        var validationResult = await _questionRelationshipValidator.ValidateAsync(questionRelationshipDto, options => options.IncludeRuleSets(RuleSets.Add));

        if (!validationResult.IsValid)
        {
            var ex = new ValidationException(validationResult.Errors);
            throw new UserFriendlyException(ex.Message, ValidationConstant.ItemNotFound);
        }
        var questionRelationship = ObjectMapper.Map<QuestionRelationshipDto, QuestionRelationship>(questionRelationshipDto);
        await _questionRelationshipRepository.InsertAsync(questionRelationship);
        await CurrentUnitOfWork.SaveChangesAsync();

        return questionRelationship.Id;
    }   

    [SecuredOperation(QuestionRelationshipServicePermissionConstants.Update)]
    public async Task<int> Update(QuestionRelationshipDto questionRelationshipDto)
    {
        var validationResult = await _questionRelationshipValidator.ValidateAsync(questionRelationshipDto, options => options.IncludeRuleSets(RuleSets.Edit));

        if (!validationResult.IsValid)
        {
            var ex = new ValidationException(validationResult.Errors);
            throw new UserFriendlyException(ex.Message, ValidationConstant.ItemNotFound);
        }

        var result = await _questionRelationshipRepository.WithDetails().AsNoTracking().FirstOrDefaultAsync(x => x.Id == questionRelationshipDto.Id);
        if (result == null)
        {
            throw new UserFriendlyException("رکوردی برای ویرایش وجود ندارد");
        }

        var qr = ObjectMapper.Map<QuestionRelationshipDto, QuestionRelationship>(questionRelationshipDto);
        await _questionRelationshipRepository.UpdateAsync(qr);             
        await CurrentUnitOfWork.SaveChangesAsync();
        return qr.Id;
  
    }
}
