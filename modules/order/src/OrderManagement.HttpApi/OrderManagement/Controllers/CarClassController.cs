using Esale.Share.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.OrderManagement;

namespace OrderManagement.HttpApi.OrderManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/CarClassService/[action]")]
    //[UserAuthorization]
    public class CarClassController: Controller, ICarClassService
    {

        private readonly ICarClassService _carClassService;
        public CarClassController(ICarClassService carClassService)
            => _carClassService = carClassService;
        [HttpDelete]
        public Task<bool> Delete(int id)
        =>_carClassService.Delete(id);
        [HttpPost]
        public Task<List<CarClassDto>> GetCarClass()
        =>_carClassService.GetCarClass();
        [HttpPost]
        public Task<CarClassDto> Save(CarClassDto carClassDto)
      =>_carClassService.Save(carClassDto);
        [HttpPut]
        public Task<CarClassDto> Update(CarClassDto carClassDto)
       =>_carClassService.Update(carClassDto);
    }
}
