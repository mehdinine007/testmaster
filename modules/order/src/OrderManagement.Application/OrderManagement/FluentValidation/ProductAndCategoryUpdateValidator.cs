using FluentValidation;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.OrderManagement.FluentValidation
{
    public class ProductAndCategoryUpdateValidator: AbstractValidator<ProductAndCategoryUpdateDto>
    {
        public ProductAndCategoryUpdateValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage(ValidationConstant.TitleNotFound);
        }
    }
}
