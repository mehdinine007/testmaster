using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using CompanyManagement.EfCore;


namespace BankService.Host.EntityFrameworkCore
{
    public class BankServiceMigrationDbContext : AbpDbContext<BankServiceMigrationDbContext>
    {
        public BankServiceMigrationDbContext(
            DbContextOptions<BankServiceMigrationDbContext> options
            ) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureCompanyManagement();
        }
    }
}
