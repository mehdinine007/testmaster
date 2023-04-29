using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace OrderService.Host.EntityFrameworkCore
{
    public class OrderServiceMigrationDbContextFactory : IDesignTimeDbContextFactory<OrderServiceMigrationDbContext>
    {
        public OrderServiceMigrationDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<OrderServiceMigrationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("OrderManagement"));

            return new OrderServiceMigrationDbContext(builder.Options);
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
