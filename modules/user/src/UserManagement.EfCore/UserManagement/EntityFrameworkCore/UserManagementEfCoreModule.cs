using Microsoft.Extensions.DependencyInjection;
using UserManagement.Domain.UserManagement;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace UserManagement.EfCore.EntityFrameworkCore
{
    [DependsOn(
        typeof(UserManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class UserManagementEfCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<UsermanagementDbContext>(opt =>
            {
                opt.AddDefaultRepositories(includeAllEntities: true);
            });
        }
    }
}
