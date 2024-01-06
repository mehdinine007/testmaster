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

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class AdvertisementDetailService : ApplicationService, IAdvertisementDetailService
    {
        private readonly IRepository<AdvertisementDetail> _advertisementDetailRepository;
        private readonly IAttachmentService _attachmentService;
        private readonly IValidator<AdvertisementDetailCreateOrUpdateDto> _advertisementDetailValidator;

        public AdvertisementDetailService(IRepository<AdvertisementDetail> advertisementDetailRepository, IAttachmentService attachmentService,
            IValidator<AdvertisementDetailCreateOrUpdateDto> advertisementDetailValidator)
        {
            _advertisementDetailRepository = advertisementDetailRepository;
            _attachmentService = attachmentService;
            _advertisementDetailValidator = advertisementDetailValidator;
         ;

        }
        [SecuredOperation(AdvertisementDetailServicePermissionConstants.Add)]
        public async Task<AdvertisementDetailDto> Add(AdvertisementDetailCreateOrUpdateDto advertisementDetailCreateOrUpdateDto)
        {
            var validationResult = await _advertisementDetailValidator.ValidateAsync(advertisementDetailCreateOrUpdateDto, Options => Options.IncludeRuleSets(RuleSets.Add));
            if (!validationResult.IsValid)
            {
                var ex = new ValidationException(validationResult.Errors);
                throw new UserFriendlyException(ex.Message, ValidationConstant.QuestionnerNotFound);
            }
            var advertisementDetail = (await _advertisementDetailRepository.GetQueryableAsync()).OrderByDescending(x => x.Priority).FirstOrDefault(x=>x.AdvertisementId== advertisementDetailCreateOrUpdateDto.Id);   
            var entity = await _advertisementDetailRepository.InsertAsync(advertisementDetail);
            return ObjectMapper.Map<AdvertisementDetail, AdvertisementDetailDto>(entity);
        }
        [SecuredOperation(AdvertisementDetailServicePermissionConstants.Delete)]
        public async Task<bool> Delete(int id)
        {
            await _advertisementDetailRepository.DeleteAsync(x => x.Id == id);
            return true;
        }

        public async Task<AdvertisementDetailDto> GetById(int id, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
        {
            var advertisementDetail = (await _advertisementDetailRepository.GetQueryableAsync()).AsNoTracking()
               .FirstOrDefault(x => x.Id == id);
            var advertisementDetailDto = ObjectMapper.Map<AdvertisementDetail, AdvertisementDetailDto>(advertisementDetail);
            var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Advertisement, new List<int>() { id }, attachmentType, attachmentlocation);
            advertisementDetailDto.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachments);
            return advertisementDetailDto;
        }

        public async Task<List<AdvertisementDetailDto>> GetList(int advertisementId, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null)
        {
            var advertisementDetailQuery = await _advertisementDetailRepository.GetQueryableAsync();
            var advertisementDetail = advertisementDetailQuery.Where(x=>x.AdvertisementId== advertisementId).ToList();
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
           var advertisementDetailResult= advertisementDetailQuery.Where(x=>x.AdvertisementId== input.AdvertisementId);
                        var count = advertisementDetailResult.Count();
            var advertisementDetails = advertisementDetailResult
               .PageBy(input)
               .SortByRule(input)
               .ToList();
            var attachments = await _attachmentService.GetList(AttachmentEntityEnum.Announcement, advertisementDetails.Select(x => x.Id).ToList()
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

            advertisementDetail.Title = advertisementDetailCreateOrUpdateDto.Title;

            var result = await _advertisementDetailRepository.UpdateAsync(advertisementDetail);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<AdvertisementDetail, AdvertisementDetailDto>(result);
        }
        [SecuredOperation(AdvertisementDetailServicePermissionConstants.UploadFile)]
        public async Task<Guid> UploadFile(UploadFileDto uploadFile)
        {
            return await _attachmentService.UploadFile(AttachmentEntityEnum.Advertisement, uploadFile);
        }

    }


}
