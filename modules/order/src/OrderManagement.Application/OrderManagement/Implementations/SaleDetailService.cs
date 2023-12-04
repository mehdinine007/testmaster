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
        var saleDetail = await _saleDetailRepository.FirstOrDefaultAsync(x => x.Id == id);
        if (saleDetail != null)
        {
            await _saleDetailRepository.DeleteAsync(x => x.Id == id, autoSave: true);
            await _cacheManager.RemoveAsync(saleDetail.UID.ToString(), RedisConstants.SaleDetailPrefix, new CacheOptions() { Provider = CacheProviderEnum.Hybrid });
        }

        return true;

    }

    [SecuredOperation(SaleDetailServicePermissionConstants.GetActiveList)]
    public List<SaleDetailDto> GetActiveList()
    {
        var currentTime = DateTime.Now;
        var saledetails = _saleDetailRepository
            .WithDetails()
            .AsNoTracking()
            .Where(x => x.SalePlanStartDate <= currentTime && currentTime <= x.SalePlanEndDate && x.Visible)
            .ToList();
        return ObjectMapper.Map<List<SaleDetail>, List<SaleDetailDto>>(saledetails);
    }

    [SecuredOperation(SaleDetailServicePermissionConstants.GetById)]
    public SaleDetailDto GetById(int id)
    {
        var saleDetail = _saleDetailRepository
            .WithDetails(x => x.Product)
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);
        var result = ObjectMapper.Map<SaleDetail, SaleDetailDto>(saleDetail);
        return result;
    }

    [SecuredOperation(SaleDetailServicePermissionConstants.GetSaleDetails)]
    public async Task<PagedResultDto<SaleDetailDto>> GetSaleDetails(BaseInquery input)
    {
        var count = await _saleDetailRepository.CountAsync();
        var saleDetails = _saleDetailRepository.WithDetails(
            x => x.SaleDetailCarColors,
            x => x.SaleDetailCarColors,
            x => x.SaleSchema,
            x => x.SaleDetailCarColors,
            x => x.Product);
        var queryResult = saleDetails.PageBy(input).Select(x => new SaleDetailDto()
        {
            CarDeliverDate = x.CarDeliverDate,
            CarFee = x.CarFee,
            CircularSaleCode = x.CircularSaleCode,
            CoOperatingProfitPercentage = x.CoOperatingProfitPercentage,
            Id = x.Id,
            DeliverDaysCount = x.DeliverDaysCount,
            EsaleName = x.ESaleType.SaleTypeName,
            EsaleTypeId = x.ESaleTypeId,
            ManufactureDate = x.ManufactureDate,
            Visible = x.Visible,
            SaleId = x.SaleId,
            SaleTitle = x.SaleSchema.Title,
            SalePlanCode = x.SalePlanCode,
            UID = x.UID,
            SaleTypeCapacity = x.SaleTypeCapacity,
            SalePlanEndDate = x.SalePlanEndDate,
            SalePlanStartDate = x.SalePlanStartDate,
            SalePlanDescription = x.SalePlanDescription,
            RefuseProfitPercentage = x.RefuseProfitPercentage,
            MinimumAmountOfProxyDeposit = x.MinimumAmountOfProxyDeposit,
            ProductId = x.ProductId,
            Product = ObjectMapper.Map<ProductAndCategory, ProductAndCategoryViewModel>(x.Product)
        }).ToList();
        var attachments = await _attachmentService.GetList(AttachmentEntityEnum.ProductAndCategory, queryResult.Select(x => x.ProductId).ToList());
        var saleDetailIds = queryResult.Select(x => x.Id).ToList();
        var saleDetailColors = (await _saleDetailColorRepository.GetQueryableAsync()).Where(x => saleDetailIds.Any(y => y == x.SaleDetailId));
        var colorIds = saleDetailColors.Select(x => x.ColorId);
        var colors = (await _colorRepository.GetQueryableAsync()).Where(x => colorIds.Any(y => y == x.Id));
        queryResult.ForEach(x =>
        {

            var attachment = attachments.Where(y => y.EntityId == x.ProductId).ToList();
            x.Product.Attachments = ObjectMapper.Map<List<AttachmentDto>, List<AttachmentViewModel>>(attachment);
            var saleDetailColor = saleDetailColors.FirstOrDefault(y => y.SaleDetailId == x.Id);
            if (saleDetailColor != null)
            {
                var color = colors.FirstOrDefault(y => y.Id == saleDetailColor.ColorId);
                if (color != null)
                {
                    x.ColorTitle = color.ColorName;
                    x.ColorId = color.Id;
                }
            }
        });
        return new PagedResultDto<SaleDetailDto>
        {
            TotalCount = count,
            Items = queryResult
        };
    }

    [UnitOfWork(isTransactional: false)]
    [SecuredOperation(SaleDetailServicePermissionConstants.Save)]
    public async Task<int> Save(CreateSaleDetailDto createSaleDetailDto)
    {

        if (createSaleDetailDto.SalePlanEndDate < createSaleDetailDto.SalePlanStartDate)
        {
            throw new UserFriendlyException("تاریخ پایان بایدبزرگتراز تاریخ شروع باشد.");
        }

        var product = await _productAndCategoryService.GetById(createSaleDetailDto.ProductId, false);
        if (product == null)
        {
            throw new UserFriendlyException(OrderConstant.ProductAndCategoryNotFound, OrderConstant.ProductAndCategoryFoundId);
        }
        var esalType = await _eSaleTypeRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.EsaleTypeId);
        if (esalType == null)
        {
            throw new UserFriendlyException("نوع طرح فروش انتخاب شده وجود ندارد");
        }

        var color = await _colorRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.ColorId);
        if (color == null)
        {
            throw new UserFriendlyException("رنگ انتخاب شده موجود نمیباشد");
        }
        // control sale schema exists
        await _saleSchemaService.GetById(createSaleDetailDto.SaleId);

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


        var resultQuery = await _saleDetailRepository.InsertAsync(saleDetail, autoSave: true);


        SaleDetailCarColor saleDetailCarColor = new SaleDetailCarColor
        {
            ColorId = createSaleDetailDto.ColorId,
            SaleDetailId = resultQuery.Id
        };
        await _saleDetailCarColor.InsertAsync(saleDetailCarColor, autoSave: true);

        return saleDetail.Id;
    }

    [SecuredOperation(SaleDetailServicePermissionConstants.Update)]
    public async Task<int> Update(CreateSaleDetailDto createSaleDetailDto)
    {
        var result = await _saleDetailRepository.WithDetails().AsNoTracking().FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.Id);
        if (result == null)
        {
            throw new UserFriendlyException("رکوردی برای ویرایش وجود ندارد");
        }
        if (createSaleDetailDto.SalePlanEndDate < createSaleDetailDto.SalePlanStartDate)
        {
            throw new UserFriendlyException("تاریخ پایان بایدبزرگتراز تاریخ شروع باشد.");
        }

        var esalTypeId = await _eSaleTypeRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.EsaleTypeId);
        if (esalTypeId == null)
        {
            throw new UserFriendlyException(" نوع طرح فروش انتخاب شده وجود ندارد");
        }

        var saleDetail = ObjectMapper.Map<CreateSaleDetailDto, SaleDetail>(createSaleDetailDto);
        await _saleDetailRepository.AttachAsync(saleDetail, c => c.CircularSaleCode, s => s.SalePlanCode, s => s.SalePlanDescription,
        c => c.CarTipId, m => m.ManufactureDate, s => s.SalePlanStartDate, m => m.ManufactureDate, s => s.SalePlanEndDate,
        c => c.CarDeliverDate, s => s.SaleTypeCapacity, c => c.CoOperatingProfitPercentage, r => r.RefuseProfitPercentage, e => e.ESaleTypeId, c => c.CarFee, d => d.DeliverDaysCount,
        d => d.MinimumAmountOfProxyDeposit, s => s.SaleId, v => v.Visible);
        await _cacheManager.RemoveAsync(saleDetail.UID.ToString(), RedisConstants.SaleDetailPrefix, new CacheOptions() { Provider = CacheProviderEnum.Hybrid });
        return saleDetail.Id;
    }


    public List<SaleDetailForDropDownDto> GetAll()
    {
        var saledetails = _saleDetailRepository
            .WithDetails()
            .AsNoTracking()
            .ToList();
        return ObjectMapper.Map<List<SaleDetail>, List<SaleDetailForDropDownDto>>(saledetails);
    }




}
