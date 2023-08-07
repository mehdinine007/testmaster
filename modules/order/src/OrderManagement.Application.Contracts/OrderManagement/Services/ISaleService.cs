using OrderManagement.Application.Contracts.OrderManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.Services
{
    public interface ISaleService : IApplicationService
    {
        Task<List<PreSaleDto>> GetPreSales();

        Task<SaleDetailDto> GetSaleDetail(Guid uid);

        Task<List<SaleDetailDto>> GetSaleDetails(SaleDetailGetListDto input);
        Task<List<SalePlanDto>> GetSalePlans(int companyId);

        Task<List<ESaleTypeDto>> GetSaleTypes();

        Task UserValidationByBirthDate(int saleId);
        Task UserValidationByMobile(int saleId);
    }
}
