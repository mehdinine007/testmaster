using OrderManagement.Domain.Shared.CompanyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.Contracts.CompanyManagement.Services
{
    public interface ICompanyAppService: IApplicationService
    {
        Task<bool> SubmitOrderInformations(List<ClientsOrderDetailByCompanyDto> clientsOrderDetailByCompnayDtos);
        List<CustomersWithCars> GetCustomersAndCars(GetCustomersAndCarsDto input);
        Task<bool> InsertCompanyProduction(List<CompanyProductionDto> companyProductionsDto);

    }
}
