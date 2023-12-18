using FluentValidation;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Constants;
using OrderManagement.Domain.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.FluentValidation
{
    public class QuestionGroupValidator : AbstractValidator<QuestionGroupDto>
    {
        public QuestionGroupValidator(IRepository<QuestionGroup, int> questionGrouprepository)
        {
            RuleFor(x => x.Title).NotNull().NotEmpty().MaximumLength(50).WithMessage(ValidationConstant.ItemNotFound);
            RuleFor(x => x.QuestionnaireId).NotNull().NotEmpty().WithMessage(ValidationConstant.QuestionnerNotFound);

            RuleSet(RuleSets.Add, () =>
            {

            });

            RuleSet(RuleSets.Edit, () =>
            {
                RuleFor(x => x.Id).MustAsync(async (o, e) => await questionGrouprepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.ItemNotFound);
            });
            RuleSet(RuleSets.Delete, () =>
            {
                RuleFor(x => x.Id).MustAsync(async (o, e) => await questionGrouprepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.ItemNotFound);
            });
            RuleSet(RuleSets.GetById, () =>
            {
                RuleFor(x => x.Id).MustAsync(async (o, e) => await questionGrouprepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.ItemNotFound);
            });
        }
    }
}
