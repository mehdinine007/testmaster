using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ReportService.Host.EntityFrameworkCore
{
    public class ReportServiceMigrationDbContextFactory : IDesignTimeDbContextFactory<ReportServiceMigrationDbContext>
    {
        public ReportServiceMigrationDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ReportServiceMigrationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("ReportManagement"));

            return new ReportServiceMigrationDbContext(builder.Options);
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
