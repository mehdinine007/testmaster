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
        Task<List<SaleDetailDto>> GetSaleDetails();
        Task<int> Save(CreateSaleDetailDto createSaleDetailDto);
        Task<int> Update(CreateSaleDetailDto createSaleDetailDto);
        Task<int> Delete(int id);
    }
}
