using Esale.Share.Authorize;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Nest;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Constants;
using OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class QuestionGroupService : ApplicationService, IQuestionGroupService
    {
        private readonly IRepository<QuestionGroup, int> _questionGroupRepository;
        private readonly IRepository<Questionnaire, int> _questionnaireRepository;
        private readonly IValidator<QuestionGroupDto> _questionGroupValidator;
        public QuestionGroupService(IRepository<QuestionGroup, int> questionGroupRepository
                                   , IRepository<Questionnaire, int> questionnaireRepository
                                  , IValidator<QuestionGroupDto> questionGroupValidator
                                  )
        {
            _questionGroupRepository = questionGroupRepository;
            _questionnaireRepository = questionnaireRepository;
            _questionGroupValidator = questionGroupValidator;

        }

        [SecuredOperation(QuestionGroupServicePermissionConstants.Add)]
        public async Task<QuestionGroupDto> Add(QuestionGroupDto questionGroupDto)
        {
            var validationResult = await _questionGroupValidator.ValidateAsync(questionGroupDto, Options => Options.IncludeRuleSets(RuleSets.Add));
            if (!validationResult.IsValid)
            {
                var ex = new ValidationException(validationResult.Errors);
                throw new UserFriendlyException(ex.Message, ValidationConstant.QuestionnerNotFound);
            }
            var maxcod = (await _questionGroupRepository.GetQueryableAsync())
                    .Where(x => x.QuestionnaireId == questionGroupDto.QuestionnaireId)
                    .DefaultIfEmpty().Max(x => x.Code);

            questionGroupDto.Code = 1;
            if (maxcod != null)
            {
                questionGroupDto.Code = (int)(maxcod + 1);
            }

            var qustionGroup = ObjectMapper.Map<QuestionGroupDto, QuestionGroup>(questionGroupDto);
            await _questionGroupRepository.InsertAsync(qustionGroup);
            return ObjectMapper.Map<QuestionGroup, QuestionGroupDto>(qustionGroup);

        }

        [SecuredOperation(QuestionGroupServicePermissionConstants.Delete)]
        public async Task<bool> Delete(int Id)
        {
            var validationResult = await _questionGroupValidator.ValidateAsync(new QuestionGroupDto { Id = Id }, options => options.IncludeRuleSets(RuleSets.Delete));

            if (!validationResult.IsValid)
            {
                var ex = new ValidationException(validationResult.Errors);
                throw new UserFriendlyException(ex.Message, ValidationConstant.ItemNotFound);
            }
            await _questionGroupRepository.DeleteAsync(Id);
            return true;
        }

        [SecuredOperation(QuestionGroupServicePermissionConstants.GetList)]
        public async Task<List<QuestionGroupDto>> GetAll(int QuestionnaireId)
        {
            var validationResult = await _questionGroupValidator.ValidateAsync(new QuestionGroupDto { QuestionnaireId = QuestionnaireId}, options => options.IncludeRuleSets(ValidationConstant.GetListByQuestionner));

            if (!validationResult.IsValid)
            {
                var ex = new ValidationException(validationResult.Errors);
                throw new UserFriendlyException(ex.Message, ValidationConstant.GetListByQuestionner);
            }
            var qusetion = (await _questionGroupRepository.GetQueryableAsync())
                            .AsNoTracking()
                            .Where(x => x.QuestionnaireId == QuestionnaireId).ToList();
              return ObjectMapper.Map<List<QuestionGroup>, List<QuestionGroupDto>>(qusetion);
        }

        [SecuredOperation(QuestionGroupServicePermissionConstants.GetById)]
        public async Task<QuestionGroupDto> GetById(int Id)
        {
            var validationResult = await _questionGroupValidator.ValidateAsync(new QuestionGroupDto { Id = Id }, options => options.IncludeRuleSets(RuleSets.GetById));

            if (!validationResult.IsValid)
            {
                var ex = new ValidationException(validationResult.Errors);
                throw new UserFriendlyException(ex.Message, ValidationConstant.ItemNotFound);
            }
            var questiongroup = (await _questionGroupRepository.GetQueryableAsync())
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == Id);
            return ObjectMapper.Map<QuestionGroup, QuestionGroupDto>(questiongroup);
        }

        [SecuredOperation(QuestionGroupServicePermissionConstants.Update)]
        public async Task<QuestionGroupDto> Update(QuestionGroupDto questionGroupDto)
        {
            var validationResult = await _questionGroupValidator.ValidateAsync(questionGroupDto, Options => Options.IncludeRuleSets(RuleSets.Edit));
            if (!validationResult.IsValid)
            {
                var ex = new ValidationException(validationResult.Errors);
                throw new UserFriendlyException(ex.Message, ValidationConstant.ItemNotFound);
            }
            var questionGroupQuery = (await _questionGroupRepository.GetQueryableAsync())
                                        .FirstOrDefault(x => x.Id == questionGroupDto.Id);
            
                questionGroupQuery.Title = questionGroupDto.Title;

            var result = await _questionGroupRepository.UpdateAsync(questionGroupQuery);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<QuestionGroup, QuestionGroupDto>(result);

        }       
    }
}
