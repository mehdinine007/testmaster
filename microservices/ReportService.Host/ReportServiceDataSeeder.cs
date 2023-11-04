using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace ReportService.Host
{
    public class ReportServiceDataSeeder : IDataSeedContributor, ITransientDependency
    {

        public ReportServiceDataSeeder()
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
