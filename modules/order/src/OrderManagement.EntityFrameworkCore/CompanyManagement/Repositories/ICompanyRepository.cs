using OrderManagement.Domain.CompanyManagement;
using OrderManagement.Domain.Shared.CompanyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.EfCore.CompanyManagement.Repositories
{
    public interface ICompanyRepository: IRepository<CompanyPaypaidPrices, long>
    {
        public List<CustomersWithCars> GetCustomerOrderList(CustomersAndCarsInputDto customersAndCarsInputDto);

    }
}
