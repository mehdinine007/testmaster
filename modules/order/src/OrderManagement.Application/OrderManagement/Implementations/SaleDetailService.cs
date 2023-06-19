using AutoMapper.Internal.Mappers;
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
    public async Task<int> Save(CreateSaleDetailDto createSaleDetailDto)
    {

        var random = new Random();
        long randomSerialDecimal = random.Next(999999999);
        var now = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
        string.Format("{0:X}", randomSerialDecimal).PadLeft(10, '0');

        if (createSaleDetailDto.CarTipId <= 0)
        {
            throw new UserFriendlyException("تیپ ماشین صحیح نمیباشد.");
        }

        if (createSaleDetailDto.ESaleTypeId <= 0)
        {
            throw new UserFriendlyException("کد نوع فروش صحیح نمیباشد");
        }
        var saleDetail = ObjectMapper.Map<CreateSaleDetailDto, SaleDetail>(createSaleDetailDto);
        var uid = Guid.NewGuid();
        saleDetail.UID = uid;
        await _saleDetailRepository.InsertAsync(saleDetail);
        await CurrentUnitOfWork.SaveChangesAsync();
        return saleDetail.Id;
    }

    public async Task<int> Update(CreateSaleDetailDto CreateSaleDetailDto)
    {
        var saleDetail = ObjectMapper.Map<CreateSaleDetailDto, SaleDetail>(CreateSaleDetailDto);

        await _saleDetailRepository.UpdateAsync(saleDetail);
        await CurrentUnitOfWork.SaveChangesAsync();
        return saleDetail.Id;
    }
}

