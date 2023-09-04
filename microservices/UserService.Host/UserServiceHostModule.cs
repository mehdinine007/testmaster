#region NS
using EasyCaching.Host.Extensions;
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
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.MongoDB;
using Esale.Core.Extensions;
using UserManagement.EfCore.MongoDb;
#endregion


namespace UserService.Host;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(UserManagementApplicationModule),
    typeof(UserManagementHttpApiModule),
    typeof(UserManagementEfCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpMongoDbModule)
    )]
    
public class UserServiceHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        context.Services.Configure<AppSecret>(configuration.GetSection("Authentication:JwtBearer"));

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
        context.Services.AddMongoDbContext<UserManagementMongoDbContext>(options =>
        {
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        context.Services.AddGrpc();
        context.Services.EasyCaching(configuration, "RedisCache:ConnectionString");
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAbpClaimsMap();


        app.UseAbpRequestLocalization(); //TODO: localization?
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "User Service API");
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
