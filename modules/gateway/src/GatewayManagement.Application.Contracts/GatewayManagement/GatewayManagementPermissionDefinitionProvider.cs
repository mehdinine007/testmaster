using GatewayManagement.Domain.Shared.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace GatewayManagement.Application.Contracts
{
    public class GatewayManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var productManagementGroup = context.AddGroup(GatewayManagementPermissions.GroupName, L("Permission:ProductManagement"));

            var products = productManagementGroup.AddPermission(GatewayManagementPermissions.Products.Default, L("Permission:Products"));
            products.AddChild(GatewayManagementPermissions.Products.Update, L("Permission:Edit"));
            products.AddChild(GatewayManagementPermissions.Products.Delete, L("Permission:Delete"));
            products.AddChild(GatewayManagementPermissions.Products.Create, L("Permission:Create"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<GatewayManagementResource>(name);
        }
    }
}