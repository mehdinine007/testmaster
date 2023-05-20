using PaymentManagement.Domain.Shared.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace PaymentManagement.Application.Contracts
{
    public class PaymentManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var productManagementGroup = context.AddGroup(PaymentManagementPermissions.GroupName, L("Permission:ProductManagement"));

            var products = productManagementGroup.AddPermission(PaymentManagementPermissions.Products.Default, L("Permission:Products"));
            products.AddChild(PaymentManagementPermissions.Products.Update, L("Permission:Edit"));
            products.AddChild(PaymentManagementPermissions.Products.Delete, L("Permission:Delete"));
            products.AddChild(PaymentManagementPermissions.Products.Create, L("Permission:Create"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PaymentManagementResource>(name);
        }
    }
}