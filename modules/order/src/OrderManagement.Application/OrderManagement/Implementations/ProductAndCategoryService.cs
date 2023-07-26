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

namespace OrderManagement.Application.OrderManagement.Implementations;

public partial class ProductAndCategoryService : ApplicationService, IProductAndCategoryService
{
    private readonly IRepository<ProductAndCategory, int> _productAndCategoryRepository;
    private readonly IAttachmentService _attachmentService;
    public ProductAndCategoryService(IRepository<ProductAndCategory, int> productAndCategoryRepository,IAttachmentService attachmentService)
    {
        _productAndCategoryRepository = productAndCategoryRepository;
        _attachmentService = attachmentService;
    }

    public async Task Delete(int id)
    {
        var productCategory = await Get(id);
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

    public async Task<ProductAndCategoryDto> Get(int id)
    {
        var productCategory = (await _productAndCategoryRepository.GetQueryableAsync())
            .FirstOrDefault(x => x.Id == id) ??
            throw new UserFriendlyException("محصول یا دسته بندی مورد نطر پیدا نشد");
        return ObjectMapper.Map<ProductAndCategory, ProductAndCategoryDto>(productCategory);
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
            EntityType = uploadFileDto.AttachmentEntityTypeEnum,
            File = uploadFileDto.File,
        });
        return attachmentStatus;
    }

    public async Task<CustomPagedResultDto<ProductAndCategoryDto>> GetListWithPagination(ProductAndCategoryQueryDto input)
    {
        var productCategoryQuery = await _productAndCategoryRepository.WithDetailsAsync(x => x.Childrens);
        productCategoryQuery = productCategoryQuery.AsSingleQuery();
        if (input.ParentId > 0)
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
}
