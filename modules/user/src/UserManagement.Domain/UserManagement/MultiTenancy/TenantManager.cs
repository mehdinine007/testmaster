using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.UserManagement.Editions;

namespace UserManagement.Domain.UserManagement.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository, 
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository, 
            EditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore) 
            : base(
                tenantRepository, 
                tenantFeatureRepository, 
                editionManager,
                featureValueStore)
        {
        }
    }
}
