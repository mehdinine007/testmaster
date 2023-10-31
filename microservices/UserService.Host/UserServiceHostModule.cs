#region NS
using IFG.Core.Caching;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using UserManagement.Application;
using UserManagement.EfCore.EntityFrameworkCore;
using UserManagement.HttpApi.UserManagement;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.Uow;
using UserService.Host.Infrastructures;
using Volo.Abp.Threading;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using IFG.Core.Extensions;
using UserManagement.EfCore.MongoDb;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.UserManagement.Implementations;
using Volo.Abp.RabbitMQ;
using Volo.Abp.EventBus.RabbitMq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IFG.Core.Utility.Security.Encyption;
using Volo.Abp.BackgroundJobs.Hangfire;
#endregion


namespace UserService.Host;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpEventBusRabbitMqModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(UserManagementApplicationModule),
    typeof(UserManagementHttpApiModule),
    typeof(UserManagementEfCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpMongoDbModule),
    typeof(AbpBackgroundJobsHangfireModule)
    )]
    
public class UserServiceHostModule : AbpModule
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

        if (configuration.GetValue<bool?>("SwaggerIsEnable") ?? false)
        {
            context.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "User Service API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
        }

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

        using var scope = context.Services.BuildServiceProvider();
        context.Services.AddEsaleResultWrapper();
        IdentityModelEventSource.ShowPII = true;
        //ConfigureHangfire(context, configuration);
        context.Services.AddMongoDbContext<UserManagementMongoDbContext>(options => options.AddDefaultRepositories(includeAllEntities: true));
        context.Services.AddMongoDbContext<UserManagementMongoDbContextWriteOnly>(options => options.AddDefaultRepositories(includeAllEntities: true));
        ConfigureHangfire(context, configuration);
        context.Services.AddGrpc();
        context.Services.EasyCaching(configuration, "RedisCache:ConnectionString");
       

    }

    private void ConfigureHangfire(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(configuration.GetConnectionString("UserHangfire"));
        });

    }
   

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
            endpoints.MapGrpcService<GetwayGrpcClient>();


        });
        app.UseAbpRequestLocalization(); //TODO: localization?
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Service API");
        });

        app.UseAuditing();
        app.UseHangfireDashboard("/hangfire");
        app.UseConfiguredEndpoints();
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
