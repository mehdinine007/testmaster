using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace BankService.Host
{
    public class BankServiceDataSeeder : IDataSeedContributor, ITransientDependency
    {

        public BankServiceDataSeeder()
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
