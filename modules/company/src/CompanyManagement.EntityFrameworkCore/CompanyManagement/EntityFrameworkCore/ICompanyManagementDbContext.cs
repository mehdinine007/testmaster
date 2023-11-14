using CompanyManagement.Domain.CompanyManagement;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace CompanyManagement.EfCore.CompanyManagement.EntityFrameworkCore
{
    [ConnectionStringName("CompanyManagement")]
    public interface ICompanyManagementDbContext : IEfCoreDbContext
    {
        DbSet<ClientsOrderDetailByCompany> ClientsOrderDetailByCompany { get; set; }
        DbSet<CompanyPaypaidPrices> CompanyPaypaidPrices { get; set; }
        DbSet<CompanySaleCallDates> CompanySaleCallDates { get; set; }
        DbSet<CompanyProduction> CompanyProduction { get; set; }
        DbSet<CompaniesCustomer> CompaniesCustomer { get; set; }

        DbSet<CompaniesCustomer> CompaniesCustomer { get; set; }
    }
}
