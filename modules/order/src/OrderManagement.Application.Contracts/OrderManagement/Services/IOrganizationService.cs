﻿using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface IOrganizationService : IApplicationService
    {
        Task<List<OrganizationDto>> GetList(OrganizationQueryDto organizationQueryDto);
        Task<OrganizationDto> Add(OrganizationAddOrUpdateDto orgDto);
        Task<OrganizationDto> Update(OrganizationAddOrUpdateDto orgDto);
        Task<bool> Delete(int id);
        Task<OrganizationDto> GetById(int id, List<AttachmentEntityTypeEnum>? attachmentType = null, List<AttachmentLocationEnum> attachmentlocation = null);
        Task<Guid> UploadFile(UploadFileDto uploadFile);
        Task<bool> Move(OrganizationPriorityDto input);
    }
}
