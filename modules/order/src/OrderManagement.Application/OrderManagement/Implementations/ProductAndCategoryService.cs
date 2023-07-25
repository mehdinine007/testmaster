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

namespace OrderManagement.Application.OrderManagement.Implementations;

public partial class ProductAndCategoryService : ApplicationService, IProductAndCategoryService
{
    private readonly IRepository<ProductAndCategory, int> _productAndCategoryRepository;

    public ProductAndCategoryService(IRepository<ProductAndCategory, int> productAndCategoryRepository)
    {
        _productAndCategoryRepository = productAndCategoryRepository;
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
                throw new UserFriendlyException("این دسته بندی قابل حذف نمیباشد");
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
        if (productAndCategoryDto.ParentId.HasValue)
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
        }
        var _parentCode = "";
        var igResult = await _productAndCategoryRepository.GetQueryableAsync();
        int codeLength = 4;
        if (productAndCategoryDto.ParentId != null)
            _parentCode = igResult.FirstOrDefault(x => x.Id == productAndCategoryDto.ParentId).Code;

        var _maxCode = igResult
            .Where(x => x.ParentId == productAndCategoryDto.ParentId)
            .Max(x => x.Code);
        if (!string.IsNullOrWhiteSpace(_maxCode))
            _maxCode = (Convert.ToInt32(_maxCode.Substring(_maxCode.Length - codeLength)) + 1).ToString();
        else _maxCode = "1";
        _maxCode = _parentCode + StringHelper.Repeat(_maxCode, codeLength);
        productAndCategoryDto.Code = _maxCode;
        var entity = await _productAndCategoryRepository.InsertAsync(
            ObjectMapper.Map<ProductAndCategoryDto, ProductAndCategory>(productAndCategoryDto));
        return ObjectMapper.Map<ProductAndCategory, ProductAndCategoryDto>(entity);
    }
}
