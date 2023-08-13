using Microsoft.AspNetCore.Http;
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
        Task<List<SaleSchemaDto>> GetAllSaleSchema();
        Task<PagedResultDto<SaleSchemaDto>> GetList(SaleSchemaGetListDto input);
        Task<int> Add(CreateSaleSchemaDto saleSchemaDto);
        Task<int> Update(CreateSaleSchemaDto saleSchemaDto);
        Task<bool> Delete(int id);
        Task<bool> UploadFile(UploadFileDto uploadFile);
    }
}
