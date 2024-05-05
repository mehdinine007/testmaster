using Esale.Share.Authorize;
using FluentValidation;
using IFG.Core.DataAccess;
using IFG.Core.Utility.Tools;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Constants;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.OrderManagement.FluentValidation;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using Permission.Order;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class AdvertisementService : ApplicationService, IAdvertisementService
    {
        private readonly IRepository<Advertisement> _advertisementRepository;
        private readonly IValidator<AdvertisementCreateOrUpdateDto> _advertisementValidator;
        private readonly IAttachmentService _attachmentService;

        public AdvertisementService(IRepository<Advertisement> advertisementRepository, IValidator<AdvertisementCreateOrUpdateDto> advertisementValidator
            , IAttachmentService attachmentService)
        {
            _advertisementRepository = advertisementRepository;
            _advertisementValidator = advertisementValidator;
            _attachmentService = attachmentService;
        }
        [SecuredOperation(AdvertisementServicePermissionConstants.Add)]
        public async Task<AdvertisementDto> Add(AdvertisementCreateOrUpdateDto advertisementCreateOrUpdateDto)
        {
            var validationResult = await _advertisementValidator.ValidateAsync(advertisementCreateOrUpdateDto, Options => Options.IncludeRuleSets(RuleSets.Add));
            if (!validationResult.IsValid)
            {
                var ex = new ValidationException(validationResult.Errors);
                throw new UserFriendlyException(ex.Message, ValidationConstant.AdvertisementNotFound);
            }

            var advertisement = ObjectMapper.Map<AdvertisementCreateOrUpdateDto, Advertisement>(advertisementCreateOrUpdateDto);
            var entity = await _advertisementRepository.InsertAsync(advertisement);
            await CurrentUnitOfWork.SaveChangesAsync();
            return ObjectMapper.Map<Advertisement, AdvertisementDto>(entity);
        }
        [SecuredOperation(AdvertisementServicePermissionConstants.Delete)]
        public async Task<bool> Delete(int id)
        {
            await Validation(id, RuleEnum.Delete);
            await _advertisementRepository.DeleteAsync(x => x.Id == id);
            return true;
        }

        public async Task<AdvertisementDto> GetById(AdvertisementQueryDto advertisementQueryDto)
        {
            var advertisement = await Validation(advertisementQueryDto.Id,null);
            var advertisementDto = ObjectMapper.Map<Advertisement, AdvertisementDto>(advertisement);
            var advertisementDetailIds = advertisementDto.AdvertisementDetails.Select(x => x.Id).ToList();
            var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Advertisement, advertisementDetailIds, EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(advertisementQueryDto.AttachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(advertisementQueryDto.Attachmentlocation));
            advertisementDto.AdvertisementDetails.ForEach(x =>
            {
                var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
                x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
            });
            return advertisementDto;
        }
        public async Task<List<AdvertisementDto>> GetList(List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
        {
            var advertisementQuery = (await _advertisementRepository.GetQueryableAsync()).AsNoTracking().Include(x => x.AdvertisementDetails);
            var advertisement = advertisementQuery.ToList();
            var advertisementDto = ObjectMapper.Map<List<Advertisement>, List<AdvertisementDto>>(advertisement);
            advertisementDto.ForEach(async x =>
            {
                var advertisementDetailIds = x.AdvertisementDetails.Select(x => x.Id).ToList();
                var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Advertisement, advertisementDetailIds, attachmentType, attachmentlocation);
                x.AdvertisementDetails.ForEach(o =>
                {
                    var attachment = attachments.Where(y => y.EntityId == o.Id).ToList();
                    o.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
                });
            });
            return advertisementDto;
        }
        [SecuredOperation(AdvertisementServicePermissionConstants.Update)]
        public async Task<AdvertisementDto> Update(AdvertisementCreateOrUpdateDto advertisementCreateOrUpdateDto)
        {
            var validationResult = await _advertisementValidator.ValidateAsync(advertisementCreateOrUpdateDto, Options => Options.IncludeRuleSets(RuleSets.Edit));
            if (!validationResult.IsValid)
            {
                var ex = new ValidationException(validationResult.Errors);
                throw new UserFriendlyException(ex.Message, ValidationConstant.ItemNotFound);
            }
            var advertisement = (await _advertisementRepository.GetQueryableAsync()).AsNoTracking()
                                        .FirstOrDefault(x => x.Id == advertisementCreateOrUpdateDto.Id);
            var advertisementMap = ObjectMapper.Map<AdvertisementCreateOrUpdateDto, Advertisement>(advertisementCreateOrUpdateDto, advertisement);

            var result = await _advertisementRepository.UpdateAsync(advertisementMap);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<Advertisement, AdvertisementDto>(result);
        }

        private async Task<Advertisement> Validation(int id, RuleEnum? rule)
        {
            var advertisementQuery = (await _advertisementRepository.GetQueryableAsync()).AsNoTracking().Include(x => x.AdvertisementDetails);
            var advertisement = advertisementQuery.FirstOrDefault(x => x.Id == id);
            if (advertisement is null)
                throw new UserFriendlyException(OrderConstant.AdvertisementNotFound, OrderConstant.AdvertisementNotFoundId);
            if (rule== RuleEnum.Delete)
            {
             if (advertisement.AdvertisementDetails.Count>0 )
                {
                    throw new UserFriendlyException(OrderConstant.AdvertisementDelete, OrderConstant.AdvertisementDeleteId);
                }
            }
           

            return advertisement;
        }
       

    }
}
