using FluentValidation;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.OrderManagement.FluentValidation
{
    public class AgencyCreateValidator: AbstractValidator<AgencyCreateOrUpdateDto>
    {
        public AgencyCreateValidator()
        {

            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage(ValidationConstant.TitleNotFound);
            RuleFor(x => x.Code).NotNull().NotEmpty().WithMessage(ValidationConstant.CodeNotFound);
            RuleFor(x => x.ProvinceId).NotNull().NotEmpty().WithMessage(ValidationConstant.ProvinceNotFound);
        }
    }
}
