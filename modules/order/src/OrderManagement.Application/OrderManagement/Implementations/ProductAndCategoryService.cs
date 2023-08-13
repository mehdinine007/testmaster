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


namespace OrderManagement.Application.OrderManagement.Implementations;

public class ProductAndCategoryService : ApplicationService, IProductAndCategoryService
{
    private readonly IRepository<ProductAndCategory, int> _productAndCategoryRepository;
    private readonly IAttachmentService _attachmentService;
    private readonly IProductPropertyService _productPropertyService;
    private readonly IRepository<ProductLevel, int> _productLevelRepository;
    private readonly ICommonAppService _commonAppService;

    public ProductAndCategoryService(IRepository<ProductAndCategory, int> productAndCategoryRepository,
                                     IAttachmentService attachmentService,
                                     IProductPropertyService productPropertyService,
                                     IRepository<ProductLevel, int> productLevelRepository,
                                     ICommonAppService commonAppService
        )
    {
        _productAndCategoryRepository = productAndCategoryRepository;
        _attachmentService = attachmentService;
        _productPropertyService = productPropertyService;
        _productLevelRepository = productLevelRepository;
        _commonAppService = commonAppService;
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
        var productLevelQuery = (await _productLevelRepository.GetQueryableAsync()).OrderBy(x=>x.Priority);

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
            productAndCategoryCreateDto.LevelId = parent.LevelId + 1;
            var parentPriority = productLevelQuery.FirstOrDefault(x => x.Id == parent.ProductLevelId).Priority;
            var currentProductlevel = productLevelQuery.Where(x => x.Priority > parentPriority).FirstOrDefault();
            if (currentProductlevel == null)
            {
                throw new UserFriendlyException(OrderConstant.LastProductLevel, OrderConstant.LastProductLevelId);
            }
            productAndCategoryCreateDto.ProductLevelId = currentProductlevel.Id;
        }
        else
        {
            productAndCategoryCreateDto.LevelId = 1;
            var productLevelId = productLevelQuery.FirstOrDefault().Id;
            productAndCategoryCreateDto.ProductLevelId = productLevelId;
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
        productAndCategoryCreateDto.Code = _maxCode;
        productAndCategoryCreateDto.ParentId = productAndCategoryCreateDto.ParentId.HasValue && productAndCategoryCreateDto.ParentId.Value > 0
            ? productAndCategoryCreateDto.ParentId.Value
            : null;
        productAndCategoryCreateDto.Active = true;




        var entity = await _productAndCategoryRepository.InsertAsync(
            ObjectMapper.Map<ProductAndCategoryCreateDto, ProductAndCategory>(productAndCategoryCreateDto));
        return ObjectMapper.Map<ProductAndCategory, ProductAndCategoryDto>(entity);
    }

    public async Task<bool> UploadFile(UploadFileDto uploadFileDto)
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
            productAndCategoryQuery = productAndCategoryQuery.Where(x => x.Active);
        var attachments = new List<AttachmentDto>();
        switch (input.Type)
        {
            case ProductAndCategoryType.Category:
                var parent = productAndCategoryQuery
                    .Include(x => x.Childrens.Where(y => y.Type == ProductAndCategoryType.Category))
                    .Where(x => EF.Functions.Like(x.Code, input.NodePath + "%") && x.Type == ProductAndCategoryType.Category)
                    .ToList();
                attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, parent.Select(x => x.Id).ToList());
                ls = string.IsNullOrWhiteSpace(input.NodePath)
                    ? parent.Where(x => x.ParentId == null).ToList()
                    : parent.Where(x => x.Code == input.NodePath).ToList();
                break;
            case ProductAndCategoryType.Product:
                if (string.IsNullOrWhiteSpace(input.NodePath))
                    throw new UserFriendlyException("مسیر نود خالی است");
                ls = productAndCategoryQuery.Where(x => EF.Functions.Like(x.Code, input.NodePath + "%") && x.Type == ProductAndCategoryType.Product).ToList();
                attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, ls.Select(x => x.Id).ToList());
                break;
        }
        var productAndCategories = ObjectMapper.Map<List<ProductAndCategory>, List<ProductAndCategoryWithChildDto>>(ls);
        productAndCategories = await FillAttachmentAndProperty(productAndCategories, attachments);
        return productAndCategories;
    }

    private async Task<List<ProductAndCategoryWithChildDto>> FillAttachmentAndProperty(List<ProductAndCategoryWithChildDto> productAndCategories, List<AttachmentDto> attachments)
    {
        productAndCategories.ForEach(async x =>
        {
            var pacAttachments = attachments.Where(y => y.EntityId == x.Id).ToList();
            x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(pacAttachments);
            if (x.Type == ProductAndCategoryType.Product)
                x.PropertyCategories = await _productPropertyService.GetByProductId(x.Id);
            if (x.Childrens != null && x.Childrens.Count > 0)
                x.Childrens = await FillAttachmentAndProperty(x.Childrens.ToList(), attachments);
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

    public async Task<List<ProductAndSaleDetailListDto>> GetProductAndSaleDetailList(string nodePath)
    {
        List<ProductAndCategory> ProductList = new();
        var currentTime = DateTime.Now;
        var productQuery = await _productAndCategoryRepository.GetQueryableAsync();
        productQuery = productQuery.Where(x => x.Active && x.Type == ProductAndCategoryType.Product).Include(x => x.SaleDetails.Where(x => x.SalePlanStartDate <= currentTime && currentTime <= x.SalePlanEndDate && x.Visible))
            .ThenInclude(y => y.ESaleType);
        var product = productQuery
                  .Where(x => EF.Functions.Like(x.Code, nodePath + "%"))
                  .ToList();
        ProductList = string.IsNullOrWhiteSpace(nodePath)
            ? product.ToList()
            : product.Where(x => x.Code == nodePath).ToList();
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, ProductList.Select(x => x.Id).ToList());
        var productAndSaleDetailListDto = ObjectMapper.Map<List<ProductAndCategory>, List<ProductAndSaleDetailListDto>>(ProductList);
        productAndSaleDetailListDto.ForEach(x =>
        {
            var attachment = attachments.Where(y => y.EntityId == x.Id).ToList();
            x.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
        });


        return productAndSaleDetailListDto;

    }
}
