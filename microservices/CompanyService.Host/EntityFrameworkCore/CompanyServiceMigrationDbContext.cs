using Microsoft.EntityFrameworkCore;
using OrderManagement.EfCore;
using Volo.Abp.EntityFrameworkCore;

namespace OrderService.Host.EntityFrameworkCore
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

            modelBuilder.ConfigureOrderManagement();
        }
    }
}
