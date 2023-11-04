using Microsoft.EntityFrameworkCore;
using WorkFlowManagement.Domain;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;


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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureAdminPanelManagement(options =>
            {
                //options.TablePrefix = TablePrefix;
                //options.Schema = Schema;
            });
        }
    }
}