using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace PaymentService.Host
{
    public class PaymentServiceDataSeeder : IDataSeedContributor, ITransientDependency
    {

        public PaymentServiceDataSeeder()
        {

        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            await AddPaymentsAsync();
        }

        private async Task AddPaymentsAsync()
        {
           
        }
    }
}
