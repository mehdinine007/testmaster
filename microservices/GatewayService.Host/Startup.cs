using Volo.Abp;
using Esale.Core.IOC;
using GatewayService.Host.Infrastructures.Extensions;
using Volo.Abp.Auditing;
using GatewayService.Host.Infrastructures.Middlewares;

namespace GatewayService.Host
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<GatewayServiceHostModule>();
            var configurations = services.GetConfiguration();
            if (configurations["IsElkEnabled"] == "1")
            {
                services.ElkNest(configurations, configurations["ElkIndexName"]);
            }
            else
            {
                services.AddScoped<IAuditingStore, AuditingStoreDb>();
            }
            ServiceTool.Create(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            app.InitializeApplication();
        }
    }
}
