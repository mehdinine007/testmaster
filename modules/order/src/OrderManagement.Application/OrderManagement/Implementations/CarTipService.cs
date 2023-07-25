using AutoMapper.Internal.Mappers;
using Esale.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace OrderManagement.Application.OrderManagement.Implementations;

public class CarTipService : ApplicationService, ICarTipService
{
    private readonly IRepository<CarTip> _carTipRepository;
    private readonly IRepository<CarType> _carTypeRepository;
    public CarTipService(IRepository<CarTip> carTipRepository, IRepository<CarType> carTypeRepository)
    {
        _carTipRepository = carTipRepository;
        _carTypeRepository = carTypeRepository;

    }

    public async Task<bool> Delete(int id)
    {

        await _carTipRepository.DeleteAsync(x => x.Id == id);
        return true;
    }

    public async Task<List<CarTipDto>> GetAllCarTips()
    {
        var carTips = await _carTipRepository.GetListAsync();
        var carTipDto = ObjectMapper.Map<List<CarTip>, List<CarTipDto>>(carTips);
        return carTipDto;
    }

    public async Task<PagedResultDto<CarTipDto>> GetCarTips(int pageNo, int sizeNo)
    {
        var count = await _carTipRepository.CountAsync();
        var carTips = await _carTipRepository.WithDetailsAsync();
        var queryResult = await carTips.Skip(pageNo * sizeNo).Take(sizeNo).ToListAsync();
        return new PagedResultDto<CarTipDto>
        {
            TotalCount = count,
            Items = ObjectMapper.Map<List<CarTip>, List<CarTipDto>>(queryResult)
        };
    }

    public async Task<int> Save(CarTipDto carTipDto)
    {
        var carType =await _carTypeRepository.FirstOrDefaultAsync(x => x.Id == carTipDto.CarTypeId);
        if (carType == null)
        {
            throw new UserFriendlyException(" نوع ماشین انتخاب شده وجود ندارد");
        }
        var carTip = ObjectMapper.Map<CarTipDto, CarTip>(carTipDto);
        await _carTipRepository.InsertAsync(carTip, autoSave: true);
        return carTip.Id;
    }

    public async Task<int> Update(CarTipDto carTipDto)
    {
        var carType =await _carTypeRepository.FirstOrDefaultAsync(x => x.Id == carTipDto.CarTypeId);
        if (carType == null)
        {
            throw new UserFriendlyException(" نوع ماشین انتخاب شده وجود ندارد");
        }
        var carTip = ObjectMapper.Map<CarTipDto, CarTip>(carTipDto);
        await _carTipRepository.AttachAsync(carTip, t => t.Title, c => c.CarTypeId);
        return carTip.Id;
    }
}
