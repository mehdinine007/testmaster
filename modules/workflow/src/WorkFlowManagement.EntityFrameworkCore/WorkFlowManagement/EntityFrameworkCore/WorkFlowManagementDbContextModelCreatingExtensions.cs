using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using WorkFlowManagement.Domain.WorkFlowManagement;
using Volo.Abp.EntityFrameworkCore.Modeling;

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
                entity.ToTable(nameof(OrganizationChart));

                entity.HasOne<OrganizationChart>(x => x.Parent)
                    .WithMany(x => x.Childrens)
                    .HasForeignKey(x => x.ParentId)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.Property(x => x.Code)
                    .HasMaxLength(250);

                entity.Property(x => x.Title)
                    .HasMaxLength(250);
            });
        }
    }
}