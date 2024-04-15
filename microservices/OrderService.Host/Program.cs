﻿using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Stimulsoft.Base;

namespace OrderService.Host
{
    public class Program
    {
        public static int Main(string[] args)
        {
            //TODO: Temporary: it's not good to read appsettings.json here just to configure logging
            //StiLicense.Key = "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";
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
                .Enrich.WithProperty("Application", "OrderService")
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
                CreateHostBuilder(args).Build().Run();
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
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseAutofac()
                .UseSerilog();
    }
}
