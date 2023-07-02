using Esale.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Inqueries;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
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
    private readonly IRepository<CarTip> _carTipRepository;
    private readonly IRepository<ESaleType> _eSaleTypeRepository;
    private readonly IRepository<SaleDetailCarColor> _saleDetailCarColor;
    private readonly IRepository<Color> _colorRepository;
    private readonly IRepository<SaleDetailCarColor, int> _saleDetailColorRepository;

    public SaleDetailService(IRepository<SaleDetail> saleDetailRepository,
                             IRepository<CarTip> carTipRepository,
                             IRepository<ESaleType> eSaleTypeRepository,
                             IRepository<SaleDetailCarColor> saleDetailCarColor,
                             IRepository<Color> color,
                             IRepository<SaleDetailCarColor, int> saleDetailColorRepository
        )
    {
        _saleDetailRepository = saleDetailRepository;
        _carTipRepository = carTipRepository;
        _eSaleTypeRepository = eSaleTypeRepository;
        _saleDetailCarColor = saleDetailCarColor;
        _colorRepository = color;
        _saleDetailColorRepository = saleDetailColorRepository;
    }



    public async Task<bool> Delete(int id)
    {

        await _saleDetailRepository.DeleteAsync(x => x.Id == id, autoSave: true);
        return true;

    }

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

    public SaleDetailDto GetById(int id)
    {
        var saleDetail = _saleDetailRepository
            .WithDetails()
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);
        return ObjectMapper.Map<SaleDetail, SaleDetailDto>(saleDetail);
    }

    public async Task<PagedResultDto<SaleDetailDto>> GetSaleDetails(BaseInquery input)
    {
        var count = await _saleDetailRepository.CountAsync();
        var saleDetails = _saleDetailRepository.WithDetails(x => x.CarTip,
            x => x.CarTip.CarType.CarFamily.Company,
            x => x.SaleDetailCarColors,
            x => x.SaleDetailCarColors,
            x => x.SaleSchema,
            x => x.SaleDetailCarColors);
        var queryResult = saleDetails.PageBy(input).Select(x => new SaleDetailDto()
        {
            CarDeliverDate = x.CarDeliverDate,
            CarFamilyId = x.CarTip.CarType.CarFamily.Id,
            CarFamilyTitle = x.CarTip.CarType.CarFamily.Title,
            CarFee = x.CarFee,
            CarTipId = x.CarTipId,
            CarTipTitle = x.CarTip.Title,
            CarTypeId = x.CarTip.CarTypeId,
            CarTypeTitle = x.CarTip.CarType.Title,
            CircularSaleCode = x.CircularSaleCode,
            CompanyId = x.CarTip.CarType.CarFamily.CompanyId,
            CompanyName = x.CarTip.CarType.CarFamily.Company.Name,
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
            MinimumAmountOfProxyDeposit = x.MinimumAmountOfProxyDeposit
        }).ToList();

        var saleDetailIds = queryResult.Select(x => x.Id).ToList();
        var saleDetailColors = (await _saleDetailColorRepository.GetQueryableAsync()).Where(x => saleDetailIds.Any(y => y == x.SaleDetailId));
        var colorIds = saleDetailColors.Select(x => x.ColorId);
        var colors = (await _colorRepository.GetQueryableAsync()).Where(x => colorIds.Any(y => y == x.Id));
        queryResult.ForEach(x =>
        {
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
    public async Task<int> Save(CreateSaleDetailDto createSaleDetailDto)
    {
        if (createSaleDetailDto.SalePlanEndDate < createSaleDetailDto.SalePlanStartDate)
        {
            throw new UserFriendlyException("تاریخ پایان بایدبزرگتراز تاریخ شروع باشد.");
        }
        var carTip = await _carTipRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.CarTipId);
        if (carTip == null || createSaleDetailDto.CarTipId <= 0)
        {
            throw new UserFriendlyException("ماشین انتخاب شده وجودندارد");
        }
        var esalType = await _eSaleTypeRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.EsaleTypeId);
        if (esalType == null || createSaleDetailDto.EsaleTypeId <= 0)
        {
            throw new UserFriendlyException("نوع طرح فروش انتخاب شده وجود ندارد");
        }

        var color = await _colorRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.ColorId);
        if (color == null)
        {
            throw new UserFriendlyException("رنگ انتخاب شده موجود نمیباشد");
        }


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

        var carTipId = await _carTipRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.CarTipId);
        if (carTipId == null || createSaleDetailDto.CarTipId <= 0)
        {
            throw new UserFriendlyException("ماشین انتخاب شده وجودندارد");
        }
        var esalTypeId = await _eSaleTypeRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.EsaleTypeId);
        if (esalTypeId == null || createSaleDetailDto.EsaleTypeId <= 0)
        {
            throw new UserFriendlyException(" نوع طرح فروش انتخاب شده وجود ندارد");
        }

        var saleDetail = ObjectMapper.Map<CreateSaleDetailDto, SaleDetail>(createSaleDetailDto);
        await _saleDetailRepository.AttachAsync(saleDetail, c => c.CircularSaleCode, s => s.SalePlanCode, s => s.SalePlanDescription,
        c => c.CarTipId, m => m.ManufactureDate, s => s.SalePlanStartDate, m => m.ManufactureDate, s => s.SalePlanEndDate,
        c => c.CarDeliverDate, s => s.SaleTypeCapacity, c => c.CoOperatingProfitPercentage, r => r.RefuseProfitPercentage, e => e.ESaleTypeId, c => c.CarFee, d => d.DeliverDaysCount,
        d => d.MinimumAmountOfProxyDeposit, s => s.SaleId, v => v.Visible);
        return saleDetail.Id;
    }




}

