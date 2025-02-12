﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices
{
    public interface IOrganizationPositionService:IApplicationService
    {
        Task<OrganizationPositionDto> GetById(int id);
        Task<OrganizationPositionDto> Add(OrganizationPositionCreateOrUpdateDto organizationPositionCreateOrUpdateDto);
        Task<OrganizationPositionDto> Update(OrganizationPositionCreateOrUpdateDto organizationPositionCreateOrUpdateDto);
        Task<List<OrganizationPositionDto>> GetList(int organizationChartId);
        Task<bool> Delete(int id);
        Task<OrganizationPositionDto> GetByPersonId(Guid personId);
        Task<OrganizationPositionDto> GetByOrganizationChartId(int organizationChartId);
        Task<AuthenticateResponseDto> Authenticate(AuthenticateReqDto input);
    }
}
