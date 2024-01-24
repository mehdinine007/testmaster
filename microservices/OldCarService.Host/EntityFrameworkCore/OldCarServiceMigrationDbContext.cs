using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using CompanyManagement.EfCore;


namespace OldCarService.Host.EntityFrameworkCore
{
    public class OldCarServiceMigrationDbContext : AbpDbContext<OldCarServiceMigrationDbContext>
    {
        public OldCarServiceMigrationDbContext(
            DbContextOptions<OldCarServiceMigrationDbContext> options
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
