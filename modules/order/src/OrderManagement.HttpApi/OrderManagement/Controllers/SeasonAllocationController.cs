using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OrderManagement.HttpApi.OrderManagement.Controllers
{

    [RemoteService]
    [Route("api/services/app/SeasonAllocationService/[action]")]
    public class SeasonAllocationController: Controller
    {
        private readonly ISeasonAllocationService _seasonAllocationService;
        public SeasonAllocationController(ISeasonAllocationService seasonAllocationService)
            => _seasonAllocationService = seasonAllocationService;
        [HttpPost]
        public Task<SeasonAllocationDto> Add(SeasonAllocationCreateDto seasonAllocationCreateDto)
        =>_seasonAllocationService.Add(seasonAllocationCreateDto);
        [HttpDelete]
        public Task<bool> Delete(int id)
        =>_seasonAllocationService.Delete(id); 
        [HttpGet]
        public Task<SeasonAllocationDto> GetById(int id)
        =>_seasonAllocationService.GetById(id);
        [HttpGet]
        public Task<List<SeasonAllocationDto>> GetList()
        => _seasonAllocationService.GetList();
        [HttpPut]
        public Task<SeasonAllocationDto> Update(SeasonAllocationUpdateDto seasonAllocationUpdateDto)
        =>_seasonAllocationService.Update(seasonAllocationUpdateDto);   
    }
}
