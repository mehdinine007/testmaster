using Microsoft.EntityFrameworkCore;
using WorkFlowManagement.Domain;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace WorkFlowManagement.EntityFrameworkCore
{
    [ConnectionStringName("WorkFlowManagement")]
    public class WorkFlowManagementDbContext : AbpDbContext<WorkFlowManagementDbContext>, IWorkFlowManagementDbContext
    {
        public static string TablePrefix { get; set; } = WorkFlowManagementConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = WorkFlowManagementConsts.DefaultDbSchema;

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