using FluentValidation;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Constants;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.OrderManagement.FluentValidation
{
    public class SeasonAllocationCreateValidator: AbstractValidator<SeasonAllocationCreateDto>
    {
        public SeasonAllocationCreateValidator()
        {

            RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage(ValidationConstant.TitleNotFound);
            RuleFor(x => x.SeasonId).Must(i => Enum.IsDefined(typeof(SeasonTypeEnum), i)).WithMessage(ValidationConstant.SeasonIdNotFound);
            RuleFor(x => x.Year).NotNull().NotEmpty().WithMessage(ValidationConstant.YearNotValid);
           
        }
    }
}
