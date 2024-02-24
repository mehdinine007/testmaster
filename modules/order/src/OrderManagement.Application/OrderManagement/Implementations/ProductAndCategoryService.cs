﻿using OrderManagement.Application.Contracts;
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
using IFG.Core.Utility.Tools;
using Volo.Abp.ObjectMapping;
using Esale.Share.Authorize;
using OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions;
using System.Linq.Dynamic.Core;
using IFG.Core.IOC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OrderManagement.Domain.Shared.OrderManagement.Enums;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OrderManagement.Domain.OrderManagement.MongoWrite;
using System.IO;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class ProductAndCategoryService : ApplicationService, IProductAndCategoryService
{
    private readonly IRepository<ProductAndCategory, int> _productAndCategoryRepository;
    private readonly IAttachmentService _attachmentService;
    private readonly IProductPropertyService _productPropertyService;
    private readonly IRepository<ProductLevel, int> _productLevelRepository;
    private readonly ICommonAppService _commonAppService;
    private readonly IRepository<ProductProperty, ObjectId> _productPropertyRepository;
    private readonly IUserDataAccessService _userDataAccessService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IRepository<Organization, int> _organizationRepository;
    private readonly IRepository<PropertyCategory, ObjectId> _propertyDefinitionRepository;
    private readonly IRepository<ProductPropertyWrite, ObjectId> _productPropertyWriteRepository;
    public ProductAndCategoryService(IRepository<ProductAndCategory, int> productAndCategoryRepository,
                                     IAttachmentService attachmentService,
                                     IProductPropertyService productPropertyService,
                                     IRepository<ProductLevel, int> productLevelRepository,
                                     ICommonAppService commonAppService,
                                     IRepository<ProductProperty, ObjectId> productPropertyRepository,
                                     IUserDataAccessService userDataAccessService,
                                     IConfiguration configuration,
                                     IRepository<Organization, int> organizationRepository,
                                     IRepository<PropertyCategory, ObjectId> propertyDefinitionRepository,
                                     IRepository<ProductPropertyWrite, ObjectId> productPropertyWriteRepository)
    {
        _productAndCategoryRepository = productAndCategoryRepository;
        _attachmentService = attachmentService;
        _productPropertyService = productPropertyService;
        _productLevelRepository = productLevelRepository;
        _commonAppService = commonAppService;
        _productPropertyRepository = productPropertyRepository;
        _userDataAccessService = userDataAccessService;
        _httpContextAccessor = ServiceTool.Resolve<IHttpContextAccessor>();
        _configuration = configuration;
        _organizationRepository = organizationRepository;
        _propertyDefinitionRepository = propertyDefinitionRepository;
        _productPropertyWriteRepository = productPropertyWriteRepository;
    }
    [SecuredOperation(ProductAndCategoryServicePermissionConstants.Delete)]
    public async Task Delete(int id)
    {
        var productCategory = await GetById(id, false, null);
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

    public async Task<ProductAndCategoryWithChildDto> GetById(int id, bool hasProperty, List<AttachmentEntityTypeEnum> attachmentType = null, List<AttachmentLocationEnum> locationType = null)
    {
        await Validation(id, null);
        var query = await _productAndCategoryRepository.GetQueryableAsync();
        //if (!_commonAppService.IsInRole("Admin"))
        query = query.Where(x => x.Active);
        var productCategory = query
            .FirstOrDefault(x => x.Id == id) ??
            throw new UserFriendlyException("محصول یا دسته بندی مورد نطر پیدا نشد");
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, new List<int>() { id }, attachmentType, locationType);
        var productResult = ObjectMapper.Map<ProductAndCategory, ProductAndCategoryWithChildDto>(productCategory);
        var productAndCategoryList = await FillAttachmentAndProperty(new List<ProductAndCategoryWithChildDto>() { productResult }, attachments, hasProperty);
        return productResult;
    }

    [SecuredOperation(ProductAndCategoryServicePermissionConstants.Add)]
    public async Task<ProductAndCategoryDto> Insert(ProductAndCategoryCreateDto productAndCategoryCreateDto)
    {
        var levelId = 0;
        var productLevelId = 0;
        var code = "";
        var productLevelQuery = (await _productLevelRepository.GetQueryableAsync()).OrderBy(x => x.Priority);
        var oganizationQuery = (await _organizationRepository.GetQueryableAsync()).AsNoTracking();
        var oganization = oganizationQuery.FirstOrDefault(x => x.Id == productAndCategoryCreateDto.OrganizationId);
        if (oganization is null)
        {
            throw new UserFriendlyException(OrderConstant.OrganizationNotFound, OrderConstant.OrganizationNotFoundId);
        }
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
        else
            _parentCode = oganization.Code.ToString();

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
        //productAndCategoryCreateDto.Active = true;
        var productAndCategory = ObjectMapper.Map<ProductAndCategoryCreateDto, ProductAndCategory>(productAndCategoryCreateDto);
        productAndCategory.LevelId = levelId;
        productAndCategory.ProductLevelId = productLevelId;
        productAndCategory.Code = code;
        var entity = await _productAndCategoryRepository.InsertAsync(productAndCategory, autoSave: true);
        return ObjectMapper.Map<ProductAndCategory, ProductAndCategoryDto>(entity);
    }

    [SecuredOperation(ProductAndCategoryServicePermissionConstants.UploadFile)]
    public async Task<Guid> UploadFile(UploadFileDto uploadFileDto)
    {
        await Validation(uploadFileDto.Id, null);
        var attachmentStatus = await _attachmentService.UploadFile(AttachmentEntityEnum.ProductAndCategory, uploadFileDto);
        return attachmentStatus;
    }

    public async Task<CustomPagedResultDto<ProductAndCategoryDto>> GetListWithPagination(ProductAndCategoryQueryDto input)
    {
        var productCategoryQuery = await _productAndCategoryRepository.WithDetailsAsync(x => x.Childrens);
        //if (!_commonAppService.IsInRole("Admin"))
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
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, ids, EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(input.AttachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(input.AttachmentLocation));

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
        productAndCategoryQuery = productAndCategoryQuery.Include(x => x.ProductLevel).OrderBy(x => x.Priority);
        if (input.IsActive)
            productAndCategoryQuery = productAndCategoryQuery.Where(x => x.Active);
        if (input.OrganizationId != null && input.OrganizationId > 0)
            productAndCategoryQuery = productAndCategoryQuery.Where(x => x.OrganizationId == input.OrganizationId);

        var attachments = new List<AttachmentDto>();
        switch (input.Type)
        {
            case ProductAndCategoryType.Category:
                var parent = productAndCategoryQuery
                    .Include(x => x.Childrens.Where(y => y.Type == ProductAndCategoryType.Category))
                    .Where(x => EF.Functions.Like(x.Code, input.NodePath + "%") && x.Type == ProductAndCategoryType.Category)
                    .ToList();
                attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, parent.Select(x => x.Id).ToList(), EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(input.attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(input.attachmentlocation));
                ls = string.IsNullOrWhiteSpace(input.NodePath)
                    ? parent.Where(x => x.ParentId == null).ToList()
                    : parent.Where(x => x.Code != input.NodePath).ToList();
                if (input.ProductLevelId != null && input.ProductLevelId > 0)
                    ls = ls.Where(x => x.ProductLevelId == input.ProductLevelId)
                        .ToList();
                break;
            case ProductAndCategoryType.Product:
                ls = productAndCategoryQuery.Where(x => EF.Functions.Like(x.Code, input.NodePath + "%") && x.Type == ProductAndCategoryType.Product).ToList();
                if (input.AdvancedSearch != null && input.AdvancedSearch.Count > 0)
                {
                    ls = await GetProductFilter(input.AdvancedSearch, ls);
                }
                attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, ls.Select(x => x.Id).ToList(), EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(input.attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(input.attachmentlocation));
                break;
        }
        var productAndCategories = ObjectMapper.Map<List<ProductAndCategory>, List<ProductAndCategoryWithChildDto>>(ls);
        productAndCategories = await FillAttachmentAndProperty(productAndCategories, attachments, input.HasProperty);
        return productAndCategories;
    }

    private async Task<List<ProductAndCategoryWithChildDto>> FillAttachmentAndProperty(List<ProductAndCategoryWithChildDto> productAndCategories, List<AttachmentDto> attachments, bool hasProperty)
    {
        productAndCategories.ForEach(async x =>
        {
            var pacAttachments = attachments.Where(y => y.EntityId == x.Id).ToList();
            x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(pacAttachments);
            if (x.Type == ProductAndCategoryType.Product && hasProperty)
                x.PropertyCategories = await _productPropertyService.GetByProductId(x.Id);
            if (x.Childrens != null && x.Childrens.Count > 0)
                x.Childrens = await FillAttachmentAndProperty(x.Childrens.ToList(), attachments, hasProperty);
        });
        return productAndCategories;
    }
    [SecuredOperation(ProductAndCategoryServicePermissionConstants.Update)]
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
            .Include(x => x.SaleDetails.Where(x => x.SalePlanStartDate <= currentTime && currentTime <= x.SalePlanEndDate && x.Visible && (input.ESaleTypeId == null || x.ESaleTypeId == input.ESaleTypeId)))
             .ThenInclude(y => y.ESaleType)
            .Include(x => x.ProductLevel)
            .OrderBy(x => x.Priority);
        ProductList = string.IsNullOrWhiteSpace(input.NodePath)
            ? productQuery.ToList()
            : productQuery.Where(x => EF.Functions.Like(x.Code, input.NodePath + "%")).ToList();
        if (input.OrganizationId != null && input.OrganizationId > 0)
            ProductList = ProductList.Where(x => x.OrganizationId == input.OrganizationId).ToList();
        if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            var nationalCode = _commonAppService.GetNationalCode();
            if (_configuration.GetValue<bool?>("UserDataAccessConfig:HasProduct") ?? false)
            {
                var products = await _userDataAccessService.ProductGetList(nationalCode);
                ProductList = ProductList.Where(x => products.Any(p => x.Id == p.ProductId))
                    .ToList();
            }
        }
        if (input.AdvancedSearch != null && input.AdvancedSearch.Count > 0)
        {
            ProductList = await GetProductFilter(input.AdvancedSearch, ProductList);
        }
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, ProductList.Select(x => x.Id).ToList(), EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(input.attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(input.attachmentLocation));

        var productAndSaleDetailListDto = ObjectMapper.Map<List<ProductAndCategory>, List<ProductAndCategoryWithChildDto>>(ProductList);

        productAndSaleDetailListDto = await FillAttachmentAndProperty(productAndSaleDetailListDto, attachments, input.HasProperty);

        return productAndSaleDetailListDto;

    }

    private async Task<List<ProductAndCategory>> GetProductFilter(List<AdvancedSearchDto> input, List<ProductAndCategory> ls)
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

    public async Task<List<FilterParamDto>> GetFilterParamList()
    {
        throw new NotImplementedException();
    }

    private async Task<ProductAndCategory> Validation(int id, ProductAndCategoryDto productAndCategoryDto)
    {
        var productAndCategory = (await _productAndCategoryRepository.GetQueryableAsync())
            .FirstOrDefault(x => x.Id == id);
        if (productAndCategory is null)
        {
            throw new UserFriendlyException(OrderConstant.ProductAndCategoryNotFound, OrderConstant.ProductAndCategoryFoundId);
        }
        return productAndCategory;
    }

    public async Task<List<ProductAndCategoryDto>> GetAllParent()
    {
        var productAndCategorytQueryable = await _productAndCategoryRepository.GetQueryableAsync();
        var productAndCategory = new ProductAndCategory();
        var parents = productAndCategorytQueryable.Where(x => x.ParentId == null).ToList();

        var productAndCategoryDto = ObjectMapper.Map<List<ProductAndCategory>, List<ProductAndCategoryDto>>(parents);
        return productAndCategoryDto;

    }

    public async Task<ProductAndCategoryDto> GetProductAndCategoryByCode(string code,
        List<AttachmentEntityTypeEnum> attachmentType = null,
        List<AttachmentLocationEnum> attachmentlocation = null
        )
    {
        var productAndCategory = (await _productAndCategoryRepository.GetQueryableAsync()).FirstOrDefault(x => x.Code == code)
            ?? throw new UserFriendlyException("دسته بندی یا محصول مرتبط بافت نشد");
        var result = ObjectMapper.Map<ProductAndCategory, ProductAndCategoryDto>(productAndCategory);
        if ((attachmentType ?? new List<AttachmentEntityTypeEnum>()).Any())
        {
            var attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, new List<int>() { productAndCategory.Id }, attachmentType, attachmentlocation);
            result.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachments);
        }
        return result;
    }
    [SecuredOperation(ProductAndCategoryServicePermissionConstants.Move)]
    public async Task<bool> Move(MoveDto move)
    {
        var productAndCategoryQuery = (await _productAndCategoryRepository.GetQueryableAsync()).AsNoTracking().OrderBy(x => x.Priority);
        var currentproductAndCategory = productAndCategoryQuery.FirstOrDefault(x => x.Id == move.Id);
        if (currentproductAndCategory == null)
        {
            throw new UserFriendlyException(OrderConstant.ProductAndCategoryNotFound, OrderConstant.ProductAndCategoryFoundId);
        }
        var currentPriority = currentproductAndCategory.Priority;
        var parentId = currentproductAndCategory.ParentId;
        if (MoveTypeEnum.Up == move.MoveType)
        {
            var previousProductAndCategory = await productAndCategoryQuery.FirstOrDefaultAsync(x => x.Priority == currentproductAndCategory.Priority - 1 && x.ParentId == parentId);
            if (previousProductAndCategory == null)
            {
                throw new UserFriendlyException(OrderConstant.FirstPriority, OrderConstant.FirstPriorityId);
            }
            var previousPriority = previousProductAndCategory.Priority;
            currentproductAndCategory.Priority = previousPriority;
            await _productAndCategoryRepository.UpdateAsync(currentproductAndCategory);
            previousProductAndCategory.Priority = currentPriority;
            await _productAndCategoryRepository.UpdateAsync(previousProductAndCategory);
        }
        else if (MoveTypeEnum.Down == move.MoveType)
        {
            var nextProductAndCategory = productAndCategoryQuery.FirstOrDefault(x => x.Priority > currentproductAndCategory.Priority && x.ParentId == parentId);
            if (nextProductAndCategory == null)
            {
                throw new UserFriendlyException(OrderConstant.LastPriority, OrderConstant.LastPriorityId);
            }
            var nextPriority = nextProductAndCategory.Priority;
            currentproductAndCategory.Priority = nextPriority;
            await _productAndCategoryRepository.UpdateAsync(currentproductAndCategory);
            nextProductAndCategory.Priority = currentPriority;
            await _productAndCategoryRepository.UpdateAsync(nextProductAndCategory);
        }


        return true;
    }
    [SecuredOperation(ProductAndCategoryServicePermissionConstants.Import)]
    public async Task Import(IFormFile file, SaleTypeEnum type)
    {
        if (type == SaleTypeEnum.esalecar)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet.Dimension is null )
                    {
                        throw new UserFriendlyException("دیتا صحیح نمیباشد");
                    }

                    var rowcount = worksheet.Dimension.Rows;
                    var colcount = worksheet.Dimension.Columns;
                    var code = worksheet.Name;
                    var productQuery = await _productAndCategoryRepository.GetQueryableAsync();
                    var product = productQuery.FirstOrDefault(x => x.Code == code);
                    if (product is null)
                    {
                        throw new UserFriendlyException("محصول وجود ندارد");
                    };
                    var productMongo = (await _productPropertyRepository.GetMongoQueryableAsync()).Where(x => x.ProductId == product.Id).ToList();
                    if (productMongo.Count > 0)
                    {
                        await _productPropertyRepository.HardDeleteAsync(y => y.ProductId == product.Id);
                    }
                    var propertyCategories = (await _propertyDefinitionRepository.GetMongoQueryableAsync()).ToList();
                    List<PropertyDto> propertyList = new List<PropertyDto>();
                    for (int row = 2; row <= rowcount; row++)
                    {

                        var key = worksheet.Cells[row, 1].Value.ToString();
                        var title = worksheet.Cells[row, 2].Value.ToString();
                        var value = worksheet.Cells[row, 3].Value.ToString();
                        foreach (var category in propertyCategories)
                        {
                            foreach (var property in category.Properties)
                            {
                                if (property.Key == key)
                                    property.Value = value;
                            }
                        }
                    }
                    var productPropertyDto = new ProductPropertyDto()
                    {
                        ProductId = product.Id,
                        PropertyCategories = ObjectMapper.Map<List<PropertyCategory>, List<ProductPropertyCategoryDto>>(propertyCategories)
                    };
                    await _productPropertyWriteRepository.InsertAsync(ObjectMapper.Map<ProductPropertyDto, ProductPropertyWrite>(productPropertyDto));

                }
            }
        }
        else if (type == SaleTypeEnum.saleauto)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    var rowcount = worksheet.Dimension.Rows;
                    var colcount = worksheet.Dimension.Columns;
                    var code = worksheet.Name;
                    var productQuery = await _productAndCategoryRepository.GetQueryableAsync();
                    var product = productQuery.FirstOrDefault(x => x.Code == code);
                    if (product is null)
                    {
                        throw new UserFriendlyException("محصول وجود ندارد");
                    };
                    var productMongo = (await _productPropertyRepository.GetMongoQueryableAsync()).Where(x => x.ProductId == product.Id).ToList();
                    if (productMongo.Count > 0)
                    {
                        await _productPropertyRepository.HardDeleteAsync(y => y.ProductId == product.Id);

                    }
                    var propertyCategories = (await _propertyDefinitionRepository.GetMongoQueryableAsync()).ToList();
                    List<PropertyDto> propertyList = new List<PropertyDto>();
                    for (int row = 2; row <= rowcount; row++)
                    {

                        var key = worksheet.Cells[row, 1].Value.ToString();
                        var title = worksheet.Cells[row, 2].Value.ToString();
                        var value = worksheet.Cells[row, 3].Value.ToString();
                        foreach (var category in propertyCategories)
                        {
                            foreach (var property in category.Properties)
                            {
                                if (property.Key == key)
                                    property.Value = value;
                            }
                        }
                    }
                    var productPropertyDto = new ProductPropertyDto()
                    {
                        ProductId = product.Id,
                        PropertyCategories = ObjectMapper.Map<List<PropertyCategory>, List<ProductPropertyCategoryDto>>(propertyCategories)
                    };
                    await _productPropertyWriteRepository.InsertAsync(ObjectMapper.Map<ProductPropertyDto, ProductPropertyWrite>(productPropertyDto));

                }
            }
        }
    }



}
