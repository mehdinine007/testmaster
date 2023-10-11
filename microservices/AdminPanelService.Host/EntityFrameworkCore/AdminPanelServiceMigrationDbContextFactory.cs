using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AdminPanelService.Host.EntityFrameworkCore
{
    public class AdminPanelServiceMigrationDbContextFactory : IDesignTimeDbContextFactory<AdminPanelServiceMigrationDbContext>
    {
        public AdminPanelServiceMigrationDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<AdminPanelServiceMigrationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("adminpanelManagement"));

            return new AdminPanelServiceMigrationDbContext(builder.Options);
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
