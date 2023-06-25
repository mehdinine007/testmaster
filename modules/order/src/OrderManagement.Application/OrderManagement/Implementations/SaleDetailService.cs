using AutoMapper.Internal.Mappers;
using Esale.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class SaleDetailService : ApplicationService, ISaleDetailService
{
    private readonly IRepository<SaleDetail> _saleDetailRepository;
    private readonly IRepository<CarTip> _carTipRepository;
    private readonly IRepository<ESaleType> _eSaleTypeRepository;
    private readonly IRepository<SaleDetailCarColor> _saleDetailCarColor;

    public SaleDetailService(IRepository<SaleDetail> saleDetailRepository, IRepository<CarTip> carTipRepository, IRepository<ESaleType> eSaleTypeRepository, IRepository<SaleDetailCarColor> saleDetailCarColor)
    {
        _saleDetailRepository = saleDetailRepository;
        _carTipRepository = carTipRepository;
        _eSaleTypeRepository = eSaleTypeRepository;
        _saleDetailCarColor = saleDetailCarColor;
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

    public async Task<PagedResultDto<SaleDetailDto>> GetSaleDetails(int pageNo, int sizeNo)
    {
        var count = await _saleDetailRepository.CountAsync();
        var saleDetails = await _saleDetailRepository.WithDetailsAsync(x => x.CarTip, x => x.CarTip.CarType);
        var queryResult = await saleDetails.Skip(pageNo * sizeNo).Take(sizeNo).ToListAsync();
        return new PagedResultDto<SaleDetailDto>
        {
            TotalCount = count,
            Items = ObjectMapper.Map<List<SaleDetail>, List<SaleDetailDto>>(queryResult)
        };



    }
    [UnitOfWork(isTransactional: false)]
    public async Task<int> Save(CreateSaleDetailDto createSaleDetailDto)
    {
        if (createSaleDetailDto.SalePlanEndDate < createSaleDetailDto.SalePlanStartDate)
        {
            throw new UserFriendlyException("تاریخ پایان بایدبزرگتراز تاریخ شروع باشد.");
        }
        var carTipId = await _carTipRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.CarTipId);
        if (carTipId == null || createSaleDetailDto.CarTipId <= 0)
        {
            throw new UserFriendlyException("ماشین انتخاب شده وجودندارد");
        }
        var esalTypeId = await _eSaleTypeRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.ESaleTypeId);
        if (esalTypeId == null || createSaleDetailDto.ESaleTypeId <= 0)
        {
            throw new UserFriendlyException("نوع طرح فروش انتخاب شده وجود ندارد");
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
        var result = await _saleDetailRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.Id);

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
        var esalTypeId = await _eSaleTypeRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.ESaleTypeId);
        if (esalTypeId == null || createSaleDetailDto.ESaleTypeId <= 0)
        {
            throw new UserFriendlyException(" نوع طرح فروش انتخاب شده وجود ندارد");
        }

        result.CircularSaleCode = createSaleDetailDto.CircularSaleCode;
        result.SalePlanCode = createSaleDetailDto.SalePlanCode;
        result.SalePlanDescription = createSaleDetailDto.SalePlanDescription;
        result.CarTipId = createSaleDetailDto.CarTipId;
        result.ManufactureDate = createSaleDetailDto.ManufactureDate;
        result.SalePlanStartDate = createSaleDetailDto.ManufactureDate;
        result.SalePlanEndDate = createSaleDetailDto.SalePlanEndDate;
        result.CarDeliverDate = createSaleDetailDto.CarDeliverDate;
        result.SaleTypeCapacity = createSaleDetailDto.SaleTypeCapacity;
        result.CoOperatingProfitPercentage = createSaleDetailDto.CoOperatingProfitPercentage;
        result.RefuseProfitPercentage = createSaleDetailDto.RefuseProfitPercentage;
        result.ESaleTypeId = createSaleDetailDto.ESaleTypeId;
        result.CarFee = createSaleDetailDto.CarFee;
        result.DeliverDaysCount = createSaleDetailDto.DeliverDaysCount;
        result.MinimumAmountOfProxyDeposit = createSaleDetailDto.MinimumAmountOfProxyDeposit;
        result.SaleId = createSaleDetailDto.SaleId;
        result.Visible = createSaleDetailDto.Visible;
        await _saleDetailRepository.UpdateAsync(result, autoSave: true);
        return result.Id;
    }




}

