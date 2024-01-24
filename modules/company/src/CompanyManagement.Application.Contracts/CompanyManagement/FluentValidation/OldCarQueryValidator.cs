using CompanyManagement.Application.Contracts.CompanyManagement.Constants.Validation;
using CompanyManagement.Application.Contracts.CompanyManagement.Dto.OldCarDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Application.Contracts.CompanyManagement.FluentValidation
{
    public class OldCarQueryValidator: AbstractValidator<OldCarQueryDto>
    {

        public OldCarQueryValidator() {

            RuleFor(x => x.NationalCode).NotNull().NotEmpty().WithMessage(ValidationConstant.NationalCodeIsRequired);
        }   
    }
}
