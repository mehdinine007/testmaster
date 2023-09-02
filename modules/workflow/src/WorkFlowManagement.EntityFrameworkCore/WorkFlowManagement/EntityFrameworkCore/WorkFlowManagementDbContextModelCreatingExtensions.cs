using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using WorkFlowManagement.Domain.WorkFlowManagement;
using Volo.Abp.EntityFrameworkCore.Modeling;
using System.Reflection.Emit;

namespace WorkFlowManagement.EntityFrameworkCore
{
    public static class WorkFlowManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureWorkFlowManagement(
            this ModelBuilder builder,
            Action<WorkFlowManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new WorkFlowManagementModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);

            builder.Entity<OrganizationChart>(entity =>
            {
                entity.ToTable("OrganizationCharts");

                entity.HasOne<OrganizationChart>(x => x.Parent)
                    .WithMany(x => x.Childrens)
                    .HasForeignKey(x => x.ParentId)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.Property(x => x.Code)
                    .HasMaxLength(250);

                entity.Property(x => x.Title)
                    .HasMaxLength(250);



            });


            builder.Entity<OrganizationPosition>(entity =>
            {
                entity.ToTable("OrganizationPositions");
                entity.HasOne<OrganizationChart>(x => x.OrganizationChart)
                    .WithMany(x => x.OrganizationPositions)
                    .HasForeignKey(x => x.OrganizationChartId);
            });

            builder.Entity<WorkFlowRoleChart>(entity =>
            {
                entity.ToTable("WorkFlowRoleCharts");

                entity.HasOne<OrganizationChart>(o => o.OrganizationChart)
                    .WithMany(w => w.WorkFlowRoleCharts)
                    .HasForeignKey(o => o.OrganizationChartId);

                entity.HasOne<WorkFlowRole>(o => o.WorkFlowRole)
                 .WithMany(w => w.WorkFlowRoleCharts)
                 .HasForeignKey(o => o.WorkFlowRoleId);

            });

         





        }
    }
}