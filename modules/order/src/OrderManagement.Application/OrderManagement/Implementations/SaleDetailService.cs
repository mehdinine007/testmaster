using AutoMapper.Internal.Mappers;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class SaleDetailService : ApplicationService, ISaleDetailService
{
    private readonly IRepository<SaleDetail> _saleDetailRepository;
    private readonly IRepository<CarTip> _carTipRepository;
    private readonly IRepository<ESaleType> _eSaleTypeRepository;

    public SaleDetailService(IRepository<SaleDetail> saleDetailRepository, IRepository<CarTip> carTipRepository, IRepository<ESaleType> eSaleTypeRepository)
    {
        _saleDetailRepository = saleDetailRepository;
        _carTipRepository = carTipRepository;
        _eSaleTypeRepository = eSaleTypeRepository;
    }



    public async Task Delete(int id)
    {
       
     await _saleDetailRepository.DeleteAsync(x => x.Id == id, autoSave: true);
        throw new UserFriendlyException("حذف با موفقیت انجام شد.");

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
        if (createSaleDetailDto.SalePlanStartDate < createSaleDetailDto.SalePlanEndDate)
        {
            throw new UserFriendlyException("تاریخ شروع نباید کوچکترازتاریخ پایان باشد.");
        }
        if (createSaleDetailDto.SalePlanEndDate > createSaleDetailDto.SalePlanStartDate)
        {
            throw new UserFriendlyException("تاریخ پایان نباید بزرگترازتاریخ شروع باشد.");
        }
        var carTipId = await _carTipRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.CarTipId);
        if (carTipId == null || createSaleDetailDto.CarTipId <= 0)
        {
            throw new UserFriendlyException("تیپ ماشین وجود ندارد");
        }
        var esalTypeId = await _eSaleTypeRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.ESaleTypeId);
        if (esalTypeId == null || createSaleDetailDto.ESaleTypeId <= 0)
        {
            throw new UserFriendlyException("کد نوع فروش وجود ندارد");
        }

        var saleDetail = ObjectMapper.Map<CreateSaleDetailDto, SaleDetail>(createSaleDetailDto);
        var uid = Guid.NewGuid();
        saleDetail.UID = uid;
        await _saleDetailRepository.InsertAsync(saleDetail, autoSave: true);
        return saleDetail.Id;
    }

    public async Task<int> Update(CreateSaleDetailDto createSaleDetailDto)
    {
        if (createSaleDetailDto.Id <= 0)
        {
            throw new UserFriendlyException("رکوردی برای ویرایش وجود ندارد");
        }

        var carTipId = await _carTipRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.CarTipId);
        if (carTipId == null || createSaleDetailDto.CarTipId <= 0)
        {
            throw new UserFriendlyException("تیپ ماشین وجود ندارد");
        }
        var esalTypeId = await _eSaleTypeRepository.FirstOrDefaultAsync(x => x.Id == createSaleDetailDto.ESaleTypeId);
        if (esalTypeId == null || createSaleDetailDto.ESaleTypeId <= 0)
        {
            throw new UserFriendlyException("کد نوع فروش وجود ندارد");
        }
        var saleDetail = ObjectMapper.Map<CreateSaleDetailDto, SaleDetail>(createSaleDetailDto);

        await _saleDetailRepository.UpdateAsync(saleDetail, autoSave: true);
        return saleDetail.Id;
    }
}

