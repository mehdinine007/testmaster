#region NS
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

#endregion

namespace UserService.Host.EntityFrameworkCore
{
    public class UserServiceMigrationDbContextFactory : IDesignTimeDbContextFactory<UserServiceMigrationDbContext>
    {
        public UserServiceMigrationDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<UserServiceMigrationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new UserServiceMigrationDbContext(builder.Options);
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
