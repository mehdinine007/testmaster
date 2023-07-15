using Microsoft.EntityFrameworkCore;
using GatewayManagement.Domain;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace GatewayManagement.EntityFrameworkCore
{
    [ConnectionStringName("GatewayManagement")]
    public class GatewayManagementDbContext : AbpDbContext<GatewayManagementDbContext>, IGatewayManagementDbContext
    {
        public static string TablePrefix { get; set; } = GatewayManagementConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = GatewayManagementConsts.DefaultDbSchema;

        public GatewayManagementDbContext(DbContextOptions<GatewayManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureGatewayManagement(options =>
            {
                //options.TablePrefix = TablePrefix;
                //options.Schema = Schema;
            });
        }
    }
}