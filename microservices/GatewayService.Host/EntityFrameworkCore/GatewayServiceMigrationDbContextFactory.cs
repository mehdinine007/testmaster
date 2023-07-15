using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GatewayService.Host.EntityFrameworkCore
{
    public class GatewayServiceMigrationDbContextFactory : IDesignTimeDbContextFactory<GatewayServiceMigrationDbContext>
    {
        public GatewayServiceMigrationDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<GatewayServiceMigrationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("GatewayManagement"));

            return new GatewayServiceMigrationDbContext(builder.Options);
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
