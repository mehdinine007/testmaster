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
        Task<SaleDetailDto> GetById(int id);
        Task<List<SaleDetailDto>> GetActiveList();
        Task<PagedResultDto<SaleDetailDto>> GetSaleDetails(BaseInquery input);
        Task<SaleDetailDto> Save(CreateSaleDetailDto createSaleDetailDto);
        Task<SaleDetailDto> Update(CreateSaleDetailDto createSaleDetailDto);
        Task<bool> Delete(int id);
        Task<List<SaleDetailForDropDownDto>> GetAll();
        Task<List<SaleDetailDto>> GetList(int? saleId);
    }
}
