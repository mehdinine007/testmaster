﻿using Microsoft.AspNetCore.Http;
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
        Task<List<SaleSchemaDto>> GetAllSaleSchema();
        PagedResultDto<SaleSchemaDto> GetSaleSchema(int pageNo, int sizeNo);
        Task<int> Save(SaleSchemaDto saleSchemaDto);
        Task<int> Update(SaleSchemaDto saleSchemaDto);
        Task<bool> Delete(int id);
        Task<bool> UploadFile(UploadFileDto uploadFile); 
    }
}
