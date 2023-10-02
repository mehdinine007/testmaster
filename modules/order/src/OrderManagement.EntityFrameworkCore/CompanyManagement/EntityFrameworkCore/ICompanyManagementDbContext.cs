using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain;
using OrderManagement.Domain.CompanyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    }
}
