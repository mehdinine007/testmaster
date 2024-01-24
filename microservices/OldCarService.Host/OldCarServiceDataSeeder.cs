using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace OldCarService.Host
{
    public class OldCarServiceDataSeeder : IDataSeedContributor, ITransientDependency
    {

        public OldCarServiceDataSeeder()
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
