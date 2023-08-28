using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using WorkFlowManagement.EntityFrameworkCore;

namespace WorkFlowService.Host.EntityFrameworkCore
{
    public class WorkFlowServiceMigrationDbContext : AbpDbContext<WorkFlowServiceMigrationDbContext>
    {
        public WorkFlowServiceMigrationDbContext(
            DbContextOptions<WorkFlowServiceMigrationDbContext> options
            ) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureWorkFlowManagement();
        }
    }
}
