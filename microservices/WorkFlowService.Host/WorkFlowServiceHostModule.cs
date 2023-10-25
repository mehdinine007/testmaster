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
using Volo.Abp.Uow;
using Microsoft.IdentityModel.Logging;
using Microsoft.Extensions.Configuration;
using EasyCaching.Host.Extensions;
using Microsoft.EntityFrameworkCore;
using WorkFlowManagement.Application;
using GatewayManagement.HttpApi;
using WorkFlowManagement.EntityFrameworkCore;
using WorkFlowService.Host.Infrastructures;
using WorkFlowManagement.Application.WorkFlowManagement.Grpc;
using Esale.Core.Utility.Security.Encyption;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WorkFlowService.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcModule),
        //typeof(AbpEventBusRabbitMqModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        //typeof(AbpMongoDbModule),
        //typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        //typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(WorkFlowManagementApplicationModule),
        typeof(WorkFlowManagementHttpApiModule),
        typeof(WorkFlowManagementEntityFrameworkCoreModule),
        //typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule)

        )]
    public class WorkFlowServiceHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            context.Services.Configure<AppSecret>(configuration.GetSection("Authentication:JwtBearer"));

            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(opt =>
                        {
                            opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidIssuer = configuration["Authentication:JwtBearer:Issuer"],
                                ValidAudience = configuration["Authentication:JwtBearer:Audience"],
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(configuration["Authentication:JwtBearer:SecurityKey"])
                            };
                        });

            context.Services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "WorkFlow Service API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                });
          

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
            //context.Services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = configuration["Redis:Configuration"];
            //});

            Configure<AbpAuditingOptions>(options =>
            {
                options.IsEnabledForGetRequests = true;
            });
            //Configure<AbpExceptionHandlingOptions>(options =>
            //{
            //    options.SendExceptionsDetailsToClients = true;
            //    options.SendStackTraceToClients = true;
            //});
            //Configure<AbpAuditingOptions>(options =>
            //{
            //    options.IsEnabled = false; //Disables the auditing system
            //});


            //context.Services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = configuration["RedisCache:ConnectionString"];
            //});

            using var scope = context.Services.BuildServiceProvider();
        


           
            IdentityModelEventSource.ShowPII = true;
            //ConfigureHangfire(context, configuration);

            context.Services.AddGrpc();
            context.Services.EasyCaching(configuration, "RedisCache:ConnectionString");
            //context.Services.AddMongoDbContext<OrderManagementMongoDbContext>(options =>
            //{
            //    options.AddDefaultRepositories(includeAllEntities: true);
            //});

            //var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            //context.Services.AddDataProtection()
            //    .PersistKeysToStackExchangeRedis(redis, "MsDemo-DataProtection-Keys");
        }
        //private void ConfigureHangfire(ServiceConfigurationContext context, IConfiguration configuration)
        //{
        //    //context.Services.AddHangfire(config =>
        //    //{
        //    //    config.UseSqlServerStorage(configuration.GetConnectionString("OrderHangfire"));
        //    //});

        //}

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAbpClaimsMap();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<UserGrpcClientService>();

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
