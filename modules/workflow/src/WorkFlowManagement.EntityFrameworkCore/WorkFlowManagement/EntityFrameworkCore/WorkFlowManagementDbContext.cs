using Microsoft.EntityFrameworkCore;
using WorkFlowManagement.Domain;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using WorkFlowManagement.Domain.WorkFlowManagement;

namespace WorkFlowManagement.EntityFrameworkCore
{
    [ConnectionStringName("WorkflowManagement")]
    public class WorkFlowManagementDbContext : AbpDbContext<WorkFlowManagementDbContext>, IWorkFlowManagementDbContext
    {
        public static string TablePrefix { get; set; } = WorkFlowManagementConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = WorkFlowManagementConsts.DefaultDbSchema;
        public DbSet<OrganizationChart> OrganizationCharts { get; set ; }
        public DbSet<OrganizationPosition> OrganizationPositions { get; set; }
        public DbSet<WorkFlowRole> WorkFlowRoles { get; set; }
        public DbSet<WorkFlowRoleChart> WorkFlowRoleCharts { get; set; }


        public WorkFlowManagementDbContext(DbContextOptions<WorkFlowManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureWorkFlowManagement(options =>
            {
                //options.TablePrefix = TablePrefix;
                //options.Schema = Schema;
            });
        }
    }
}