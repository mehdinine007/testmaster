﻿using OrderManagement.Domain.Shared;
using OrderManagement.Localization;
using Volo.Abp.Application;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace OrderManagement.Application.Contracts
{
    [DependsOn(
        typeof(OrderManagementDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class OrderManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<OrderManagementApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<OrderManagementResource>()
                    .AddVirtualJson("/OrderManagement/Localization/ApplicationContracts");
            });
        }
    }
}
