﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.OrderManagement.Implementations;

namespace OrderManagement.HttpApi.OrderManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/AgencyService/[action]")]
    public class AgencyController : Controller, IAgencyService
    {

        private readonly IAgencyService _agencyServicecs;
        public AgencyController(IAgencyService agencyServicecs)
            => _agencyServicecs = agencyServicecs;



        [HttpDelete]
        public async Task<int> Delete(int id)
         => await _agencyServicecs.Delete(id);
        [HttpGet]
        public async Task<List<AgencyDto>> GetAgencies()
         => await _agencyServicecs.GetAgencies();
        [HttpPost]
        public async Task<int> Save(AgencyDto agencyDto)
         => await _agencyServicecs.Save(agencyDto);

        [HttpPut]
        public async Task<int> Update(AgencyDto agencyDto)
         => await _agencyServicecs.Update(agencyDto);
    }
}
