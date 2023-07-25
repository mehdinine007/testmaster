using OrderManagement.Application.Contracts.OrderManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using OrderManagement.Application.Contracts;
using Esale.Share.Authorize;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/CarTipService/[action]")]
[UserAuthorization]
public class CarTipController : Controller, ICarTipService
{
    private readonly ICarTipService _carTipService;
    public CarTipController(ICarTipService carTipService)
        => _carTipService = carTipService;
    [HttpDelete]
    public Task<bool> Delete(int id)
    =>_carTipService.Delete(id);    
    [HttpGet]
    public Task<List<CarTipDto>> GetAllCarTips()
   => _carTipService.GetAllCarTips();

    [HttpGet]
    public Task<PagedResultDto<CarTipDto>> GetCarTips(int pageNo, int sizeNo)
    => _carTipService.GetCarTips(pageNo, sizeNo);
    [HttpPost]
    public Task<int> Save(CarTipDto carTipDto)
    => _carTipService.Save(carTipDto);
    [HttpPut]
    public Task<int> Update(CarTipDto carTipDto)
    =>_carTipService.Update(carTipDto);
}
