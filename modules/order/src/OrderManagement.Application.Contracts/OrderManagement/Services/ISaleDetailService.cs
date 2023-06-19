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
        Task<int> Save(SaleDetailDto saleDetail);
        Task<int> Update(SaleDetailDto saleDetail);
        Task<int> Delete(int id);
    }
}
