using Abp.Authorization;
using UserManagement.Domain.Authorization.Roles;
using UserManagement.Domain.Authorization.Users;

namespace UserManagement.Domain.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
