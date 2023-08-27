﻿using Microsoft.AspNetCore.Http;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface ISaleSchemaService: IApplicationService
    {
        Task<List<SaleSchemaDto>> GetList(AttachmentEntityTypeEnum attachmentType);
        Task<SaleSchemaDto> Add(CreateSaleSchemaDto saleSchemaDto);
        Task<SaleSchemaDto> Update(CreateSaleSchemaDto saleSchemaDto);
        Task<bool> Delete(int id);
        Task<SaleSchemaDto> GetById(int id);
        Task<bool> UploadFile(UploadFileDto uploadFile);
    }
}
