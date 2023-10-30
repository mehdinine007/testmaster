using OrderManagement.Application.Contracts.OrderManagement.Inqueries;
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
        SaleDetailDto GetById(int id);
        List<SaleDetailDto> GetActiveList();
        Task<PagedResultDto<SaleDetailDto>> GetSaleDetails(BaseInquery input);
        Task<int> Save(CreateSaleDetailDto createSaleDetailDto);
        Task<int> Update(CreateSaleDetailDto createSaleDetailDto);
        Task<bool> Delete(int id);
        List<SaleDetailForDropDownDto> GetAll();
    }
}
