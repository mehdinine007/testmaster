using Microsoft.EntityFrameworkCore;
using OrderManagement.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace OrderService.Host.EntityFrameworkCore
{
    public class OrderServiceMigrationDbContext : AbpDbContext<OrderServiceMigrationDbContext>
    {
        public OrderServiceMigrationDbContext(
            DbContextOptions<OrderServiceMigrationDbContext> options
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
