using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;
using UserManagement.Domain.UserManagement;

namespace UserManagement.Domain.Authorization
{
    public class EsaleAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(PermissionNames.Pages_Bank_Advocacy, L("Create_Advocacy_Users"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(PermissionNames.Pages_Order, L("Create_Order"), multiTenancySides: MultiTenancySides.Host);

            

    }

    private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, EsaleConsts.LocalizationSourceName);
        }
    }
}
