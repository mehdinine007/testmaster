using CompanyManagement.Domain.CompanyManagement;
using CompanyManagement.EfCore.CompanyManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace CompanyManagement.EfCore.CompanyManagement.EntityFrameworkCore
{
    [ConnectionStringName("CompanyManagement")]

    public class CompanyManagementDbContext : AbpDbContext<CompanyManagementDbContext>, ICompanyManagementDbContext
    {


        public CompanyManagementDbContext(DbContextOptions<CompanyManagementDbContext> options)
          : base(options)
        {

        }

        public DbSet<ClientsOrderDetailByCompany> ClientsOrderDetailByCompany { get; set; }
        public DbSet<CompanyPaypaidPrices> CompanyPaypaidPrices { get; set; }
        public DbSet<CompanySaleCallDates> CompanySaleCallDates { get; set; }
        public DbSet<CompanyProduction> CompanyProduction { get; set; }
        public DbSet<CompaniesCustomer> CompaniesCustomer { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //System.Diagnostics.Debugger.Launch();
            base.OnModelCreating(builder);

            //builder.ConfigureAuditLogging();
            builder.ConfigureCompanyManagement(options =>
            {
                //options.TablePrefix = TablePrefix;
                //options.Schema = Schema;
            });
        }

    }


}
