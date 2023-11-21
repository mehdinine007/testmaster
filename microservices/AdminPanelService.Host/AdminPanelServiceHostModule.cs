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
using Microsoft.EntityFrameworkCore;
using AdminPanelManagement.HttpApi;
using AdminPanelManagement.EntityFrameworkCore;
using AdminPanelManagement.Application;
using AdminPanelService.Host.Infrastructures;
using AdminPanelManagement.Application.AdminPanelManagement.Grpc;
using IFG.Core.Caching;
using IFG.Core.Extensions;
using IFG.Core.Utility.Security.Encyption;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Licence;
using System.Collections.Generic;

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
        typeof(AdminPanelManagementApplicationModule),
        typeof(AdminPanelManagementHttpApiModule),
        typeof(AdminPanelManagementEntityFrameworkCoreModule),
        //typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule)

        )]
    public class AdminPanelServiceHostModule : AbpModule
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
                    var version = AppLicence.GetVersion(configuration.GetSection("Licence:SerialNumber").Value).Version;
                    context.Services.AddSwaggerGen(options =>
                    {
                        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Admin Panel API", Version = version });
                        options.DocInclusionPredicate((docName, description) => true);
                        options.CustomSchemaIds(type => type.FullName);
                        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                        {
                            Name = "Authorization",
                            Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.Http,
                            Scheme = "Bearer",
                        });
                        options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });
                    });
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

            context.Services.AddEsaleResultWrapper();

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

            //if (MsDemoConsts.IsMultiTenancyEnabled)
            //{
            //    app.UseMultiTenancy();
            //}
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGrpcService<CompanyGrpcClient>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ReportGrpcClientService>();

            });


            app.UseAbpRequestLocalization(); //TODO: localization?
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Service API");
            });

            app.UseAuditing();
            //app.UseHangfireDashboard();
            //app.UseHangfireDashboard("/hangfire");
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
