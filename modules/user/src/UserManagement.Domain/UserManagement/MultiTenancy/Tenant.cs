using Abp.MultiTenancy;
using UserManagement.Domain.Authorization.Users;

namespace UserManagement.Domain.UserManagement.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
