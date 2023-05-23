using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
//using Volo.Abp.MultiTenancy;
//using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.Threading;
using OrderManagement.Application;
using OrderManagement.HttpApi;
using OrderManagement.EfCore;
using OrderService.Host.Infrastructures;
using OrderManagement.Application.OrderManagement.Implementations;
using Volo.Abp.Uow;
using Microsoft.IdentityModel.Logging;
using Volo.Abp.Uow;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Microsoft.Extensions.Configuration;

namespace OrderService.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcModule),
        //typeof(AbpEventBusRabbitMqModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        //typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        //typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(OrderManagementApplicationModule),
        typeof(OrderManagementHttpApiModule),
        typeof(OrderManagementEntityFrameworkCoreModule)
        //typeof(AbpAspNetCoreMultiTenancyModule),
        //typeof(AbpTenantManagementEntityFrameworkCoreModule)
        )]
    public class OrderServiceHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            context.Services.Configure<AppSecret>(configuration.GetSection("Authentication:JwtBearer"));
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
            if (configuration.GetValue<bool?>("SwaggerIsEnable") ?? false)
            {
                context.Services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Order Service API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                });
            }

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });
            Configure<AbpUnitOfWorkDefaultOptions>(options =>
            {
                options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
            });
            context.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:Configuration"];
            });

            Configure<AbpAuditingOptions>(options =>
            {
                options.IsEnabledForGetRequests = true;
            });
            Configure<AbpExceptionHandlingOptions>(options =>
            {
                options.SendExceptionsDetailsToClients = true;
                options.SendStackTraceToClients = true;
            });
            //Configure<AbpAuditingOptions>(options =>
            //{
            //    options.IsEnabled = false; //Disables the auditing system
            //});


            context.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["RedisCache:ConnectionString"];
            });

            using var scope = context.Services.BuildServiceProvider();
            var service = scope.GetRequiredService<IActionResultWrapperFactory>();


            context.Services.AddControllers(x =>
            {
                x.Filters.Add(new EsaleResultFilter(service));
            });
            //IdentityModelEventSource.ShowPII = true;

            context.Services.AddGrpc();
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
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GrpcTestService>();
            });
            app.UseAbpRequestLocalization(); //TODO: localization?
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Service API");
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
