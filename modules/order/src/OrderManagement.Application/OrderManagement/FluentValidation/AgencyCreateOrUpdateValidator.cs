using FastReport;
using FluentValidation;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Constants;
using OrderManagement.Domain.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.OrderManagement.FluentValidation
{
    public class AgencyCreateOrUpdateValidator : AbstractValidator<AgencyCreateDto>
    {
        public AgencyCreateOrUpdateValidator()
        {
            
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage(ValidationConstant.TitleNotFound);
            RuleFor(x => x.Code).NotNull().NotEmpty().WithMessage(ValidationConstant.CodeNotFound);
            RuleFor(x => x.ProvinceId).NotNull().NotEmpty().WithMessage(ValidationConstant.ProvinceNotFound);
        }
    }
}
