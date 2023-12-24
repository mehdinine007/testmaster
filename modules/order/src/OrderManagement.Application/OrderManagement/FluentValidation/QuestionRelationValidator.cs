using FluentValidation;
using IFG.Core.Utility.Tools;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Constants;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using static Volo.Abp.ObjectExtending.AuditLoggingModuleExtensionConsts;

namespace OrderManagement.Application.OrderManagement.FluentValidation
{
    public class QuestionRelationValidator : AbstractValidator<QuestionRelationshipDto>
    {
        private readonly IRepository<Question, int> _questionRepository;
        private readonly IRepository<QuestionAnswer, long> _questionAnswerRepository;

        public QuestionRelationValidator(IRepository<QuestionRelationship, int> questionRelationrepository, IRepository<Question, int> questionRepository, IRepository<QuestionAnswer, long> questionAnswerRepository)
        {
            _questionRepository = questionRepository;
            _questionAnswerRepository = questionAnswerRepository;

            RuleFor(x => x.QuestionId).NotNull().NotEmpty().WithMessage(ValidationConstant.ItemNotFound);
            RuleFor(x => x.QuestionRelationId).NotNull().NotEmpty().WithMessage(ValidationConstant.ItemNotFound);
            RuleFor(x => x.QuestionAnswerId).NotNull().NotEmpty().WithMessage(ValidationConstant.ItemNotFound);
            RuleFor(x => x.OperationType).NotNull().NotEmpty().WithMessage(ValidationConstant.ItemNotFound);

            RuleSet(RuleSets.Add, () =>
            {
                RuleFor(x => x.QuestionId).MustAsync(async (o, e) => await _questionRepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.QuestionNotFound);
                RuleFor(x => x.QuestionRelationId).MustAsync(async (o, e) => await _questionRepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.QuestionNotFound);
                RuleFor(x => x.QuestionAnswerId).MustAsync(async (o, e) => await _questionAnswerRepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.AnswerNotFound);
                RuleFor(x => x.OperationType).Must((o, e) => EnumExtension.GetEnumValuesAndDescriptions<OperatorFilterEnum>().Any(d => d.Key == (int)o.OperationType)).WithMessage(ValidationConstant.OperatorNotFound);


            });

            RuleSet(RuleSets.Edit, () =>
            {
                RuleFor(x => x.Id).MustAsync(async (o, e) => await questionRelationrepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.ItemNotFound);
                RuleFor(x => x.QuestionId).MustAsync(async (o, e) => await _questionRepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.QuestionNotFound);
                RuleFor(x => x.QuestionRelationId).MustAsync(async (o, e) => await _questionRepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.QuestionNotFound);
                RuleFor(x => x.QuestionAnswerId).MustAsync(async (o, e) => await _questionAnswerRepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.AnswerNotFound);
                RuleFor(x => x.OperationType).Must((o, e) => EnumExtension.GetEnumValuesAndDescriptions<OperatorFilterEnum>().Any(d => d.Key == (int)o.OperationType)).WithMessage(ValidationConstant.OperatorNotFound);

            });
            RuleSet(RuleSets.Delete, () =>
            {
                RuleFor(x => x.Id).MustAsync(async (o, e) => await questionRelationrepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.ItemNotFound);
            });
            RuleSet(RuleSets.GetById, () =>
            {
                RuleFor(x => x.Id).MustAsync(async (o, e) => await questionRelationrepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.ItemNotFound);
            });
            RuleSet(RuleSets.GetListByQuestionId, () =>
            {
                RuleFor(x => x.QuestionId).MustAsync(async (o, e) => await questionRelationrepository.AnyAsync(p => p.QuestionId == o)).WithMessage(ValidationConstant.QuestionNotFound);
            });

        }


    }
}
