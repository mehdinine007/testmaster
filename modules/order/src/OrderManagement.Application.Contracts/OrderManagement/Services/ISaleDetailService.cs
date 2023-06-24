using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.OrderManagement.Services
{
    public interface ISaleDetailService:IApplicationService
    {
        Task<PagedResultDto<SaleDetailDto>> GetSaleDetails(int pageNo,int sizeNo);
        Task<int> Save(CreateSaleDetailDto createSaleDetailDto);
        Task<int> Update(CreateSaleDetailDto createSaleDetailDto);
        Task Delete(int id);
    }
}
