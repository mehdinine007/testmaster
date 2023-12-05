using CompanyManagement.Domain.CompanyManagement;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace CompanyManagement.EfCore.CompanyManagement.EntityFrameworkCore
{
    public interface IOrderManagementDbContex : IEfCoreDbContext
    {
        DbSet<CompaniesCustomer> CompaniesCustomer { get; set; }
    }
}
