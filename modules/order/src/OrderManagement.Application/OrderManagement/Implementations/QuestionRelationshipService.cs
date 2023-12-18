using EasyCaching.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Nest;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.Dtos;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;
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


    public QuestionRelationshipService(
        IRepository<QuestionRelationship, int> questionRelationshipRepository)
    {
        _questionRelationshipRepository = questionRelationshipRepository;
    }

    public async Task<int> Add(QuestionRelationshipDto questionRelationshipDto)
    {
        var questionRelationship = ObjectMapper.Map<QuestionRelationshipDto, QuestionRelationship>(questionRelationshipDto);
        await _questionRelationshipRepository.InsertAsync(questionRelationship, autoSave: true);
        return questionRelationship.Id;
    }

    public async Task<bool> Delete(int id)
    {
        await _questionRelationshipRepository.DeleteAsync(x => x.Id == id, autoSave: true);
        return true;
    }

    public async Task<QuestionRelationshipDto> GetById(int id)
    {
        var questionRelationship = await _questionRelationshipRepository.GetAsync(x => x.Id == id);
        var questionRelationshipDto = ObjectMapper.Map<QuestionRelationship, QuestionRelationshipDto>(questionRelationship);
        return questionRelationshipDto;

    }

    public async Task<List<QuestionRelationshipDto>> GetList(int questionId)
    {
        var qrs = await _questionRelationshipRepository.GetQueryableAsync();
        var questionRel = qrs
                        .AsNoTracking()
                       .Where(x => x.QuestionId == questionId)
                       .ToList();
        var questionRelationshipDto = ObjectMapper.Map<List<QuestionRelationship>, List<QuestionRelationshipDto>>(questionRel);
        return questionRelationshipDto;
    }

    public async Task<int> Update(QuestionRelationshipDto questionRelationshipDto)
    {
        var result = await _questionRelationshipRepository.WithDetails().AsNoTracking().FirstOrDefaultAsync(x => x.Id == questionRelationshipDto.Id);
        if (result == null)
        {
            throw new UserFriendlyException("رکوردی برای ویرایش وجود ندارد");
        }
        var qr = ObjectMapper.Map<QuestionRelationshipDto, QuestionRelationship>(questionRelationshipDto);

        return qr.Id;
    }
}
