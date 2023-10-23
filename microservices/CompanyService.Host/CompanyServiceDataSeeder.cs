using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace CompanyService.Host
{
    public class CompanyServiceDataSeeder : IDataSeedContributor, ITransientDependency
    {

        public CompanyServiceDataSeeder()
        {

        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            await AddCompanyAsync();
        }

        private async Task AddCompanyAsync()
        {
           
        }
    }
}
