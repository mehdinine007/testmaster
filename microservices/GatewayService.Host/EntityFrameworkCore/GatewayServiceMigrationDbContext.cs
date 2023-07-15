using Microsoft.EntityFrameworkCore;
using GatewayManagement.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace GatewayService.Host.EntityFrameworkCore
{
    public class GatewayServiceMigrationDbContext : AbpDbContext<GatewayServiceMigrationDbContext>
    {
        public GatewayServiceMigrationDbContext(
            DbContextOptions<GatewayServiceMigrationDbContext> options
            ) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureGatewayManagement();
        }
    }
}
