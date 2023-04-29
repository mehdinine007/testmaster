using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace OrderManagement.EntityFrameworkCore
{
    [ConnectionStringName("OrderManagement")]
    public class OrderManagementDbContext : AbpDbContext<OrderManagementDbContext>, IOrderManagementDbContext
    {
        public static string TablePrefix { get; set; } = OrderManagementConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = OrderManagementConsts.DefaultDbSchema;

        public DbSet<Order> Orders { get; set; }

        public OrderManagementDbContext(DbContextOptions<OrderManagementDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureOrderManagement(options =>
            {
                options.TablePrefix = TablePrefix;
                options.Schema = Schema;
            });
        }
    }
}