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
using OrderManagement.Domain.Shared;

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
        [HttpGet]
        public Task<List<CarClassDto>> GetList(AttachmentEntityTypeEnum? attachmentType)
        =>_carClassService.GetList(attachmentType);
        [HttpPost]
        public Task<CarClassDto> Add(CarClassDto carClassDto)
      =>_carClassService.Add(carClassDto);
        [HttpPut]
        public Task<CarClassDto> Update(CarClassDto carClassDto)
       =>_carClassService.Update(carClassDto);
        [HttpPost]
        public Task<bool> UploadFile([FromForm]UploadFileDto uploadFile)
       => _carClassService.UploadFile(uploadFile);
        [HttpPost]
        public Task<CarClassDto> GetById(CarClassQueryDto carClassQueryDto)
        =>_carClassService.GetById(carClassQueryDto);
    }
}
