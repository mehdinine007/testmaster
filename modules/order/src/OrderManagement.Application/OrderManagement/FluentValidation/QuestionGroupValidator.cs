using FluentValidation;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Constants;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.FluentValidation
{
    public class QuestionGroupValidator : AbstractValidator<QuestionGroupDto>
    {
        private readonly IRepository<Questionnaire, int> _questionnaireRepository;

        public QuestionGroupValidator(IRepository<QuestionGroup, int> questionGrouprepository, IRepository<Questionnaire, int> questionnaireRepository)
        {
            _questionnaireRepository = questionnaireRepository;
           
            RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage(ValidationConstant.TitleNotFound).MaximumLength(50).WithMessage(ValidationConstant.ItemNotFound);
           
            RuleSet(RuleSets.Add, () =>
            {
                RuleFor(x => x.Title).NotNull().NotEmpty().MaximumLength(50).WithMessage(ValidationConstant.TitleNotFound);
                RuleFor(x => x.QuestionnaireId).MustAsync(async (o, e) => await questionnaireRepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.QuestionnerNotExist);
            });

            RuleFor(x => x.Title).NotNull().NotEmpty().MaximumLength(50).WithMessage(ValidationConstant.ItemNotFound);
            RuleFor(x => x.QuestionnaireId).NotNull().NotEmpty().WithMessage(ValidationConstant.QuestionnerNotFound);

          
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
            RuleSet(ValidationConstant.GetListByQuestionner, () =>
            {
                RuleFor(x => x.QuestionnaireId).MustAsync(async (o, e) => await questionGrouprepository.AnyAsync(p => p.QuestionnaireId == o)).WithMessage(ValidationConstant.ItemNotFound);

            });
        }
    }
}
