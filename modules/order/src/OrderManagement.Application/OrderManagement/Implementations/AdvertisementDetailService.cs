using Esale.Share.Authorize;
using FluentValidation;
using IFG.Core.DataAccess;
using IFG.Core.Utility.Tools;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Constants;
using OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.OrderManagement.FluentValidation;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class AdvertisementDetailService : ApplicationService, IAdvertisementDetailService
    {
        private readonly IRepository<AdvertisementDetail> _advertisementDetailRepository;
        private readonly IAttachmentService _attachmentService;
        private readonly IValidator<AdvertisementDetailCreateOrUpdateDto> _advertisementDetailValidator;
        private readonly IValidator<AdvertisementDetailWithIdDto> _advertisementDetailWithIdValidator;

        public AdvertisementDetailService(IRepository<AdvertisementDetail> advertisementDetailRepository, IAttachmentService attachmentService,
            IValidator<AdvertisementDetailCreateOrUpdateDto> advertisementDetailValidator
            , IValidator<AdvertisementDetailWithIdDto> advertisementDetailWithIdValidator)
        {
            _advertisementDetailRepository = advertisementDetailRepository;
            _attachmentService = attachmentService;
            _advertisementDetailValidator = advertisementDetailValidator;
            _advertisementDetailWithIdValidator= advertisementDetailWithIdValidator;
         

        }
        [SecuredOperation(AdvertisementDetailServicePermissionConstants.Add)]
        public async Task<AdvertisementDetailDto> Add(AdvertisementDetailCreateOrUpdateDto advertisementDetailCreateOrUpdateDto)
        {
            var validationResult = await _advertisementDetailValidator.ValidateAsync(advertisementDetailCreateOrUpdateDto, Options => Options.IncludeRuleSets(RuleSets.Add));
            if (!validationResult.IsValid)
            {
                var ex = new ValidationException(validationResult.Errors);
                throw new UserFriendlyException(ex.Message, ValidationConstant.ItemNotFoundId);
            }
            var advertisementDetail = (await _advertisementDetailRepository.GetQueryableAsync()).AsNoTracking().OrderByDescending(x => x.Priority).FirstOrDefault(x => x.AdvertisementId == advertisementDetailCreateOrUpdateDto.AdvertisementId);
            var advertisementDetailMap = ObjectMapper.Map<AdvertisementDetailCreateOrUpdateDto, AdvertisementDetail>(advertisementDetailCreateOrUpdateDto);
            if (advertisementDetail == null)
            {
                advertisementDetailMap.Priority = 1;
            }
            else
            {
                advertisementDetailMap.Priority = advertisementDetail.Priority + 1;
            }
            var entity = await _advertisementDetailRepository.InsertAsync(advertisementDetailMap);
            await CurrentUnitOfWork.SaveChangesAsync();
            return ObjectMapper.Map<AdvertisementDetail, AdvertisementDetailDto>(entity);
        }
        [SecuredOperation(AdvertisementDetailServicePermissionConstants.Delete)]
        public async Task<bool> Delete(int id)
        {
            await Validation(id);
            await _advertisementDetailRepository.DeleteAsync(x => x.Id == id);
            return true;
        }

        public async Task<AdvertisementDetailDto> GetById(int id, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
        {
           var advertisementDetail =await Validation(id);
            var advertisementDetailDto = ObjectMapper.Map<AdvertisementDetail, AdvertisementDetailDto>(advertisementDetail);
            var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Advertisement, new List<int>() { id }, attachmentType, attachmentlocation);
            advertisementDetailDto.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachments);
            return advertisementDetailDto;
        }

        public async Task<List<AdvertisementDetailDto>> GetList(int advertisementId, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
        {
            var advertisementDetailQuery = await _advertisementDetailRepository.GetQueryableAsync();
            var advertisementDetail = advertisementDetailQuery.Where(x => x.AdvertisementId == advertisementId).ToList();
            var advertisementDetailDto = ObjectMapper.Map<List<AdvertisementDetail>, List<AdvertisementDetailDto>>(advertisementDetail);
            var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Advertisement, advertisementDetail.Select(x => x.Id).ToList(), attachmentType, attachmentlocation);
            advertisementDetailDto.ForEach(x =>
            {
                var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
                x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
            });
            return advertisementDetailDto;
        }
        public async Task<PagedResultDto<AdvertisementDetailDto>> GetPagination(AdvertisementDetailPaginationDto input)
        {
            var advertisementDetailQuery = (await _advertisementDetailRepository.GetQueryableAsync()).AsNoTracking();
            var advertisementDetailResult = advertisementDetailQuery.Where(x => x.AdvertisementId == input.AdvertisementId);
            var count = advertisementDetailResult.Count();
            var advertisementDetails = advertisementDetailResult
               .PageBy(input)
               .SortByRule(input)
               .ToList();
            var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Advertisement, advertisementDetails.Select(x => x.Id).ToList()
                                                        , EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(input.AttachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(input.AttachmentLocation));
            var advertisementDetail = ObjectMapper.Map<List<AdvertisementDetail>, List<AdvertisementDetailDto>>(advertisementDetails);
            advertisementDetail.ForEach(x =>
            {
                var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
                x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
            });
            return new PagedResultDto<AdvertisementDetailDto>
            {
                TotalCount = count,
                Items = advertisementDetail
            };

        }

        [SecuredOperation(AdvertisementDetailServicePermissionConstants.Update)]
        public async Task<AdvertisementDetailDto> Update(AdvertisementDetailCreateOrUpdateDto advertisementDetailCreateOrUpdateDto)
        {
            var validationResult = await _advertisementDetailValidator.ValidateAsync(advertisementDetailCreateOrUpdateDto, Options => Options.IncludeRuleSets(RuleSets.Edit));
            if (!validationResult.IsValid)
            {
                var ex = new ValidationException(validationResult.Errors);
                throw new UserFriendlyException(ex.Message, ValidationConstant.ItemNotFound);
            }
            var advertisementDetail = (await _advertisementDetailRepository.GetQueryableAsync())
            .FirstOrDefault(x => x.Id == advertisementDetailCreateOrUpdateDto.Id);
            var advertisementDetailMap = ObjectMapper.Map<AdvertisementDetailCreateOrUpdateDto, AdvertisementDetail>(advertisementDetailCreateOrUpdateDto, advertisementDetail);
            var result = await _advertisementDetailRepository.UpdateAsync(advertisementDetailMap);
            await CurrentUnitOfWork.SaveChangesAsync();
            return ObjectMapper.Map<AdvertisementDetail, AdvertisementDetailDto>(result);
        }
        [SecuredOperation(AdvertisementDetailServicePermissionConstants.UploadFile)]
        public async Task<Guid> UploadFile(UploadFileDto uploadFile)
        {


            var validationResult = await _advertisementDetailWithIdValidator.ValidateAsync(new AdvertisementDetailWithIdDto { Id= uploadFile.Id }, Options => Options.IncludeRuleSets(RuleSets.UploadFile));
            if (!validationResult.IsValid)
            {
                var ex = new ValidationException(validationResult.Errors);
                throw new UserFriendlyException(ex.Message, ValidationConstant.ItemNotFoundId);
            }
            return await _attachmentService.UploadFile(AttachmentEntityEnum.Advertisement, uploadFile);
        }
        [SecuredOperation(AdvertisementDetailServicePermissionConstants.Move)]
        public async Task<bool> Move(AdvertisementDetailWithIdDto advertisementDetailWithId)
        {
            var validationResult = await _advertisementDetailWithIdValidator.ValidateAsync(advertisementDetailWithId, Options => Options.IncludeRuleSets(RuleSets.Move));
            if (!validationResult.IsValid)
            {
                var ex = new ValidationException(validationResult.Errors);
                throw new UserFriendlyException(ex.Message, ValidationConstant.QuestionnerNotFound);
            }
            var advertisementDetailQuery = (await _advertisementDetailRepository.GetQueryableAsync()).AsNoTracking().OrderBy(x=>x.Priority);
            var currentAdvertisementDetail = advertisementDetailQuery.FirstOrDefault(x => x.Id == advertisementDetailWithId.Id);
            var currentPriority = currentAdvertisementDetail.Priority;
            var parentId = currentAdvertisementDetail.AdvertisementId;
            if (MoveTypeEnum.Up == advertisementDetailWithId.MoveType)
            {
                var previousAdvertisementDetail = await advertisementDetailQuery.FirstOrDefaultAsync(x => x.Priority == currentAdvertisementDetail.Priority-1 && x.AdvertisementId == parentId);
                if (previousAdvertisementDetail is null)
                {
                    throw new UserFriendlyException(OrderConstant.FirstPriority, OrderConstant.FirstPriorityId);
                }
                var previousPriority = previousAdvertisementDetail.Priority;
                currentAdvertisementDetail.Priority = previousPriority;
                await _advertisementDetailRepository.UpdateAsync(currentAdvertisementDetail);
                previousAdvertisementDetail.Priority = currentPriority;
                await _advertisementDetailRepository.UpdateAsync(previousAdvertisementDetail);
            }
            else if (MoveTypeEnum.Down == advertisementDetailWithId.MoveType)
            {
                var nextAdvertisementDetail = advertisementDetailQuery.FirstOrDefault(x => x.Priority > currentAdvertisementDetail.Priority && x.AdvertisementId == parentId);
                if (nextAdvertisementDetail is null)
                {
                    throw new UserFriendlyException(OrderConstant.LastPriority, OrderConstant.LastPriorityId);
                }
                var nextPriority = nextAdvertisementDetail.Priority;
                currentAdvertisementDetail.Priority = nextPriority;
                await _advertisementDetailRepository.UpdateAsync(currentAdvertisementDetail);
                nextAdvertisementDetail.Priority = currentPriority;
                await _advertisementDetailRepository.UpdateAsync(nextAdvertisementDetail);
            }


            return true;
        }


        private async Task<AdvertisementDetail> Validation(int id)
        {
            var advertisementDetailQuery = (await _advertisementDetailRepository.GetQueryableAsync()).AsNoTracking();
            var advertisementDetail = advertisementDetailQuery.FirstOrDefault(x => x.Id == id);
            if (advertisementDetail is null)
                throw new UserFriendlyException(OrderConstant.AdvertisementDetailNotFound, OrderConstant.AdvertisementDetailNotFoundId);
           
            return advertisementDetail;
        }


    }
}




      
