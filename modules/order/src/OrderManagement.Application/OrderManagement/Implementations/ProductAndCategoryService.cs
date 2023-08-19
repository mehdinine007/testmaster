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
using OrderManagement.Application.Contracts.Services;
using Nest;
using OrderManagement.Domain;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using System.ComponentModel;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class ProductAndCategoryService : ApplicationService, IProductAndCategoryService
{
    private readonly IRepository<ProductAndCategory, int> _productAndCategoryRepository;
    private readonly IAttachmentService _attachmentService;
    private readonly IProductPropertyService _productPropertyService;
    private readonly IRepository<ProductLevel, int> _productLevelRepository;
    private readonly ICommonAppService _commonAppService;
    private readonly IRepository<ProductProperty, ObjectId> _productPropertyRepository;

    public ProductAndCategoryService(IRepository<ProductAndCategory, int> productAndCategoryRepository,
                                     IAttachmentService attachmentService,
                                     IProductPropertyService productPropertyService,
                                     IRepository<ProductLevel, int> productLevelRepository,
                                     ICommonAppService commonAppService
,
                                     IRepository<ProductProperty, ObjectId> productPropertyRepository)
    {
        _productAndCategoryRepository = productAndCategoryRepository;
        _attachmentService = attachmentService;
        _productPropertyService = productPropertyService;
        _productLevelRepository = productLevelRepository;
        _commonAppService = commonAppService;
        _productPropertyRepository = productPropertyRepository;
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
        await _attachmentService.DeleteByEntityId(AttachmentEntityEnum.ProductAndCategory, id);
    }

    public async Task<ProductAndCategoryDto> GetById(int id)
    {
        var query = await _productAndCategoryRepository.GetQueryableAsync();
        if (!_commonAppService.IsInRole("Admin"))
            query = query.Where(x => x.Active);
        var productCategory = query
            .FirstOrDefault(x => x.Id == id) ??
            throw new UserFriendlyException("محصول یا دسته بندی مورد نطر پیدا نشد");
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, new List<int>() { id });
        var propertyCategories = await _productPropertyService.GetByProductId(id);
        var productResult = ObjectMapper.Map<ProductAndCategory, ProductAndCategoryDto>(productCategory);
        productResult.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachments);
        productResult.PropertyCategories = propertyCategories;
        return productResult;
    }

    public async Task<ProductAndCategoryDto> Insert(ProductAndCategoryCreateDto productAndCategoryCreateDto)
    {
        var levelId = 0;
        var productLevelId = 0;
        var code = "";
        var productLevelQuery = (await _productLevelRepository.GetQueryableAsync()).OrderBy(x => x.Priority);

        if (productAndCategoryCreateDto.Priority == 0)
        {
            var lastPriority = (await _productAndCategoryRepository.GetQueryableAsync()).OrderByDescending(x => x.Priority)
                          .FirstOrDefault(x => x.ParentId == productAndCategoryCreateDto.ParentId);
            if (lastPriority == null)
            {
                productAndCategoryCreateDto.Priority = 1;
            }
            else
                productAndCategoryCreateDto.Priority = lastPriority.Priority + 1;

        }
        else
        {
            var duplicatePriority = (await _productAndCategoryRepository.GetQueryableAsync()).
                FirstOrDefault(x => x.ParentId == productAndCategoryCreateDto.ParentId && x.Priority == productAndCategoryCreateDto.Priority);
            if (duplicatePriority != null)
            {
                throw new UserFriendlyException(OrderConstant.DuplicatePriority, OrderConstant.DuplicatePriorityId);
            }
        }

        if (productAndCategoryCreateDto.ParentId.HasValue && productAndCategoryCreateDto.ParentId.Value > 0)
        {
            var parent = (await _productAndCategoryRepository.GetQueryableAsync())
                .FirstOrDefault(x => x.Id == productAndCategoryCreateDto.ParentId)
                ?? throw new UserFriendlyException("دسته بندی سطح بالایی یافت نشد");
            if (parent.Type == ProductAndCategoryType.Product)
                throw new UserFriendlyException("یک محصول نمیتواند به عنوان دسته بندی سطح بالایی انتخاب شود");

            if (productAndCategoryCreateDto.Type == ProductAndCategoryType.Product)
            {
                var parentFirstCategorychild = (await _productAndCategoryRepository.GetQueryableAsync())
                    .FirstOrDefault(x => x.ParentId == parent.Id && x.Type == ProductAndCategoryType.Category);
                if (parentFirstCategorychild != null)
                    throw new UserFriendlyException("برای دسته بندی سطح بالایی قبلا زیر سطح از نوع دسته بندی تعریف شده است");
            }
            else if (productAndCategoryCreateDto.Type == ProductAndCategoryType.Category)
            {
                var parentFirstProductChild = (await _productAndCategoryRepository.GetQueryableAsync())
                    .FirstOrDefault(x => x.ParentId == parent.Id && x.Type == ProductAndCategoryType.Product);
                if (parentFirstProductChild != null)
                    throw new UserFriendlyException("برای دسته بندی سطح بالایی قبلا زیر سطح از نوع محصول تعریف شده است");
            }
            levelId = parent.LevelId + 1;
            var parentPriority = productLevelQuery.FirstOrDefault(x => x.Id == parent.ProductLevelId).Priority;
            var currentProductlevel = productLevelQuery.Where(x => x.Priority > parentPriority).FirstOrDefault();
            if (currentProductlevel == null)
            {
                throw new UserFriendlyException(OrderConstant.LastProductLevel, OrderConstant.LastProductLevelId);
            }
            productLevelId = currentProductlevel.Id;
        }
        else
        {
            levelId = 1;
            productLevelId = productLevelQuery.FirstOrDefault().Id;
        }

        var _parentCode = "";
        var igResult = await _productAndCategoryRepository.GetQueryableAsync();
        int codeLength = 4;
        if (productAndCategoryCreateDto.ParentId.HasValue && productAndCategoryCreateDto.ParentId.Value > 0)
            _parentCode = igResult.FirstOrDefault(x => x.Id == productAndCategoryCreateDto.ParentId).Code;

        var _maxCode = igResult
            .Where(x => x.ParentId == productAndCategoryCreateDto.ParentId)
            .Max(x => x.Code);
        if (!string.IsNullOrWhiteSpace(_maxCode))
            _maxCode = (Convert.ToInt32(_maxCode.Substring(_maxCode.Length - codeLength)) + 1).ToString();
        else _maxCode = "1";
        _maxCode = _parentCode + StringHelper.Repeat(_maxCode, codeLength);
        code = _maxCode;
        productAndCategoryCreateDto.ParentId = productAndCategoryCreateDto.ParentId.HasValue && productAndCategoryCreateDto.ParentId.Value > 0
            ? productAndCategoryCreateDto.ParentId.Value
            : null;
        productAndCategoryCreateDto.Active = true;
        var productAndCategory = ObjectMapper.Map<ProductAndCategoryCreateDto, ProductAndCategory>(productAndCategoryCreateDto);
        productAndCategory.LevelId = levelId;
        productAndCategory.ProductLevelId = productLevelId;
        productAndCategory.Code = code;
        var entity = await _productAndCategoryRepository.InsertAsync(productAndCategory,autoSave:true);
        return ObjectMapper.Map<ProductAndCategory, ProductAndCategoryDto>(entity);
    }

    public async Task<Guid> UploadFile(UploadFileDto uploadFileDto)
    {
        var attachmentStatus = await _attachmentService.UploadFile(AttachmentEntityEnum.ProductAndCategory, uploadFileDto);
        return attachmentStatus;
    }

    public async Task<CustomPagedResultDto<ProductAndCategoryDto>> GetListWithPagination(ProductAndCategoryQueryDto input)
    {
        var productCategoryQuery = await _productAndCategoryRepository.WithDetailsAsync(x => x.Childrens);
        if (!_commonAppService.IsInRole("Admin"))
            productCategoryQuery = productCategoryQuery.Where(x => x.Active);

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
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, ids, input.AttachmentType);

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
        if (!_commonAppService.IsInRole("Admin"))
            productAndCategoryQuery = productAndCategoryQuery.Include(x=>x.ProductLevel).Where(x => x.Active);
        var attachments = new List<AttachmentDto>();
        switch (input.Type)
        {
            case ProductAndCategoryType.Category:
                var parent = productAndCategoryQuery
                    .Include(x => x.Childrens.Where(y => y.Type == ProductAndCategoryType.Category))
                    .Where(x => EF.Functions.Like(x.Code, input.NodePath + "%") && x.Type == ProductAndCategoryType.Category)
                    .ToList();
                attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, parent.Select(x => x.Id).ToList(), input.attachmentType);
                ls = string.IsNullOrWhiteSpace(input.NodePath)
                    ? parent.Where(x => x.ParentId == null).ToList()
                    : parent.Where(x => x.Code == input.NodePath).ToList();
                if (input.ProductLevelId != null && input.ProductLevelId > 0)
                    ls = ls.Where(x => x.ProductLevelId == input.ProductLevelId)
                        .ToList();
                break;
            case ProductAndCategoryType.Product:
                ls = productAndCategoryQuery.Where(x => EF.Functions.Like(x.Code, input.NodePath + "%") && x.Type == ProductAndCategoryType.Product).ToList();
                if (input.PropertyFilters != null && input.PropertyFilters.Count > 0)
                {
                     ls = await GetProductFilter(input.PropertyFilters, ls);
                }
                attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, ls.Select(x => x.Id).ToList(), input.attachmentType);
                break;
        }
        var productAndCategories = ObjectMapper.Map<List<ProductAndCategory>, List<ProductAndCategoryWithChildDto>>(ls);
        productAndCategories = await FillAttachmentAndProperty(productAndCategories, attachments,input.HasProperty);
        return productAndCategories;
    }

    private async Task<List<ProductAndCategoryWithChildDto>> FillAttachmentAndProperty(List<ProductAndCategoryWithChildDto> productAndCategories, List<AttachmentDto> attachments,bool hasProperty)
    {
        productAndCategories.ForEach(async x =>
        {
            var pacAttachments = attachments.Where(y => y.EntityId == x.Id).ToList();
            x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(pacAttachments);
            if (x.Type == ProductAndCategoryType.Product && hasProperty )
                x.PropertyCategories = await _productPropertyService.GetByProductId(x.Id);
            if (x.Childrens != null && x.Childrens.Count > 0)
                x.Childrens = await FillAttachmentAndProperty(x.Childrens.ToList(), attachments,hasProperty);
        });
        return productAndCategories;
    }
    public async Task<ProductAndCategoryDto> Update(ProductAndCategoryUpdateDto productAndCategoryUpdateDto)
    {
        var productAndCategory = (await _productAndCategoryRepository.GetQueryableAsync()).FirstOrDefault(x => x.Id == productAndCategoryUpdateDto.Id)
            ?? throw new UserFriendlyException("محصول یا دسته بندی یافت نشد");

        productAndCategory.Active = productAndCategoryUpdateDto.Active;
        productAndCategory.Title = productAndCategoryUpdateDto.Title;
        var entity = await _productAndCategoryRepository.UpdateAsync(productAndCategory);
        return ObjectMapper.Map<ProductAndCategory, ProductAndCategoryDto>(entity);
    }

    public async Task<List<ProductAndCategoryWithChildDto>> GetProductAndSaleDetailList(ProductAndSaleDetailGetListQueryDto input)
    {
        List<ProductAndCategory> ProductList = new();
        var currentTime = DateTime.Now;
        var productQuery = await _productAndCategoryRepository.GetQueryableAsync();
        productQuery = productQuery
            .Where(x => x.Active && x.Type == ProductAndCategoryType.Product)
            .Include(x => x.SaleDetails.Where(x => x.SalePlanStartDate <= currentTime && currentTime <= x.SalePlanEndDate && x.Visible))
             .ThenInclude(y => y.ESaleType)
            .Include(x => x.ProductLevel);

        //var product = productQuery
        //          .Where(x => EF.Functions.Like(x.Code, input.NodePath + "%"));
        ProductList = string.IsNullOrWhiteSpace(input.NodePath)
            ? productQuery.ToList()
            : productQuery.Where(x => EF.Functions.Like(x.Code, input.NodePath + "%")).ToList();
        if (input.PropertyFilters != null && input.PropertyFilters.Count > 0)
        {
             ProductList = await GetProductFilter(input.PropertyFilters, ProductList);
        }
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, ProductList.Select(x => x.Id).ToList(), input.attachmentType);

        var productAndSaleDetailListDto = ObjectMapper.Map<List<ProductAndCategory>, List<ProductAndCategoryWithChildDto>>(ProductList);

        productAndSaleDetailListDto = await FillAttachmentAndProperty(productAndSaleDetailListDto, attachments, input.HasProperty);

        return productAndSaleDetailListDto;

    }

    private async Task<List<ProductAndCategory>> GetProductFilter(List<PropertyFilter> input,List<ProductAndCategory> ls)
    {
        List<Object> products = new List<object>();
        List<ProductAndCategory> productAndCategoryList = new List<ProductAndCategory>();
            IMongoCollection<ProductProperty> productPropertyCollection = await _productPropertyRepository.GetCollectionAsync();
            var productPropertyFilter = productPropertyCollection
                .Aggregate()
                .Unwind(x => x.PropertyCategories)
                .Unwind(x => x["PropertyCategories.Properties"]);
            foreach (var filter in input)
            {
                if (filter.Operator == OperatorFilterEnum.Equal)
                    productPropertyFilter = productPropertyFilter
                        .Match(x => x["PropertyCategories.Properties.Key"] == filter.Key && x["PropertyCategories.Properties.Value"] == filter.Value);
                if (filter.Operator == OperatorFilterEnum.EqualOpposite)
                    productPropertyFilter = productPropertyFilter
                        .Match(x => x["PropertyCategories.Properties.Key"] == filter.Key && x["PropertyCategories.Properties.Value"] != filter.Value);
                if (filter.Operator == OperatorFilterEnum.Bigger)
                    productPropertyFilter = productPropertyFilter
                        .Match(x => x["PropertyCategories.Properties.Key"] == filter.Key && x["PropertyCategories.Properties.Value"] > filter.Value);
                if (filter.Operator == OperatorFilterEnum.Smaller)
                    productPropertyFilter = productPropertyFilter
                        .Match(x => x["PropertyCategories.Properties.Key"] == filter.Key && x["PropertyCategories.Properties.Value"] < filter.Value);
            }
            var productProperties = productPropertyFilter.ToList();
            if (productProperties != null && productProperties.Count > 0)
            {
                products = BsonSerializer.Deserialize<List<Dictionary<string, object>>>(productProperties.ToJson())
                   .Select(x => x["ProductId"])
                   .ToList();
            productAndCategoryList = ls.Where(x => products.Any(p => (int)p == x.Id)).ToList();
        }
        return productAndCategoryList;
    }
}
