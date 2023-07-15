using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
//using Volo.Abp.MultiTenancy;
//using Volo.Abp.PermissionManagement.EntityFrameworkCore;
//using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.Threading;
using GatewayManagement.Application;
using GatewayManagement.HttpApi;
using GatewayManagement.EntityFrameworkCore;
using ProtoBuf.Grpc.Server;
using Volo.Abp.Uow;
using Microsoft.EntityFrameworkCore;
using GatewayManagement.Application.Servicess;

namespace GatewayService.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcModule),
        //typeof(AbpEventBusRabbitMqModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        //typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        //typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        //typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(GatewayManagementApplicationModule),
        typeof(GatewayManagementHttpApiModule),
        typeof(GatewayManagementEntityFrameworkCoreModule),
        //typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule)
        )]
    public class GatewayServiceHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            //Configure<AbpMultiTenancyOptions>(options =>
            //{
            //    options.IsEnabled = MsDemoConsts.IsMultiTenancyEnabled;
            //});

            //context.Services.AddAuthentication("Bearer")
            //    .AddIdentityServerAuthentication(options =>
            //    {
            //        options.Authority = configuration["AuthServer:Authority"];
            //        options.ApiName = configuration["AuthServer:ApiName"];
            //        options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
            //    });

            context.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo {Title = "Gateway Service API", Version = "v1"});
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
            Configure<AbpUnitOfWorkDefaultOptions>(options =>
            {
                options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
            });
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
               

            });


            context.Services.AddCodeFirstGrpc();

            //context.Services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = configuration["Redis:Configuration"];
            //});

            //Configure<AbpAuditingOptions>(options =>
            //{
            //    options.IsEnabled = false;
            //    options.ApplicationName = "GatewayService";
            //});

            //var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            //context.Services.AddDataProtection()
            //    .PersistKeysToStackExchangeRedis(redis, "MsDemo-DataProtection-Keys");
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAbpClaimsMap();

            //if (MsDemoConsts.IsMultiTenancyEnabled)
            //{
            //    app.UseMultiTenancy();
            //}

            app.UseConfiguredEndpoints(endpoints =>
            {
                endpoints.MapGet("/grpc", () => "grpc");
                endpoints.MapGrpcService<GatewayGrpcServiceProvider>();
            });

            app.UseAbpRequestLocalization(); //TODO: localization?
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway Service API");
            });
            app.UseAuditing();
            app.UseConfiguredEndpoints();

            //TODO: Problem on a clustered environment
            AsyncHelper.RunSync(async () =>
            {
                using (var scope = context.ServiceProvider.CreateScope())
                {
                    await scope.ServiceProvider
                        .GetRequiredService<IDataSeeder>()
                        .SeedAsync();
                }
            });
        }
    }
}
