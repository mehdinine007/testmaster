using System.IO;
using BankService.Host.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BankService.Host.EntityFrameworkCore
{
    public class BankServiceMigrationDbContextFactory : IDesignTimeDbContextFactory<BankServiceMigrationDbContext>
    {
        public BankServiceMigrationDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<BankServiceMigrationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("CompanyManagement"));

            return new BankServiceMigrationDbContext(builder.Options);
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
