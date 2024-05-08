﻿using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface ISeasonAllocationService:IApplicationService
    {

        Task<List<SeasonAllocationDto>> GetList();
        Task<SeasonAllocationDto> GetById(int id);
        Task<SeasonAllocationDto> Add(SeasonAllocationCreateOrUpdateDto SeasonAllocationCreateOrUpdateDto);
        Task<SeasonAllocationDto> Update(SeasonAllocationCreateOrUpdateDto SeasonAllocationCreateOrUpdateDto);
        Task<bool> Delete(int id);
      

    }
}
