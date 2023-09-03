
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



            builder.Entity<Process>(entity =>
            {
                entity.ToTable("Processes", "Flow");
                entity.HasOne<Scheme>(x => x.Scheme)
                    .WithMany(x => x.Processes)
                    .HasForeignKey(x => x.SchemeId)
                    .OnDelete(DeleteBehavior.ClientCascade); ;

                entity.HasOne<Activity>(x => x.Activity)
                   .WithMany(x => x.Processes)
                   .HasForeignKey(x => x.ActivityId);
                entity.HasOne<Activity>(x => x.PreviousActivity)
                  .WithMany(x => x.PreviousProcesses)
                  .HasForeignKey(x => x.PreviousActivityId);

                entity.HasOne<OrganizationChart>(x => x.OrganizationChart)
                 .WithMany(x => x.Processes)
                 .HasForeignKey(x => x.OrganizationChartId)
                  .OnDelete(DeleteBehavior.ClientCascade);
                entity.HasOne<OrganizationChart>(x => x.CreatedOrganizationChart)
                .WithMany(x => x.CreatedProcesses)
                .HasForeignKey(x => x.CreatedOrganizationChartId)
                 .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne<OrganizationChart>(x => x.PreviousOrganizationChart)
               .WithMany(x => x.PreviousProcesses)
               .HasForeignKey(x => x.PreviousOrganizationChartId)
                .OnDelete(DeleteBehavior.ClientCascade);



            });
            builder.Entity<Inbox>(entity =>
            {
                entity.ToTable("Inboxes", "Flow");
                entity.HasOne<Process>(x => x.Process)
                    .WithMany(x => x.Inboxes)
                    .HasForeignKey(x => x.ProcessId);


                entity.HasOne<OrganizationChart>(x => x.OrganizationChart)
                    .WithMany(x => x.Inboxes)
                    .HasForeignKey(x => x.OrganizationChartId)
                    .OnDelete(DeleteBehavior.ClientCascade);


                entity.HasOne<OrganizationPosition>(x => x.OrganizationPosition)
                   .WithMany(x => x.Inboxes)
                   .HasForeignKey(x => x.OrganizationPositionId)
                   .OnDelete(DeleteBehavior.ClientCascade);

            });

        }
    }
}