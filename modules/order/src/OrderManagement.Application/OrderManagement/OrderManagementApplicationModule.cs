using Abp.Domain.Uow;
using OrderManagement.Application.Contracts;
using OrderManagement.Domain;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace OrderManagement.Application;
[DependsOn(
    typeof(OrderManagementDomainModule),
    typeof(OrderManagementApplicationContractsModule),
    typeof(AbpAutoMapperModule)
    )]
public class OrderManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<OrderManagementApplicationAutoMapperProfile>(validate: true);
        });
    }

    public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
    {
        base.OnPreApplicationInitialization(context);


        //TODO: configure redis cache

        //ServiceConfigurationContext.Caching.UseRedis(options =>
        //{
        //    options.ConnectionString = _appConfiguration["RedisCache:ConnectionString"];
        //    options.DatabaseId = _appConfiguration.GetValue<int>("RedisCache:DatabaseId");
        //});
        //Configuration.Caching.Configure("AuthenticateTejarat", cache =>
        //{
        //    cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(30);
        //});
        //Configuration.Caching.Configure("AdvocacyConfirm", cache =>
        //{
        //    cache.DefaultSlidingExpireTime = TimeSpan.FromHours(24);
        //});
        //Configuration.Caching.Configure("UserProf", cache =>
        //{
        //    cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(30);
        //});
        //Configuration.Caching.Configure("SMS", cache =>
        //{
        //    cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(4);
        //});
        //Configuration.Caching.Configure("UserRejection", cache =>
        //{
        //    cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(20);
        //});
        //Configuration.Caching.Configure("SaleDetail", cache =>
        //{
        //    cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(20);
        //});
        //Configuration.Caching.Configure("UserLogin", cache =>
        //{
        //    cache.DefaultSlidingExpireTime = TimeSpan.FromDays(1);
        //});
        //Configuration.UnitOfWork.OverrideFilter(AbpDataFilters.MayHaveTenant, false);
        //Configuration.UnitOfWork.OverrideFilter(AbpDataFilters.MustHaveTenant, false);


    }
}
