using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OldCarService.Host.EntityFrameworkCore;

namespace OldCar.Host.EntityFrameworkCore
{
    public class OldCarServiceMigrationDbContextFactory : IDesignTimeDbContextFactory<OldCarServiceMigrationDbContext>
    {
        public OldCarServiceMigrationDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<OldCarServiceMigrationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("CompanyManagement"));

            return new OldCarServiceMigrationDbContext(builder.Options);
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
