﻿using Esale.Share.Authorize;
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
using IFG.Core.Utility.Tools;

namespace OrderManagement.HttpApi.OrderManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/CarClassService/[action]")]
    //[UserAuthorization]
    public class CarClassController: Controller
    {

        private readonly ICarClassService _carClassService;
        public CarClassController(ICarClassService carClassService)
            => _carClassService = carClassService;
        [HttpDelete]
        public Task<bool> Delete(int id)
        =>_carClassService.Delete(id);
        [HttpGet]
        public Task<List<CarClassDto>> GetList(string attachmentType, string attachmentlocation)
        =>_carClassService.GetList(EnumHelper.ConvertStringToEnum<AttachmentEntityTypeEnum>(attachmentType), EnumHelper.ConvertStringToEnum<AttachmentLocationEnum>(attachmentlocation));
        [HttpPost]
        public Task<CarClassDto> Add(CarClassCreateDto carClassDto)
      =>_carClassService.Add(carClassDto);
        [HttpPut]
        public Task<CarClassDto> Update(CarClassCreateDto carClassDto)
       =>_carClassService.Update(carClassDto);
        [HttpPost]
        public Task<Guid> UploadFile([FromForm]UploadFileDto uploadFile)
       => _carClassService.UploadFile(uploadFile);
        [HttpGet]
        public Task<CarClassDto> GetById(CarClassQueryDto carClassQueryDto)
        =>_carClassService.GetById(carClassQueryDto);
    }
}
