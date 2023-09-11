using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace WorkFlowService.Host
{
    public class WorkFolwServiceDataSeeder : IDataSeedContributor, ITransientDependency
    {

        public WorkFolwServiceDataSeeder()
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
