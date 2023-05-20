using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PaymentService.Host.EntityFrameworkCore
{
    public class PaymentServiceMigrationDbContextFactory : IDesignTimeDbContextFactory<PaymentServiceMigrationDbContext>
    {
        public PaymentServiceMigrationDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<PaymentServiceMigrationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("PaymentManagement"));

            return new PaymentServiceMigrationDbContext(builder.Options);
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
