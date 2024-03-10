using IFG.Core.DataAccess.Migration;
using IFG.Core.IOC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Migrator
{
    public class Program
    {
        private static bool _quietMode;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build();
            var migrateExecuter = new MigrateExecuter(Licence.AppLicence.GetVersion("").FixVersion);
            migrateExecuter.Run();
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();

        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureServices(service =>
                {
                    ServiceTool.Create(service);
                })
            .ConfigureAppConfiguration((context, builder) =>
            {
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                builder.SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile(!string.IsNullOrEmpty(environmentName) ? $"appsettings.{environmentName}.json" : "appsettings.json");
            });


    }
}
