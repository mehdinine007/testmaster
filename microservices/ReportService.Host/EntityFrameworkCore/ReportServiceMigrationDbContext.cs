using Microsoft.EntityFrameworkCore;
using ReportManagement.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;



namespace ReportService.Host.EntityFrameworkCore
{
    public class ReportServiceMigrationDbContext : AbpDbContext<ReportServiceMigrationDbContext>
    {
        public ReportServiceMigrationDbContext(
            DbContextOptions<ReportServiceMigrationDbContext> options
            ) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureReportManagement();
        }
    }
}
