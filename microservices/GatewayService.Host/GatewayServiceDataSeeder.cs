using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace GatewayService.Host
{
    public class GatewayServiceDataSeeder : IDataSeedContributor, ITransientDependency
    {

        public GatewayServiceDataSeeder()
        {

        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            await AddGatewaysAsync();
        }

        private async Task AddGatewaysAsync()
        {
           
        }
    }
}
