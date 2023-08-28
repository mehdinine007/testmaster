
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using WorkFlowManagement.Application.Contracts;
using WorkFlowManagement.Domain.Shared.Localization;

namespace GatewayManagement.Application.Contracts
{
    public class WorkFlowManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var productManagementGroup = context.AddGroup(WorkFlowManagementPermissions.GroupName, L("Permission:ProductManagement"));

            var products = productManagementGroup.AddPermission(WorkFlowManagementPermissions.Products.Default, L("Permission:Products"));
            products.AddChild(WorkFlowManagementPermissions.Products.Update, L("Permission:Edit"));
            products.AddChild(WorkFlowManagementPermissions.Products.Delete, L("Permission:Delete"));
            products.AddChild(WorkFlowManagementPermissions.Products.Create, L("Permission:Create"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<WorkFlowManagementResource>(name);
        }
    }
}