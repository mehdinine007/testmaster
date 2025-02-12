﻿using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace OrderService.Host
{
    public class OrderServiceDataSeeder : IDataSeedContributor, ITransientDependency
    {

        public OrderServiceDataSeeder()
        {

        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            await AddOrdersAsync();
        }

        private async Task AddOrdersAsync()
        {
           
        }
    }
}
