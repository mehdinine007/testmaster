using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using CompanyManagement.EfCore;


namespace CompanyService.Host.EntityFrameworkCore
{
    public class CompanyServiceMigrationDbContext : AbpDbContext<CompanyServiceMigrationDbContext>
    {
        public CompanyServiceMigrationDbContext(
            DbContextOptions<CompanyServiceMigrationDbContext> options
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
