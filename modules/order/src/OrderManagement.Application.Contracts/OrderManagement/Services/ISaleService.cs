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

        Task<List<SaleDetailDto>> GetSaleDetails(int tipId, int typeId, int familyId, int companyId, int esaleTypeId);
        Task<List<SalePlanDto>> GetSalePlans(int companyId);

        Task<List<ESaleTypeDto>> GetSaleTypes();

        Task UserValidationByBirthDate(int saleId);
        Task UserValidationByMobile(int saleId);

    }
}
