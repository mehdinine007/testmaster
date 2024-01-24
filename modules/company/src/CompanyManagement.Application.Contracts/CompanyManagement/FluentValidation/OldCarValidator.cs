using CompanyManagement.Application.Contracts.CompanyManagement.Constants.Validation;
using CompanyManagement.Application.Contracts.CompanyManagement.Dto.OldCarDtos;
using FluentValidation;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Application.Contracts.CompanyManagement.FluentValidation
{
    public class OldCarValidator : AbstractValidator<OldCarCreateDtoList>
    {
        public OldCarValidator()
        {
            RuleFor(x => x.Vehicle).NotNull().NotEmpty().WithMessage(ValidationConstant.ItemNotFound).MaximumLength(50).WithMessage(ValidationConstant.ItemNotFound);
            
            //RuleForEach(x => x.OldCars).ChildRules(oldCar =>
            //{
            //    oldCar.RuleFor(x => x.Nationalcode).NotNull().NotEmpty().WithMessage(ValidationConstant.NationalCodeIsRequired);
            //    oldCar.RuleFor(x => x.ChassiNo).NotNull().NotEmpty().WithMessage(ValidationConstant.ChassiIsRequired);
            //    oldCar.RuleFor(x => x.Vin).NotNull().NotEmpty().WithMessage(ValidationConstant.VinIsRequired);
            //    oldCar.RuleFor(x => x.Vehicle).NotNull().NotEmpty().WithMessage(ValidationConstant.VehicleIsRequired);
            //    oldCar.RuleFor(x => x.EngineNo).NotNull().NotEmpty().WithMessage(ValidationConstant.EngineIsRequired);
            //});

            


        }
    }
    



}


