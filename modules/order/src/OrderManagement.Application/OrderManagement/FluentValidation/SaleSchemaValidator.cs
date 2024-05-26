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
    public class SaleSchemaValidator: AbstractValidator<CreateSaleSchemaDto>
    {
        public SaleSchemaValidator()
        {

            RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage(ValidationConstant.TitleNotFound);
            RuleFor(x => x.Code).NotNull().NotEmpty().WithMessage(ValidationConstant.CodeNotFound);
        }
    }
}
