using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WorkFlowService.Host.EntityFrameworkCore
{
    public class WorkFlowServiceMigrationDbContextFactory : IDesignTimeDbContextFactory<WorkFlowServiceMigrationDbContext>
    {
        public WorkFlowServiceMigrationDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<WorkFlowServiceMigrationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("WorkflowManagement"));

            return new WorkFlowServiceMigrationDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
