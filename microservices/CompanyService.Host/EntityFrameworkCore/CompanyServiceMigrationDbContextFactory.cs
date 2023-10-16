using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CompanyService.Host.EntityFrameworkCore
{
    public class CompanyServiceMigrationDbContextFactory : IDesignTimeDbContextFactory<CompanyServiceMigrationDbContext>
    {
        public CompanyServiceMigrationDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<CompanyServiceMigrationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("CompanyManagement"));

            return new CompanyServiceMigrationDbContext(builder.Options);
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
