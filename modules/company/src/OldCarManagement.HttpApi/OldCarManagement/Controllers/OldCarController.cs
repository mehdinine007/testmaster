﻿using CompanyManagement.Application.Contracts.CompanyManagement.Dto.OldCarDtos;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;

namespace CompanyManagement.HttpApi.OldCarManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/OldCar/[action]")]
    public class OldCarController: Controller
    {
        private readonly IOldCarService _oldCarService;
        public OldCarController(IOldCarService oldCarService)
     => _oldCarService = oldCarService;
        [HttpPost]
        public Task<bool> AddList(OldCarCreateDtoList oldCarCreateDto)
       =>_oldCarService.AddList(oldCarCreateDto);
        [HttpDelete]
        public Task<bool> Delete(string nationalcode)
        =>_oldCarService.Delete(nationalcode);
        [HttpGet]
        public Task<List<OldCarDto>> Inquiry(string nationalcode)
       =>_oldCarService.Inquiry(nationalcode);
    }
}
