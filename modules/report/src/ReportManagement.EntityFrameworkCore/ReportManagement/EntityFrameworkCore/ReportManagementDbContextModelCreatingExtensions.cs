
using Microsoft.EntityFrameworkCore;
using ReportManagement.Domain.ReportManagement;
using Volo.Abp;
using Volo.Abp.Domain.Entities;


namespace ReportManagement.EntityFrameworkCore
{
    public static class ReportManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureReportManagement(
            this ModelBuilder builder,
            Action<ReportManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));
            var options = new ReportManagementModelBuilderConfigurationOptions();
            optionsAction?.Invoke(options);


            builder.Entity<Dashboard>(entity =>
            {
                entity.ToTable("Dashboards")
                .HasMany(e => e.Widgets)
        .WithMany(e => e.Dashboards)
        .UsingEntity<DashboardWidget>();

            });


            builder.Entity<Dashboard>(entity =>
            {
                entity.ToTable("Dashboards");
                entity.Property(x => x.Title)
                    .HasMaxLength(100);
            });
            builder.Entity<Widget>(entity =>
            {
                entity.ToTable("Widgets");
                entity.Property(x => x.Title)
                    .HasMaxLength(100);
            });

        }
    }
}