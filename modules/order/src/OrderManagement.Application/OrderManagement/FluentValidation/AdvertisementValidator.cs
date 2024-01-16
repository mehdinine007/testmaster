using FluentValidation;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Constants;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.FluentValidation
{
    public class AdvertisementValidator : AbstractValidator<AdvertisementCreateOrUpdateDto>
    {
        private readonly IRepository<Advertisement, int> _advertisementRepository;
        public AdvertisementValidator(IRepository<Advertisement, int> advertisementRepository)
        {
            _advertisementRepository = advertisementRepository;
            RuleSet(RuleSets.Add, () =>
            {
                RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage(ValidationConstant.TitleNotFound);
                RuleFor(x => x.Id==0).NotNull().NotEmpty().WithMessage(ValidationConstant.AddAdvertisementIdValue);
                
            });

            RuleSet(RuleSets.Edit, () =>
            {
                RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage(ValidationConstant.TitleNotFound);
                RuleFor(x => x.Id).MustAsync(async (o, e) => await _advertisementRepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.ItemNotFound);
            });

            RuleSet(RuleSets.Delete, () =>
            {
                RuleFor(x => x.Id).MustAsync(async (o, e) => await _advertisementRepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.ItemNotFound);
            });
            

        }

        
          


    }
}
