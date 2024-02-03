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
    public class AdvertisementDetailWithIdValidator : AbstractValidator<AdvertisementDetailWithIdDto>
    {
        private readonly IRepository<AdvertisementDetail> _advertisementDetailRepository;
        public AdvertisementDetailWithIdValidator(IRepository<AdvertisementDetail> advertisementDetailRepository)
        {
            _advertisementDetailRepository = advertisementDetailRepository;
            RuleSet(RuleSets.Move, () =>
            {

                RuleFor(x => x.Id).MustAsync(async (o, e) => await _advertisementDetailRepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.ItemNotFound);
            });

            RuleSet(RuleSets.Delete, () =>
            {
                RuleFor(x => x.Id).MustAsync(async (o, e) => await _advertisementDetailRepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.ItemNotFound);
            });

            RuleSet(RuleSets.UploadFile, () =>
            {
                RuleFor(x => x.Id).MustAsync(async (o, e) => await _advertisementDetailRepository.AnyAsync(p => p.Id == o)).WithMessage(ValidationConstant.ItemNotFound);
            });
        }

    }
}
