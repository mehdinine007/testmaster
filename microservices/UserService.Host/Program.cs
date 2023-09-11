using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using UserService.Host;

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(!string.IsNullOrEmpty(environmentName) ? $"appsettings.{environmentName}.json" : "appsettings.json")
    .AddEnvironmentVariables()
    .Build();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.WithProperty("Application", "UserService")
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/logs.txt")
    .WriteTo.Elasticsearch(
        new ElasticsearchSinkOptions(new Uri(configuration["ElasticSearch:Url"]))
        {
            AutoRegisterTemplate = true,
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
            IndexFormat = "msdemo-log-{0:yyyy.MM}"
        })
    .CreateLogger();

try
{
    Log.Information("Starting IdentityService.Host.");
    Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseAutofac()
                .UseSerilog()
                .Build()
                .Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "IdentityService.Host terminated unexpectedly!");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
