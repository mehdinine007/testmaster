﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices
{
    public interface IProcessService: IApplicationService
    {
        Task<ProcessDto> GetById(Guid id);
        Task<ProcessDto> Add(ProcessCreateOrUpdateDto processCreateOrUpdateDto);
        Task<ProcessDto> Update(ProcessCreateOrUpdateDto processCreateOrUpdateDto);
        Task<List<ProcessDto>> GetList();
        Task<bool> Delete(Guid id);
    }
}
