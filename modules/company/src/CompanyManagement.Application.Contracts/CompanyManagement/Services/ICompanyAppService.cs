using CompanyManagement.Domain.Shared.CompanyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Services
{
    public interface ICompanyAppService: IApplicationService
    {
        Task SubmitOrderInformations(List<ClientsOrderDetailByCompanyDto> clientsOrderDetailByCompnayDtos);
        List<CustomersWithCars> GetCustomersAndCars(GetCustomersAndCarsDto input);
        Task InsertCompanyProduction(List<CompanyProductionDto> companyProductionsDto);

    }
}
