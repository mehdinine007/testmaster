
using ReportManagement.Domain.Shared.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using WorkFlowManagement.Application.Contracts;


namespace GatewayManagement.Application.Contracts
{
    public class ReportManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var productManagementGroup = context.AddGroup(ReportManagementPermissions.GroupName, L("Permission:ProductManagement"));

            var products = productManagementGroup.AddPermission(ReportManagementPermissions.Products.Default, L("Permission:Products"));
            products.AddChild(ReportManagementPermissions.Products.Update, L("Permission:Edit"));
            products.AddChild(ReportManagementPermissions.Products.Delete, L("Permission:Delete"));
            products.AddChild(ReportManagementPermissions.Products.Create, L("Permission:Create"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ReportManagementResource>(name);
        }
    }
}