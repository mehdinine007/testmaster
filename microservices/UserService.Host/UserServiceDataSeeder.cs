#region NS
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
#endregion


namespace UserService.Host
{
    public class UserServiceDataSeeder : IDataSeedContributor, ITransientDependency
    {

        public UserServiceDataSeeder()
        {

        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            await AddUsersAsync();
        }

        private async Task AddUsersAsync()
        {
           
        }
    }
}
