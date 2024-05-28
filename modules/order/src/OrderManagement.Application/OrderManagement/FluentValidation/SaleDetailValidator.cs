using FluentValidation;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Constants;
using OrderManagement.Domain.Shared;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.OrderManagement.FluentValidation
{
    public class SaleDetailValidator : AbstractValidator<CreateSaleDetailDto>
    {
        public SaleDetailValidator()
        {

            RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage(ValidationConstant.TitleNotFound);
            RuleFor(x => x.SalePlanCode).NotNull().NotEmpty().WithMessage(ValidationConstant.CodeNotFound);
            RuleFor(x => x.SalePlanDescription).NotNull().NotEmpty().WithMessage(ValidationConstant.SalePlanDescriptionNotFound);
            RuleFor(x => x.SalePlanStartDate).NotNull().NotEmpty().WithMessage(ValidationConstant.SalePlanStartDateNotFound);
            RuleFor(x => x.SalePlanEndDate).NotNull().NotEmpty().WithMessage(ValidationConstant.SalePlanEndDateNotFound);
            RuleFor(x => x.EsaleTypeId).Must(i => Enum.IsDefined(typeof(ESaleTypeEnums), i)).WithMessage(ValidationConstant.SaleProcessNotFound);
            RuleFor(x => x.CarFee).NotNull().NotEmpty().WithMessage(ValidationConstant.CarFeeNotFound);
            RuleFor(x => x.MinimumAmountOfProxyDeposit).NotNull().NotEmpty().WithMessage(ValidationConstant.MinimumAmountOfProxyDepositNotFound);
            RuleFor(x => x.SaleId).NotNull().NotEmpty().WithMessage(ValidationConstant.SaleIdNotFound);
            RuleFor(x => x.SaleProcess).Must(i => Enum.IsDefined(typeof(SaleProcessType), i)).WithMessage(ValidationConstant.SaleProcessNotFound);
     
        }
    }
}
