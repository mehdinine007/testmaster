using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain.OrderManagement;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using Volo.Abp;
using OrderManagement.Domain.Shared;
using System;
using Core.Utility.Tools;
using OrderManagement.Application.Contracts.OrderManagement;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Nest;
using Newtonsoft.Json;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class ProductAndCategoryService : ApplicationService, IProductAndCategoryService
{
    private readonly IRepository<ProductAndCategory, int> _productAndCategoryRepository;
    private readonly IAttachmentService _attachmentService;
    private readonly IProductPropertyService _productPropertyService;
    public ProductAndCategoryService(IRepository<ProductAndCategory, int> productAndCategoryRepository, 
                                     IAttachmentService attachmentService, 
                                     IProductPropertyService productPropertyService
        )
    {
        _productAndCategoryRepository = productAndCategoryRepository;
        _attachmentService = attachmentService;
        _productPropertyService = productPropertyService;
    }

    public async Task Delete(int id)
    {
        var productCategory = await GetById(id);
        if (productCategory.Type == ProductAndCategoryType.Category)
        {
            var firstDependentCategory = (await _productAndCategoryRepository.GetQueryableAsync())
                .Select(x => new { x.ParentId, x.Type })
                .FirstOrDefault(x => x.Type == ProductAndCategoryType.Category && x.ParentId == productCategory.Id);
            if (firstDependentCategory != null)
                throw new UserFriendlyException("این دسته بندی در حال استفاده است");
        }
        await _productAndCategoryRepository.DeleteAsync(id);
    }

    public async Task<ProductAndCategoryDto> GetById(int id)
    {
        var productCategory = (await _productAndCategoryRepository.GetQueryableAsync())
            .FirstOrDefault(x => x.Id == id) ??
            throw new UserFriendlyException("محصول یا دسته بندی مورد نطر پیدا نشد");
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, new List<int>() { id });
        var propertyCategories = await _productPropertyService.GetByProductId(id);
        var productResult = ObjectMapper.Map<ProductAndCategory, ProductAndCategoryDto>(productCategory);
        productResult.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachments);
        productResult.PropertyCategories = propertyCategories;
        return productResult;
    }

    public async Task<ProductAndCategoryDto> Insert(ProductAndCategoryDto productAndCategoryDto)
    {
        if (productAndCategoryDto.ParentId.HasValue && productAndCategoryDto.ParentId.Value > 0)
        {
            var parent = (await _productAndCategoryRepository.GetQueryableAsync())
                .FirstOrDefault(x => x.Id == productAndCategoryDto.ParentId)
                ?? throw new UserFriendlyException("دسته بندی سطح بالایی یافت نشد");
            if (parent.Type == ProductAndCategoryType.Product)
                throw new UserFriendlyException("یک محصول نمیتواند به عنوان دسته بندی سطح بالایی انتخاب شود");

            if (productAndCategoryDto.Type == ProductAndCategoryType.Product)
            {
                var parentFirstCategorychild = (await _productAndCategoryRepository.GetQueryableAsync())
                    .FirstOrDefault(x => x.ParentId == parent.Id && x.Type == ProductAndCategoryType.Category);
                if (parentFirstCategorychild != null)
                    throw new UserFriendlyException("برای دسته بندی سطح بالایی قبلا زیر سطح از نوع دسته بندی تعریف شده است");
            }
            else if (productAndCategoryDto.Type == ProductAndCategoryType.Category)
            {
                var parentFirstProductChild = (await _productAndCategoryRepository.GetQueryableAsync())
                    .FirstOrDefault(x => x.ParentId == parent.Id && x.Type == ProductAndCategoryType.Product);
                if (parentFirstProductChild != null)
                    throw new UserFriendlyException("برای دسته بندی سطح بالایی قبلا زیر سطح از نوع محصول تعریف شده است");
            }
            productAndCategoryDto.LevelId = parent.LevelId + 1;
        }
        else
            productAndCategoryDto.LevelId = 1;
        var _parentCode = "";
        var igResult = await _productAndCategoryRepository.GetQueryableAsync();
        int codeLength = 4;
        if (productAndCategoryDto.ParentId.HasValue && productAndCategoryDto.ParentId.Value > 0)
            _parentCode = igResult.FirstOrDefault(x => x.Id == productAndCategoryDto.ParentId).Code;

        var _maxCode = igResult
            .Where(x => x.ParentId == productAndCategoryDto.ParentId)
            .Max(x => x.Code);
        if (!string.IsNullOrWhiteSpace(_maxCode))
            _maxCode = (Convert.ToInt32(_maxCode.Substring(_maxCode.Length - codeLength)) + 1).ToString();
        else _maxCode = "1";
        _maxCode = _parentCode + StringHelper.Repeat(_maxCode, codeLength);
        productAndCategoryDto.Code = _maxCode;
        productAndCategoryDto.ParentId = productAndCategoryDto.ParentId.HasValue && productAndCategoryDto.ParentId.Value > 0
            ? productAndCategoryDto.ParentId.Value
            : null;
        productAndCategoryDto.Active = true;
        var entity = await _productAndCategoryRepository.InsertAsync(
            ObjectMapper.Map<ProductAndCategoryDto, ProductAndCategory>(productAndCategoryDto));
        return ObjectMapper.Map<ProductAndCategory, ProductAndCategoryDto>(entity);
    }

    public async Task<bool> UploadFile(UploadFileDto uploadFileDto)
    {
        var attachmentStatus = await _attachmentService.UploadFile(new AttachFileDto()
        {
            Entity = AttachmentEntityEnum.ProductAndCategory,
            EntityId = uploadFileDto.Id,
            EntityType = uploadFileDto.Type,
            File = uploadFileDto.File,
        });
        return attachmentStatus;
    }

    public async Task<CustomPagedResultDto<ProductAndCategoryDto>> GetListWithPagination(ProductAndCategoryQueryDto input)
    {
        var productCategoryQuery = await _productAndCategoryRepository.WithDetailsAsync(x => x.Childrens);
        productCategoryQuery = productCategoryQuery.AsSingleQuery();
        productCategoryQuery = productCategoryQuery.Where(x => x.ParentId == input.ParentId);

        var totalCount = await productCategoryQuery.CountAsync();
        if (totalCount <= 0)
            return new CustomPagedResultDto<ProductAndCategoryDto>(new ProductAndCategoryDto[0], 0);

        var queryResult = productCategoryQuery.PageBy(input).ToList();

        //var attachmentQuery = await _attachmentRepository.GetQueryableAsync();
        //if (input.AttachmentEntityType > 0)
        //    attachmentQuery = attachmentQuery.Where(x => x.EntityType == (AttachmentEntityTypeEnum)input.AttachmentEntityType);
        //var ids = queryResult.Select(x => x.Id).ToList();
        //attachmentQuery = attachmentQuery.Where(x => x.Entity == AttachmentEntityEnum.ProductAndCategory && ids.Any(y => y == x.EntityId));
        //var attachments = attachmentQuery.ToList();

        var ids = queryResult.Select(x => x.Id).ToList();
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, ids, input.AttachmentEntityType);

        var resultList = ObjectMapper.Map<List<ProductAndCategory>, List<ProductAndCategoryDto>>(queryResult);
        resultList.ForEach(x =>
        {
            var pacAttachments = attachments.Where(y => y.EntityId == x.Id).ToList();
            x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(pacAttachments);
        });
        return new CustomPagedResultDto<ProductAndCategoryDto>(resultList, totalCount);
    }

    public async Task<List<ProductAndCategoryWithChildDto>> GetList(ProductAndCategoryGetListQueryDto input)
    {
        List<ProductAndCategory> ls = new();
        var productAndCategoryQuery = await _productAndCategoryRepository.GetQueryableAsync();
        switch (input.Type)
        {
            case ProductAndCategoryType.Category:
                var parent = productAndCategoryQuery
                    .Include(x => x.Childrens.Where(y => y.Type == ProductAndCategoryType.Category))
                    .Where(x => EF.Functions.Like(x.Code, input.NodePath + "%") && x.Type == ProductAndCategoryType.Category)
                    .ToList();
                ls = parent.Where(x => x.Code == input.NodePath).ToList();
                break;
            case ProductAndCategoryType.Product:
                ls = productAndCategoryQuery.Where(x => EF.Functions.Like(x.Code, input.NodePath+"%") && x.Type == ProductAndCategoryType.Product).ToList();
                break;
        }
        return ObjectMapper.Map<List<ProductAndCategory>, List<ProductAndCategoryWithChildDto>>(ls);
    }
}
