using EasyCaching.Core;
using IFG.Core.Caching;
using IFG.Core.DataAccess;
using Esale.Share.Authorize;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Inqueries;
using OrderManagement.Application.Contracts.OrderManagement.Models;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using Permission.Order;
using Volo.Abp.ObjectMapping;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class SaleDetailService : ApplicationService, ISaleDetailService
{
    private readonly IRepository<SaleDetail> _saleDetailRepository;
    private readonly IRepository<ESaleType> _eSaleTypeRepository;
    private readonly IRepository<SaleDetailCarColor> _saleDetailCarColor;
    private readonly IRepository<Color> _colorRepository;
    private readonly IRepository<SaleDetailCarColor, int> _saleDetailColorRepository;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ICommonAppService _commonAppService;
    private readonly ICacheManager _cacheManager;
    private readonly IAttachmentService _attachmentService;
    private readonly IRepository<ProductLevel, int> _productLevelRepository;
    private readonly IProductAndCategoryService _productAndCategoryService;
    private readonly ISaleSchemaService _saleSchemaService;
    public SaleDetailService(IRepository<SaleDetail> saleDetailRepository,
                             IRepository<ESaleType> eSaleTypeRepository,
                             IRepository<SaleDetailCarColor> saleDetailCarColor,
                             IRepository<Color> color,
                             IRepository<SaleDetailCarColor, int> saleDetailColorRepository,
                             IHttpContextAccessor contextAccessor,
                             ICommonAppService commonAppService, IHybridCachingProvider hybridCache, ICacheManager cacheManager,
                             IAttachmentService attachmentService, IRepository<ProductLevel, int> productLevelRepository,
                             IProductAndCategoryService productAndCategoryService,
                             ISaleSchemaService saleSchemaService
        )
    {
        _saleDetailRepository = saleDetailRepository;
        _eSaleTypeRepository = eSaleTypeRepository;
        _saleDetailCarColor = saleDetailCarColor;
        _colorRepository = color;
        _saleDetailColorRepository = saleDetailColorRepository;
        _contextAccessor = contextAccessor;
        _commonAppService = commonAppService;
        _cacheManager = cacheManager;
        _attachmentService = attachmentService;
        _productLevelRepository = productLevelRepository;
        _productAndCategoryService = productAndCategoryService;
        _saleSchemaService = saleSchemaService;
    }


    [SecuredOperation(SaleDetailServicePermissionConstants.Delete)]
    public async Task<bool> Delete(int id)
    {
        var saleDetail = await Validation(id, null);
        await _saleDetailRepository.DeleteAsync(x => x.Id == id);
        await _cacheManager.RemoveAsync(saleDetail.UID.ToString(), RedisConstants.SaleDetailPrefix, new CacheOptions() { Provider = CacheProviderEnum.Hybrid });
        return true;

    }

    [SecuredOperation(SaleDetailServicePermissionConstants.GetActiveList)]
    public async Task<List<SaleDetailDto>> GetActiveList()
    {
        var currentTime = DateTime.Now;
        var saledetails = (await _saleDetailRepository.GetQueryableAsync())
            .AsNoTracking()
            .Where(x => x.SalePlanStartDate <= currentTime && currentTime <= x.SalePlanEndDate && x.Visible)
            .ToList();
        return ObjectMapper.Map<List<SaleDetail>, List<SaleDetailDto>>(saledetails);
    }

    //[SecuredOperation(SaleDetailServicePermissionConstants.GetById)]
    public async Task<SaleDetailDto> GetById(int id)
    {
        var saleDetailQuery = (await _saleDetailRepository.GetQueryableAsync()).AsNoTracking()
            .Include(x => x.Product).ThenInclude(x => x.Organization)
            .Include(x => x.ESaleType)
            .Include(x => x.SaleSchema);
        var saleDetail = saleDetailQuery.FirstOrDefault(x => x.Id == id);
        if (saleDetail is null)
            throw new UserFriendlyException(OrderConstant.SaleDetailNotFound, OrderConstant.SaleDetailNotFoundId);
        var result = ObjectMapper.Map<SaleDetail, SaleDetailDto>(saleDetail);
        return result;
    }

    [SecuredOperation(SaleDetailServicePermissionConstants.GetSaleDetails)]
    public async Task<PagedResultDto<SaleDetailDto>> GetSaleDetails(BaseInquery input)
    {
        var saleDetailsQuery = (await _saleDetailRepository.GetQueryableAsync())
           .AsNoTracking();

        saleDetailsQuery= saleDetailsQuery.Include(x => x.SaleDetailCarColors)
           .Include(x => x.SaleDetailCarColors)
           .ThenInclude(x=>x.Color)
           .Include(x => x.SaleSchema)
           .Include(x => x.SaleDetailCarColors)
           .Include(x => x.Product)
           .ThenInclude(x=>x.Organization);
      

        saleDetailsQuery = input.SaleId is not null
           ? saleDetailsQuery.Where(x => x.SaleId == input.SaleId)
           : saleDetailsQuery;

        var count =await saleDetailsQuery.CountAsync();
       var  saleDetails = saleDetailsQuery.OrderByDescending(x=>x.Id).PageBy(input).ToList();
        var saleDetailDto = ObjectMapper.Map<List<SaleDetail>, List<SaleDetailDto>>(saleDetails);
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, saleDetailDto.Select(x => x.ProductId).ToList());
        saleDetailDto.ForEach(x =>
        {
            var attachment = attachments.Where(y => y.EntityId == x.ProductId).ToList();
            x.Product.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
        });
        return new PagedResultDto<SaleDetailDto>
        {
            TotalCount = count,
            Items = saleDetailDto
        };
    }

    [UnitOfWork(isTransactional: false)]
    [SecuredOperation(SaleDetailServicePermissionConstants.Save)]
    public async Task<SaleDetailDto> Save(CreateSaleDetailDto createSaleDetailDto)
    {
        await Validation(null, createSaleDetailDto);
        var uid = Guid.NewGuid();

        var result = await _saleDetailRepository.SingleOrDefaultAsync(x => x.UID == uid);
        var saleDetail = ObjectMapper.Map<CreateSaleDetailDto, SaleDetail>(createSaleDetailDto);
        saleDetail.UID = uid;
        while (result != null)
        {
            uid = Guid.NewGuid();
            saleDetail.UID = uid;
            result = await _saleDetailRepository.SingleOrDefaultAsync(x => x.UID == uid);

        }
        var resultQuery = await _saleDetailRepository.InsertAsync(saleDetail);
        await CurrentUnitOfWork.SaveChangesAsync();
        SaleDetailCarColor saleDetailCarColor = new SaleDetailCarColor
        {
            ColorId = createSaleDetailDto.ColorId,
            SaleDetailId = resultQuery.Id
        };
        await _saleDetailCarColor.InsertAsync(saleDetailCarColor);
        return await GetById(saleDetail.Id); ;
    }

    [SecuredOperation(SaleDetailServicePermissionConstants.Update)]
    public async Task<SaleDetailDto> Update(CreateSaleDetailDto createSaleDetailDto)
    {

        var saleDetail = await Validation(createSaleDetailDto.Id, createSaleDetailDto);
        await _saleDetailRepository.UpdateAsync(saleDetail);
        await _cacheManager.RemoveAsync(saleDetail.UID.ToString(), RedisConstants.SaleDetailPrefix, new CacheOptions() { Provider = CacheProviderEnum.Hybrid });
        return await GetById(saleDetail.Id);
    }


    public async Task<List<SaleDetailForDropDownDto>> GetAll()
    {
        var saledetails = (await _saleDetailRepository.GetQueryableAsync())
            .AsNoTracking()
            .ToList();
        return ObjectMapper.Map<List<SaleDetail>, List<SaleDetailForDropDownDto>>(saledetails);
    }

    public async Task<List<SaleDetailDto>> GetList(int? saleId)
    {
        var saleDetailQuery = (await _saleDetailRepository.GetQueryableAsync()).AsNoTracking()
            .Include(x => x.Product).ThenInclude(x => x.Organization)
            .Include(x => x.ESaleType)
           .Include(x => x.SaleSchema);
        var saledetails = saleId is not null ? saleDetailQuery.Where(x => x.SaleId == saleId).ToList() : saleDetailQuery.ToList();
        var saledetailDto = ObjectMapper.Map<List<SaleDetail>, List<SaleDetailDto>>(saledetails);
        return saledetailDto;
    }

    private async Task<SaleDetail> Validation(int? id, CreateSaleDetailDto createSaleDetailDto)
    {
        var saleDetailQuery = (await _saleDetailRepository.GetQueryableAsync()).AsNoTracking();
        SaleDetail saleDetail = new SaleDetail();
        if (id is not null)
        {
            saleDetail = saleDetailQuery.FirstOrDefault(x => x.Id == id);
            if (saleDetail is null)
                throw new UserFriendlyException(OrderConstant.SaleDetailNotFound, OrderConstant.SaleDetailNotFoundId);
        }
        if (createSaleDetailDto is not null)
        {
            if (createSaleDetailDto.SalePlanEndDate < createSaleDetailDto.SalePlanStartDate)
            {
                throw new UserFriendlyException(OrderConstant.SalePlanEndDate, OrderConstant.SalePlanEndDateId);
            }

            var product = await _productAndCategoryService.GetById(createSaleDetailDto.ProductId, false);
            if (product == null)
            {
                throw new UserFriendlyException(OrderConstant.ProductAndCategoryNotFound, OrderConstant.ProductAndCategoryFoundId);
            }
            var esalType = await _eSaleTypeRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.EsaleTypeId);
            if (esalType == null)
            {
                throw new UserFriendlyException(OrderConstant.EsaleTypeIdNotFound, OrderConstant.EsaleTypeIdNotFoundId);
            }

            var color = await _colorRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.ColorId);
            if (color == null)
            {
                throw new UserFriendlyException(OrderConstant.ColorIdNotFound, OrderConstant.ColorIdNotFoundId);
            }
            // control sale schema exists
            await _saleSchemaService.GetById(createSaleDetailDto.SaleId);

        }


        return saleDetail;
    }


}
