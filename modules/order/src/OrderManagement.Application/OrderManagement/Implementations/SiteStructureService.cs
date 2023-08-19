﻿using Nest;
using Newtonsoft.Json;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class SiteStructureService : ApplicationService, ISiteStructureService
    {
        private readonly IRepository<SiteStructure, int> _siteStructureRepository;
        private readonly IAttachmentService _attachmentService;
        private readonly ICarClassService _carClassService;
        private readonly IProductAndCategoryService _productAndCategoryService;
        private readonly IRepository<ESaleType, int> _eSaleTypeRepository;
        public SiteStructureService(IRepository<SiteStructure, int> siteStructureRepository, IAttachmentService attachmentService, ICarClassService carClassService, IProductAndCategoryService productAndCategoryService, IRepository<ESaleType, int> eSaleTypeRepository)
        {
            _siteStructureRepository = siteStructureRepository;
            _attachmentService = attachmentService;
            _carClassService = carClassService;
            _productAndCategoryService = productAndCategoryService;
            _eSaleTypeRepository = eSaleTypeRepository;
        }
        public async Task<SiteStructureDto> Add(SiteStructureAddOrUpdateDto siteStructureDto)
        {
            var siteStructureQuery = await _siteStructureRepository.GetQueryableAsync();
            var siteStructure = siteStructureQuery
                .ToList();
            var _siteStructure = ObjectMapper.Map<SiteStructureAddOrUpdateDto, SiteStructure>(siteStructureDto);
            if (siteStructureDto.Priority == 0)
            {
                var _priority = 1;
                if (siteStructure != null && siteStructure.Count > 1)
                    _priority = siteStructure.Max(x => x.Priority) + 1;
                _siteStructure.Priority = _priority;
            }
            _siteStructure = await _siteStructureRepository.InsertAsync(_siteStructure, autoSave: true);
            return await GetById(_siteStructure.Id);
        }
        public async Task<SiteStructureDto> Update(SiteStructureAddOrUpdateDto siteStructureDto)
        {
            var siteStructure = await Validation(siteStructureDto.Id, siteStructureDto);
            siteStructure.Title = siteStructureDto.Title;
            siteStructure.Description = siteStructureDto.Description;
            siteStructure.Type = siteStructureDto.Type;
            siteStructure.Priority = siteStructureDto.Priority;
            await _siteStructureRepository.UpdateAsync(siteStructure, autoSave: true);
            return await GetById(siteStructure.Id);
        }

        public async Task<bool> Delete(int id)
        {
            await Validation(id, null);
            await _siteStructureRepository.DeleteAsync(x => x.Id == id);
            await _attachmentService.DeleteByEntityId(AttachmentEntityEnum.SiteStructure, id);
            return true;
        }

        private async Task<SiteStructure> Validation(int id, SiteStructureAddOrUpdateDto siteStructureDto)
        {
            var siteStructure = (await _siteStructureRepository.GetQueryableAsync())
                .FirstOrDefault(x => x.Id == id);
            if (siteStructure is null)
            {
                throw new UserFriendlyException(OrderConstant.SiteStuctureNotFound, OrderConstant.SiteStuctureNotFoundId);
            }
            return siteStructure;
        }

        public  async Task<SiteStructureDto> GetById(int id)
        {
            var siteStructure = (await _siteStructureRepository.GetQueryableAsync())
                .FirstOrDefault(x => x.Id == id);
            return ObjectMapper.Map<SiteStructure, SiteStructureDto>(siteStructure);
        }

        public async Task<List<SiteStructureDto>> GetList(SiteStructureQueryDto siteStructureQuery)
        {
            var getSiteStructures = (await _siteStructureRepository.GetQueryableAsync())
                .Where(x=>x.Location== siteStructureQuery.Location)
                .OrderBy(x=> x.Priority)
                .ToList();
            var attachments = await _attachmentService.GetList(AttachmentEntityEnum.SiteStructure, getSiteStructures.Select(x => x.Id).ToList(), siteStructureQuery.AttachmentType);
            var siteStructures = ObjectMapper.Map<List<SiteStructure>, List<SiteStructureDto>>(getSiteStructures);
            siteStructures.ForEach(async x =>
            {
                var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
                x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
                if (x.Type == SiteStructureTypeEnum.ProductCarousel)
                {
                    var products = await _productAndCategoryService.GetList(JsonConvert.DeserializeObject<ProductAndCategoryGetListQueryDto>(x.Content));
                    x.CarouselData = products.Select(x => new CarouselData()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Title = x.Title,
                        Attachments = x.Attachments
                    }).ToList();
                }
                if (x.Type == SiteStructureTypeEnum.CarClassCarousel)
                {
                    var carclass = await _carClassService.GetList(siteStructureQuery.AttachmentType);
                    x.CarouselData = carclass.Select(x => new CarouselData()
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Attachments = x.Attachments
                    }).ToList();
                }
                if (x.Type == SiteStructureTypeEnum.EsaleType)

                {
                    var eslaType = (await _eSaleTypeRepository.GetQueryableAsync()).ToList();
                    x.CarouselData = eslaType.Select(x => new CarouselData()
                    {
                        Id = x.Id,
                        Title = x.SaleTypeName,
                    }).ToList();
                }
                x.Content = null;
            });
            return siteStructures;
        }

        public async Task<bool> UploadFile(UploadFileDto uploadFile)
        {
            await _attachmentService.UploadFile(AttachmentEntityEnum.SiteStructure, uploadFile);
            return true;
        }
    }
}
