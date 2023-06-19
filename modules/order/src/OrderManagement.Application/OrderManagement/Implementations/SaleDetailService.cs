using AutoMapper.Internal.Mappers;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class SaleDetailService : ApplicationService, ISaleDetailService
{
    private readonly IRepository<SaleDetail> _saleDetailRepository;

    public SaleDetailService(IRepository<SaleDetail> saleDetailRepository)
    {
        _saleDetailRepository = saleDetailRepository;
    }



    public async Task<int> Delete(int id)
    {

        await _saleDetailRepository.DeleteAsync(x => x.Id == id);
        await CurrentUnitOfWork.SaveChangesAsync();
        return id;
    }

    public async Task<List<SaleDetailDto>> GetSaleDetails()
    {
        var saleDetails = await _saleDetailRepository.WithDetailsAsync(x => x.CarTip, x => x.CarTip.CarType);
        var queryResult = saleDetails.ToList();
        var saleDetailDtos = ObjectMapper.Map<List<SaleDetail>, List<SaleDetailDto>>(queryResult);
        return saleDetailDtos;

    }
    [UnitOfWork(isTransactional: false)]
    public async Task<int> Save(SaleDetailDto saleDetailDto)
    {
        SaleDetail saleDetail = new SaleDetail()
        {

            CarTipId = saleDetailDto.CarTipId,
            ESaleTypeId = saleDetailDto.EsaleTypeId,
            CarFee = saleDetailDto.CarFee,
            CircularSaleCode = saleDetailDto.CircularSaleCode,
            CarDeliverDate = saleDetailDto.CarDeliverDate,
            SalePlanCode = saleDetailDto.SalePlanCode,
            SaleTypeCapacity = saleDetailDto.SaleTypeCapacity,
            SalePlanDescription = saleDetailDto.SalePlanDescription,
            DeliverDaysCount= saleDetailDto.DeliverDaysCount,
            CoOperatingProfitPercentage= saleDetailDto.CoOperatingProfitPercentage,
            MinimumAmountOfProxyDeposit = saleDetailDto.MinimumAmountOfProxyDeposit,
             
        };

        //var saleDetail = ObjectMapper.Map<SaleDetailDto, SaleDetail> (saleDetailDto);
        await _saleDetailRepository.InsertAsync(saleDetail);
        await CurrentUnitOfWork.SaveChangesAsync();
        return saleDetail.Id;
    }

    public async Task<int> Update(SaleDetailDto saleDetailDto)
    {
        //var saleDetail = ObjectMapper.Map<SaleDetailDto, SaleDetail>(saleDetailDto);
        SaleDetail saleDetail = new SaleDetail()
        {
           
            CarTipId = saleDetailDto.CarTipId,
            ESaleTypeId = saleDetailDto.EsaleTypeId,
            CarFee = saleDetailDto.CarFee,
            CircularSaleCode = saleDetailDto.CircularSaleCode,
            CarDeliverDate = saleDetailDto.CarDeliverDate,
            SalePlanCode = saleDetailDto.SalePlanCode,
            SaleTypeCapacity = saleDetailDto.SaleTypeCapacity,
            SalePlanDescription = saleDetailDto.SalePlanDescription,
            DeliverDaysCount = saleDetailDto.DeliverDaysCount,
            CoOperatingProfitPercentage = saleDetailDto.CoOperatingProfitPercentage,
            MinimumAmountOfProxyDeposit = saleDetailDto.MinimumAmountOfProxyDeposit,

        };
     
        await _saleDetailRepository.UpdateAsync(saleDetail);
        await CurrentUnitOfWork.SaveChangesAsync();
        return saleDetail.Id;
    }
}

