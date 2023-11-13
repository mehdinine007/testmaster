using Microsoft.EntityFrameworkCore;
using WorkFlowManagement.Domain;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Db;
using AdminPanelManagement.Domain.AdminPanelManagement;

namespace AdminPanelManagement.EntityFrameworkCore
{
    [ConnectionStringName("AdminPanelManagement")]
    public class AdminPanelManagementDbContext : AbpDbContext<AdminPanelManagementDbContext>, IAdminPanelManagementDbContext
    {
      //  public static string TablePrefix { get; set; } = adminpanelmana.DefaultDbTablePrefix;

       // public static string Schema { get; set; } = AdminPanelManagementConsts.DefaultDbSchema;
     //   public DbSet<OrganizationChart> OrganizationCharts { get; set ; }
        


        public AdminPanelManagementDbContext(DbContextOptions<AdminPanelManagementDbContext> options)
            : base(options)
        {
        }
        public DbSet<Test> Test { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureAdminPanelManagement(options =>
            {
                //options.TablePrefix = TablePrefix;
                //options.Schema = Schema;
            });
            builder.Entity<UserRejectionFromBankDto>().ToTable(nameof(UserRejectionFromBankDto), t => t.ExcludeFromMigrations());
            builder.Entity<CustomerOrderDb>().ToTable(nameof(CustomerOrderDb), t => t.ExcludeFromMigrations());
            builder.Entity<OrderRejectionTypeReadOnly>().ToTable(nameof(OrderRejectionTypeReadOnly), t => t.ExcludeFromMigrations());
            builder.Entity<OrderStatusTypeReadOnly>().ToTable(nameof(OrderStatusTypeReadOnly), t => t.ExcludeFromMigrations());
            builder.Entity<UserInfoDb>().ToTable(nameof(UserInfoDb), t => t.ExcludeFromMigrations());
            builder.Entity<UserRejectionAdvocacyDb>().ToTable(nameof(UserRejectionAdvocacyDb), t => t.ExcludeFromMigrations());
            builder.Entity<AdvocacyUsersFromBankDb>().ToTable(nameof(AdvocacyUsersFromBankDb), t => t.ExcludeFromMigrations());
            builder.Entity<ReportQuestionnaireDb>().ToTable(nameof(ReportQuestionnaireDb), t => t.ExcludeFromMigrations());
        }
    }
}