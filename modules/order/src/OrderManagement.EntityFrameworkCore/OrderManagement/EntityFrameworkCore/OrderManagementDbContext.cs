using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace OrderManagement.EfCore
{
    [ConnectionStringName("OrderManagement")]
    public class OrderManagementDbContext : AbpDbContext<OrderManagementDbContext>, IOrderManagementDbContext
    {
        public static string TablePrefix { get; set; } = OrderManagementConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = OrderManagementConsts.DefaultDbSchema;

        public DbSet<CustomerOrder> CustomerOrders { get; set; }

        public DbSet<SaleDetail> SaleDetails { get; set; }

        public DbSet<Bank> Banks { get; set; }

        public DbSet<AdvocacyUsersFromBank> AdvocacyUsersFromBanks { get; set; }

        public DbSet<AdvocacyUsers> AdvocacyUsers { get; set; }

        public DbSet<UserRejectionFromBank> UserRejectionFromBank { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<CarFamily> CarFamilies { get; set; }

        public DbSet<CarType> CarTypes { get; set; }

        public DbSet<CarTip> CarTip { get; set; }

        public DbSet<CarTip_Gallery_Mapping> CarTip_Gallery_Mappings { get; set; }

        public DbSet<Season_Company_CarTip> Season_Company_CarTip { get; set; }

        public DbSet<ESaleType> ESaleTypes { get; set; }

        public DbSet<Year> Year { get; set; }

        public DbSet<Season> Seasons { get; set; }

        public DbSet<WhiteList> WhiteLists { get; set; }

        public DbSet<ExternalApiLogResult> ExternalApiLogResults { get; set; }

        public DbSet<ExternalApiResponsLog> ExternalApiResponsLogs { get; set; }

        public DbSet<Logs> Logs { get; set; }

        public OrderManagementDbContext(DbContextOptions<OrderManagementDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureOrderManagement(options =>
            {
                //options.TablePrefix = TablePrefix;
                //options.Schema = Schema;
            });
        }
    }
}