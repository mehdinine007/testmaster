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
                entity.ToTable("OrganizationCharts","Flow");

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
                entity.ToTable("OrganizationPositions", "Flow");
                entity.HasOne<OrganizationChart>(x => x.OrganizationChart)
                    .WithMany(x => x.OrganizationPositions)
                    .HasForeignKey(x => x.OrganizationChartId);
            });

            builder.Entity<RoleOrganizationChart>(entity =>
            {
                entity.ToTable("RoleOrganizationChart", "Flow");

                entity.HasOne<OrganizationChart>(o => o.OrganizationChart)
                    .WithMany(w => w.RoleOrganizationCharts)
                    .HasForeignKey(o => o.OrganizationChartId);

                entity.HasOne<Role>(o => o.Role)
                 .WithMany(w => w.RoleOrganizationCharts)
                 .HasForeignKey(o => o.RoleId);

            });
            builder.Entity<Activity>(entity =>
            {
                entity.ToTable("Activities", "Flow");
                entity.HasOne<Scheme>(x => x.Scheme)
                    .WithMany(x => x.Activities)
                    .HasForeignKey(x => x.SchemeId);
            });

            builder.Entity<Transition>(entity =>
            {
                entity.ToTable("Transitions", "Flow");
                entity.HasOne<Activity>(x => x.ActivitySource)
                    .WithMany(x => x.SourceTransitions)
                    .HasForeignKey(x => x.ActivitySourceId)
                     .OnDelete(DeleteBehavior.ClientCascade); ;


                entity.HasOne<Activity>(x => x.ActivityTarget)
                   .WithMany(x => x.TargetTransitions)
                   .HasForeignKey(x => x.ActivityTargetId)
                  .OnDelete(DeleteBehavior.ClientCascade);
                entity.HasOne<Scheme>(x => x.Scheme)
                  .WithMany(x => x.Transitions)
                  .HasForeignKey(x => x.SchemeId);
            });


            builder.Entity<ActivityRole>(entity =>
            {
                entity.ToTable("ActivityRoles", "Flow");
                entity.HasOne<Activity>(x => x.Activity)
                    .WithMany(x => x.ActivityRoles)
                    .HasForeignKey(x => x.ActivityId);

                entity.HasOne<Role>(x => x.Role)
                   .WithMany(x => x.ActivityRoles)
                   .HasForeignKey(x => x.RoleId);

            });


        }
    }
}