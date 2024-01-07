using FluentValidation;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Constants;
using OrderManagement.Domain.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.FluentValidation
{
    public class AdvertisementDetailValidator : AbstractValidator<AdvertisementDetailCreateOrUpdateDto>
    {
        private readonly IRepository<AdvertisementDetail> _advertisementDetailRepository;
        private readonly IRepository<Advertisement> _advertisementRepository;

        public AdvertisementDetailValidator(IRepository<AdvertisementDetail> advertisementDetailRepository,
            IRepository<Advertisement> advertisementRepository)
        {
            _advertisementDetailRepository = advertisementDetailRepository;
            _advertisementRepository = advertisementRepository;
            RuleSet(RuleSets.Add, () =>
            {
                RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage(ValidationConstant.TitleNotFound);
                RuleFor(x => x.AdvertisementId).MustAsync(async (o, e) => await _advertisementRepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.AdvertisementNotFound);
            });

            RuleSet(RuleSets.Edit, () =>
            {
                RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage(ValidationConstant.TitleNotFound);
                RuleFor(x => x.Id).MustAsync(async (o, e) => await _advertisementDetailRepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.ItemNotFound);
                RuleFor(x => x.AdvertisementId).MustAsync(async (o, e) => await _advertisementRepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.AdvertisementNotFound);
            });


        }
    }
}
