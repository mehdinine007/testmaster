using Microsoft.EntityFrameworkCore;
using ReportManagement.Domain.ReportManagement;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;


namespace ReportManagement.EntityFrameworkCore
{
    [ConnectionStringName("ReportManagement")]
    public class ReportManagementDbContext : AbpDbContext<ReportManagementDbContext>, IReportManagementDbContext
    {
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<Widget> Widgets { get; set; }
        public DbSet<DashboardWidget> DashboardWidgets { get; set; }


        public ReportManagementDbContext(DbContextOptions<ReportManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureReportManagement(options =>
            {
                //options.TablePrefix = TablePrefix;
                //options.Schema = Schema;
            });
        }
    }
}